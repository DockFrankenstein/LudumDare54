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

namespace LudumDare54.Player
{
    public class PlayerMove : SyncScript
    {
        public CharacterComponent character;
        public TransformComponent directionTransform;

        public float speed = 6f;

        public override void Start()
        {
            
        }

        public override void Update()
        {
            directionTransform.UpdateWorldMatrix();

            var input = GetPathInput();

            Vector3 direction = directionTransform.WorldMatrix.Right * input.X +
                directionTransform.WorldMatrix.Forward * input.Y;

            character.SetVelocity(direction * speed);
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
            character.Teleport(targetPosition);
            qDebug.Log($"Teleported player to {targetPosition}");
        }
    }
}
