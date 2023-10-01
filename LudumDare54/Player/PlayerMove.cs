using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Physics;
using Stride.Engine;
using qASIC;
using Stride.Core;

namespace LudumDare54.Player
{
    public class PlayerMove : SyncScript
    {
        public CharacterComponent character;
        public TransformComponent directionTransform;

        public float speed = 6f;

        [DataMember] float jumpHeight = 5f;

        [DataMemberIgnore] public Atmosphere DefaultAtmosphere { get; private set; }
        private Atmosphere _currentAtmosphere;
        [DataMemberIgnore] public Atmosphere CurrentAtmosphere
        {
            get => _currentAtmosphere;
            set
            {
                qDebug.Log("Changed atmosphere", "green");
                _currentAtmosphere = value;
                character.Gravity = value.gravity;
            }
        }

        public override void Start()
        {
            CurrentAtmosphere = DefaultAtmosphere = new Atmosphere()
            {
                gravity = character.Gravity,
                jumpHeight = jumpHeight,
            };
        }

        public override void Update()
        {
            directionTransform.UpdateWorldMatrix();

            var input = GetPathInput();

            Vector3 direction = directionTransform.WorldMatrix.Right * input.X +
                directionTransform.WorldMatrix.Forward * input.Y;

            character.SetVelocity(direction * speed);

            if (Input.HasKeyboard && Input.IsKeyPressed(Keys.Space))
                character.Jump(Vector3.UnitY * CurrentAtmosphere.jumpHeight);
        }

        Vector2 GetPathInput()
        {
            if (!Input.HasKeyboard)
                return Vector2.Zero;

            var path = new Vector2();

            if (Input.IsKeyDown(Keys.W))
                path.Y += 1f;

            if (Input.IsKeyDown(Keys.S))
                path.Y -= 1f;

            if (Input.IsKeyDown(Keys.D))
                path.X += 1f;

            if (Input.IsKeyDown(Keys.A))
                path.X -= 1f;

            return path;
        }

        public void Teleport(Vector3 targetPosition)
        {
            character.Jump(Vector3.Zero);
            character.Teleport(targetPosition);
            qDebug.Log($"Teleported player to {targetPosition}");
        }

        [DataContract]
        public struct Atmosphere
        {
            public Vector3 gravity;
            public float jumpHeight;
        }
    }
}
