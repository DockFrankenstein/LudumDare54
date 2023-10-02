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
        public CameraComponent Camera;

        [Stride.Core.DataMemberIgnore] public Vector2 Rotation => _rotation;

        GameSettings settings;

        public override void Start()
        {
            settings = ((CustomGame)Game).GameSettings;

            CursorManager.GlobalState = false;
        }

        public override void Cancel()
        {
            CursorManager.GlobalState = true;
        }

        public override void Update()
        {
            if (!Game.Window.Focused || CursorManager.IsMouseVisible)
                return;

            Camera.VerticalFieldOfView = settings.SettingsData.fov;

            _rotation -= Input.MouseDelta * Game.Window.ClientBounds.Height / 1080f * settings.SettingsData.mouseSensitivity;

            _rotation.Y = Math.Clamp(_rotation.Y, -(float)Math.PI / 2f, (float)Math.PI / 2f);

            YRotation.RotationEulerXYZ = new Vector3(0f, _rotation.X, 0f);
            XRotation.RotationEulerXYZ = new Vector3(_rotation.Y, 0f, 0f);
        }
    }
}
