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
using System.Windows;
using static BulletSharp.Dbvt;
using Stride.Core.Collections;
using System.Collections.Specialized;

namespace LudumDare54.Player
{
    public class PlayerAtmosphere : AsyncScript
    {
        public PlayerMove.Atmosphere atmosphere;
        public Vector2 xRotationActivationRange = new Vector2(-2f, 2f);

        PlayerLook look = null;
        PlayerMove move = null;
        bool collided = false;
        bool active = false;

        StaticColliderComponent trigger;

        public bool Active
        {
            get => active;
            set
            {
                if (active == value)
                    return;

                active = value;

                switch (active)
                {
                    case true:
                        move.CurrentAtmosphere = atmosphere;
                        break;
                    case false:
                        move.CurrentAtmosphere = move.DefaultAtmosphere;
                        break;
                }
            }
        }

        public override async Task Execute()
        {
            trigger = Entity.Get<StaticColliderComponent>();
            trigger.Collisions.CollectionChanged += Collisions_CollectionChanged;

            while (Game.IsRunning)
            {
                await Script.NextFrame();

                Active = collided &&
                    xRotationActivationRange.X <= look.Rotation.Y &&
                    xRotationActivationRange.Y >= look.Rotation.Y;
            }
        }

        private void Collisions_CollectionChanged(object sender, TrackingCollectionChangedEventArgs args)
        {
            if (args.Action != NotifyCollectionChangedAction.Add &&
                args.Action != NotifyCollectionChangedAction.Remove)
                return;

            var collision = (Collision)args.Item;

            PhysicsComponent otherCollider = trigger == collision.ColliderA
                ? collision.ColliderB
                : collision.ColliderA;

            var newLook = otherCollider.Entity.Get<PlayerLook>();
            var newMove = otherCollider.Entity.Get<PlayerMove>();

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (collided)
                        return;

                    if (newLook == null || newMove == null)
                        return;

                    look = newLook;
                    move = newMove;
                    collided = true;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (!collided)
                        return;

                    if (newLook != look && newMove != move)
                        return;

                    collided = false;
                    Active = false;
                    move = null;
                    look = null;
                    break;
            }
        }

        async Task CheckCollision()
        {
            while (Game.IsRunning)
            {
                Collision firstCollision = await trigger.NewCollision();

                PhysicsComponent otherCollider = trigger == firstCollision.ColliderA
                    ? firstCollision.ColliderB
                    : firstCollision.ColliderA;

                look = otherCollider.Entity.Get<PlayerLook>();
                move = otherCollider.Entity.Get<PlayerMove>();

                if (look == null || move == null)
                {
                    look = null;
                    move = null;
                    continue;
                }

                collided = true;

                //Wait for the entity to exit the trigger.
                Collision collision;

                do
                {
                    collision = await trigger.CollisionEnded();
                }
                while (collision != firstCollision);
                collided = false;
            }
        }
    }
}
