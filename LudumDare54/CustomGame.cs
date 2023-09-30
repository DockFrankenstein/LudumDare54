using Stride.Engine;
using qASIC;
using Stride.Core.Diagnostics;
using Myra;
using Myra.Graphics2D.UI.Styles;
using LudumDare54.UI;

namespace LudumDare54
{
    public class CustomGame : Game
    {
        public qInstance QasicInstance { get; private set; }

        public GameSettings GameSettings { get; private set; } = new GameSettings();

        protected override void Initialize()
        {
            System.AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            GlobalLogger.GlobalMessageLogged += GlobalLogger_GlobalMessageLogged;
            
            QasicInstance = new qInstance();

            MyraEnvironment.Game = this;
            MyraRenderer.InitializeFonts();

            CursorManager.Game = this;

            base.Initialize();

            GameSettings = new GameSettings()
            {
                Game = this,
            };

            QasicInstance.Start();
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            GameSettings.Load();
        }

        private void GlobalLogger_GlobalMessageLogged(ILogMessage obj)
        {
            var color = obj.Type switch
            {
                LogMessageType.Debug => "green",
                LogMessageType.Info => "blue",
                LogMessageType.Warning => qDebug.WARNING_COLOR_TAG,
                LogMessageType.Verbose => qDebug.DEFAULT_COLOR_TAG,
                LogMessageType.Error => qDebug.ERROR_COLOR_TAG,
                LogMessageType.Fatal => "darkred",
                _ => qDebug.DEFAULT_COLOR_TAG,
            };

            qDebug.Log(obj.Text, color);
        }

        private void CurrentDomain_ProcessExit(object sender, System.EventArgs e)
        {
            QasicInstance.Stop();
        }
    }
}
