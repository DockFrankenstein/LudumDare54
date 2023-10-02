using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using LudumDare54.Player;
using qASIC;
using LudumDare54.UI;

using Color = Stride.Core.Mathematics.Color;

namespace LudumDare54
{
    public class Door : AsyncScript
    {
        public const double TRANSITION_TO_DURATION = 0.4;
        public const double TRANSITION_FROM_DURATION = 0.5;

        public const float SOUND_VOLUME = 0.8f;

        public StaticColliderComponent trigger;
        public Room teleportRoom;
        public Color transitionColor = new Color(255, 255, 255, 255);

        public bool playSound = true;

        public event Action<PlayerMove> OnPlayerEnter;

        static Random DoorSoundEffectsRandom { get; } = new Random();

        public override async Task Execute()
        {
            while (Game.IsRunning)
            {
                Collision firstCollision = await trigger.NewCollision();

                PhysicsComponent otherCollider = trigger == firstCollision.ColliderA
                    ? firstCollision.ColliderB
                    : firstCollision.ColliderA;

                var playerMove = otherCollider.Entity.Get<PlayerMove>();

                if (playerMove == null)
                    continue;

                qDebug.Log(teleportRoom == null ?
                    "Player has entered a door" :
                    $"Player has entered room {teleportRoom.Entity.Name}", "blue");

                if (teleportRoom != null)
                {
                    FaderUI.Instance.Fade(transitionColor, TRANSITION_TO_DURATION, TRANSITION_FROM_DURATION, () =>
                    {
                        playerMove.Teleport(teleportRoom.startPoint.WorldMatrix.TranslationVector);
                    });
                }

                if (playSound)
                {
                    var sounds = otherCollider.Entity.Get<PlayerSounds>();
                    if (sounds != null)
                        PlaySound(sounds);
                }

                OnPlayerEnter?.Invoke(playerMove);

                //Wait for the entity to exit the trigger.
                Collision collision;

                do
                {
                    collision = await trigger.CollisionEnded();
                }
                while (collision != firstCollision);
            }
        }

        static void PlaySound(PlayerSounds sounds)
        {
            if (sounds.doorSounds.Count == 0)
                return;

            var index = DoorSoundEffectsRandom.Next(0, sounds.doorSounds.Count);

            var instance = sounds.doorSounds[index].CreateInstance();
            instance.IsLooping = false;
            instance.Volume = SOUND_VOLUME;
            instance.Play();
        }
    }
}
