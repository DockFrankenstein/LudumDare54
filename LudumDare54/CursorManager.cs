using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare54
{
    public static class CursorManager
    {
        public static Game Game { get; set; }

        private static bool _globalState = true;
        public static bool GlobalState
        {
            get => _globalState; 
            set
            {
                _globalState = value;
                ChangeState("global", value);
            }
        }

        public static bool IsMouseVisible => States.Count > 0;

        private static List<string> States { get; set; } = new List<string>()
        {
            "global",
        };

        public static void ChangeState(string stateName, bool isVisible)
        {
            stateName = stateName.ToLower();

            switch (isVisible)
            {
                case true:
                    if (!States.Contains(stateName))
                        States.Add(stateName);

                    break;
                case false:
                    if (States.Contains(stateName))
                        States.Remove(stateName);

                    break;
            }

            UpdateCursor();
        }

        public static void UpdateCursor()
        {
            bool visible = States.Count > 0;
            if (Game.Input.HasMouse)
            {
                switch (visible)
                {
                    case true:
                        Game.Input.Mouse.UnlockPosition();
                        Game.IsMouseVisible = true;
                        break;
                    case false:
                        Game.Input.Mouse.LockPosition();
                        Game.IsMouseVisible = false;
                        break;
                }
            }

            qASIC.qDebug.Log($"Updated cursor state, states: {string.Join(", ", States)}, visible: {visible}", "cyan");
        }
    }
}