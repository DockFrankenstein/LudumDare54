using Stride.Engine;
using qASIC;
using Stride.Core.Diagnostics;
using Myra;

namespace LudumDare54
{
    public class CustomGame : Game
    {
        public qInstance QasicInstance { get; private set; }

        protected override void Initialize()
        {
            System.AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            GlobalLogger.GlobalMessageLogged += GlobalLogger_GlobalMessageLogged;
            
            QasicInstance = new qInstance();

            MyraEnvironment.Game = this;
            UI.MyraRenderer.InitializeFonts();

            CursorManager.Game = this;

            base.Initialize();

            QasicInstance.Start();
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
