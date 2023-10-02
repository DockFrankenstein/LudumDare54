using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Stride.Engine;
using qASIC;
using Newtonsoft.Json;
using Stride.Core.Mathematics;
using Stride.Games;

namespace LudumDare54
{
    public class GameSettings
    {
        public const string FILE_PATH = "game settings.json";

        public Game Game { get; set; }

        public GameSettingsData SettingsData { get; set; } = GameSettingsData.Default;

        public void Load()
        {
            LoadFile();
            LoadSettings();
        }

        public void LoadFile()
        {
            try
            {
                if (!File.Exists(FILE_PATH))
                {
                    qDebug.Log("There is no game settings file, using default preferences.", "darkyellow");
                    return;
                }

                var txt = File.ReadAllText(FILE_PATH);

                JsonConvert.PopulateObject(txt, SettingsData);
            }
            catch (Exception e)
            {
                qDebug.LogError($"There was an error while loading game settings: {e}");
            }
        }

        public void LoadSettings()
        {
            var screenBounds = Game.GraphicsDevice.Adapter.Outputs[0].DesktopBounds;
            Game.Window.IsBorderLess = SettingsData.fullscreen;

            switch (SettingsData.fullscreen)
            {
                case true:
                    Game.Window.Position = new Int2(0, 0);

                    Game.Window.SetSize(new Int2(screenBounds.Width, screenBounds.Height));
                    break;
                case false:
                    Game.Window.SetSize(new Int2(SettingsData.resolutionHorizontal != 0 ?
                        SettingsData.resolutionHorizontal :
                        1280,
                        SettingsData.resolutionVertical != 0 ?
                        SettingsData.resolutionVertical :
                        720));

                    break;
            }

            Game.GraphicsDeviceManager.PreferredBackBufferWidth = SettingsData.resolutionHorizontal == 0 ? 
                Game.Window.ClientBounds.Width : 
                SettingsData.resolutionHorizontal;

            Game.GraphicsDeviceManager.PreferredBackBufferHeight = SettingsData.resolutionVertical == 0 ?
                Game.Window.ClientBounds.Height :
                SettingsData.resolutionVertical;

            Game.Audio.AudioEngine.MasterVolume = SettingsData.volume;
        }

        public void Save()
        {
            try
            {
                var txt = JsonConvert.SerializeObject(SettingsData, Formatting.Indented);
                File.WriteAllText(FILE_PATH, txt);
                qDebug.Log("Saved game settings", "darkyellow");
            }
            catch (Exception e)
            {
                qDebug.LogError($"There was an error while saving game settings: {e}");
            }
        }

        public struct GameSettingsData
        {
            public int resolutionHorizontal;
            public int resolutionVertical;
            public bool fullscreen;
            public float mouseSensitivity;
            public float fov;
            public float volume;

            public static GameSettingsData Default =>
                new GameSettingsData()
                {
                    resolutionHorizontal = 0,
                    resolutionVertical = 0,
                    fullscreen = true,
                    mouseSensitivity = 1f,
                    fov = 50f,
                    volume = 1f,
                };

            public static bool operator ==(GameSettingsData a, GameSettingsData b) =>
                a.Equals(b);

            public static bool operator !=(GameSettingsData a, GameSettingsData b) =>
                !(a == b);

            public override bool Equals(object obj)
            {
                if (obj is not GameSettingsData data)
                    return false;

                return resolutionVertical == data.resolutionVertical &&
                    resolutionHorizontal == data.resolutionHorizontal &&
                    fullscreen == data.fullscreen &&
                    mouseSensitivity == data.mouseSensitivity &&
                    fov == data.fov &&
                    volume == data.volume;
            }

            public override int GetHashCode() =>
                ToString().GetHashCode();
        }
    }
}
