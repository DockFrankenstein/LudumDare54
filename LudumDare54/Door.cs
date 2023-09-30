using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using LudumDare54.Player;
using qASIC;

namespace LudumDare54
{
    public class Door : AsyncScript
    {
        public StaticColliderComponent trigger;
        public TransformComponent teleportPoint;

        public event Action<PlayerMove> OnPlayerEnter;

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

                qDebug.Log("Player has entered a door", "blue");

                if (teleportPoint != null)
                    playerMove.Teleport(teleportPoint.WorldMatrix.TranslationVector);

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
    }
}
