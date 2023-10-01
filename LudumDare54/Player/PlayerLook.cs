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

        [Stride.Core.DataMemberIgnore] public Vector2 Rotation => _rotation;

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
            if (CursorManager.IsMouseVisible)
                return;

            _rotation -= Input.MouseDelta * Game.Window.ClientBounds.Height / 1000f;

            _rotation.Y = Math.Clamp(_rotation.Y, -(float)Math.PI / 2f, (float)Math.PI / 2f);

            YRotation.RotationEulerXYZ = new Vector3(0f, _rotation.X, 0f);
            XRotation.RotationEulerXYZ = new Vector3(_rotation.Y, 0f, 0f);
        }
    }
}
