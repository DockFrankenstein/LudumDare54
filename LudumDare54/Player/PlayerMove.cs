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
using BulletSharp.SoftBody;

namespace LudumDare54.Player
{
    public class PlayerMove : SyncScript
    {
        public const float SFX_VOLUME = 0.5f;

        [DataMemberIgnore] public bool CanMove { get; set; } = true;

        public CharacterComponent character;
        public TransformComponent directionTransform;

        public float speed = 6f;
        public float jumpHeight = 5f;
        public float stepDistance = 0.3f;

        PlayerSounds sounds;

        static Random Random { get; } = new Random();

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
            sounds = Entity.Get<PlayerSounds>();

            CurrentAtmosphere = DefaultAtmosphere = new Atmosphere()
            {
                gravity = character.Gravity,
                jumpHeight = jumpHeight,
            };

            SetStep(false);
            _isGrounded = character.IsGrounded;
        }

        bool _isGrounded;
        Vector3 _lastStepPos;

        bool CharacterGrounded =>
            -0.1f < Entity.Transform.WorldMatrix.TranslationVector.Y &&
            Entity.Transform.WorldMatrix.TranslationVector.Y < 0.1f &&
            character.IsGrounded;

        public override void Update()
        {
            ProcessSounds();
            directionTransform.UpdateWorldMatrix();

            var input = GetPathInput();

            Vector3 direction = directionTransform.WorldMatrix.Right * input.X +
                directionTransform.WorldMatrix.Forward * input.Y;

            character.SetVelocity(direction * speed);

            if (CanMove && CharacterGrounded && Input.HasKeyboard && Input.IsKeyPressed(Keys.Space))
                character.Jump(Vector3.UnitY * CurrentAtmosphere.jumpHeight);
        }

        void ProcessSounds()
        {
            bool groundedPreviously = _isGrounded;
            _isGrounded = CharacterGrounded;

            var pos = Entity.Transform.WorldMatrix.TranslationVector;
            
            if (!_isGrounded)
                return;

            if (!groundedPreviously)
                SetStep();

            if (Vector3.Distance(_lastStepPos, pos) >= stepDistance)
                SetStep();
        }

        void SetStep(bool playSound = true)
        {
            _lastStepPos = Entity.Transform.WorldMatrix.TranslationVector;

            if (!playSound) return;

            if (sounds.footsteps.Count == 0)
                return;

            var index = Random.Next(0, sounds.footsteps.Count);

            var instance = sounds.footsteps[index].CreateInstance();
            instance.IsLooping = false;
            instance.Volume = SFX_VOLUME;
            instance.Play();
        }

        Vector2 GetPathInput()
        {
            if (!CanMove)
                return Vector2.Zero;

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
            character.SetVelocity(Vector3.Zero);
            character.Jump(Vector3.Zero);
            character.Teleport(targetPosition);
            SetStep(false);
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
