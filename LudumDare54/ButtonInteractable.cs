using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using LudumDare54.Player;
using Stride.Core;
using Stride.Audio;

namespace LudumDare54
{
    public class ButtonInteractable : Interactable
    {
        public const float SFX_VOLUME = 0.5f;

        public Entity EntityToToggle;

        public SpriteComponent SpriteComponent;
        public ISpriteProvider EnabledSprite;

        public EndingTrigger ending;

        int _presses = 0;

        public List<Sound> sounds = new List<Sound>();

        static Random Random { get; } = new Random();

        public override void Interact(PlayerInteract player)
        {
            if (_presses > 20)
                return;

            SpriteComponent.SpriteProvider = EnabledSprite;
            if (_presses == 0)
                EntityToToggle.EnableAll(true, false);

            PlaySound();

            _presses++;

            if (_presses > 20)
                ending.Active = true;
        }

        void PlaySound()
        {
            if (sounds.Count == 0) return;

            var sound = sounds[Random.Next(0, sounds.Count)];

            var instance = sound.CreateInstance();

            instance.IsLooping = false;
            instance.Volume = SFX_VOLUME;
            instance.Play();
        }
    }
}
