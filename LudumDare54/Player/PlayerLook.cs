﻿using System;
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
            _rotation -= Input.MouseDelta;

            _rotation.Y = Math.Clamp(_rotation.Y, -1.5f, 1.5f);

            YRotation.RotationEulerXYZ = new Vector3(0f, _rotation.X, 0f);
            XRotation.RotationEulerXYZ = new Vector3(_rotation.Y, 0f, 0f);
        }
    }
}
