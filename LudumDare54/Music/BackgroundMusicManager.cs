using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Audio;

namespace LudumDare54.Music
{
    public class BackgroundMusicManager : AsyncScript
    {
        public double minWaitTime = 120.0;
        public double maxWaitTime = 300.0;

        public Sound backgroundSound;
        public List<Sound> sounds = new List<Sound>();

        public float volume = 0.25f;
        public float soundClipsVolume = 0.25f;

        public override async Task Execute()
        {
            var random = new Random();

            var backgroundInstance = backgroundSound.CreateInstance();
            backgroundInstance.IsLooping = true;
            backgroundInstance.Volume = volume;
            await backgroundInstance.ReadyToPlay();
            backgroundInstance.Play();

            await WaitRandom();
            await PlaySound(sounds[0]);

            while (Game.IsRunning)
            {
                await WaitRandom();
                var sound = sounds[random.Next(0, sounds.Count)];
                await PlaySound(sound);
            }

            async Task WaitRandom()
            {
                var waitAmount = random.NextDouble() * (maxWaitTime - minWaitTime) + minWaitTime;
                await Task.Delay((int)Math.Round(waitAmount * 1000.0));
            }

            async Task PlaySound(Sound sound)
            {
                using (SoundInstance soundInstance = sound.CreateInstance())
                {
                    soundInstance.IsLooping = false;
                    soundInstance.Volume = volume;
                    await soundInstance.ReadyToPlay();
                    soundInstance.Play();
                    await Task.Delay((int)Math.Round(sound.TotalLength.TotalMilliseconds));
                }
            }
        }
    }
}
