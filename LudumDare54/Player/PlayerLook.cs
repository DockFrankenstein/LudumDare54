using System;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace LudumDare54.Player
{
    public class PlayerLook : SyncScript
    {
        Vector2 _rotation;

        public TransformComponent XRotation;
        public TransformComponent YRotation;

        bool _mouseLocked = false;

        public override void Start()
        {
            CursorManager.GlobalState = false;
        }

        public override void Cancel()
        {
            CursorManager.GlobalState = true;
        }

        public override void Update()
        {
            if (Input.HasKeyboard && Input.IsKeyPressed(Keys.Tab))
                ToggleMouse();

            _rotation -= Input.MouseDelta;

            _rotation.Y = Math.Clamp(_rotation.Y, -1.5f, 1.5f);

            YRotation.RotationEulerXYZ = new Vector3(0f, _rotation.X, 0f);
            XRotation.RotationEulerXYZ = new Vector3(_rotation.Y, 0f, 0f);
        }

        void ToggleMouse()
        {
            _mouseLocked = !_mouseLocked;

            switch (_mouseLocked)
            {
                case true:
                    Input.Mouse.LockPosition();
                    Game.IsMouseVisible = false;
                    break;
                case false:
                    Input.Mouse.UnlockPosition();
                    Game.IsMouseVisible = true;
                    break;
            }
        }
    }
}
