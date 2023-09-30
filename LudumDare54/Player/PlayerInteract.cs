using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;

namespace LudumDare54.Player
{
    public class PlayerInteract : SyncScript
    {
        public float length = 8f;
        public TransformComponent center;
        public PhysicsComponent physicsComponent;
        public CollisionFilterGroupFlags layer;

        public override void Start()
        {
            
        }

        public override void Update()
        {
            center.UpdateWorldMatrix();
            var hit = Cast(center, physicsComponent.Simulation);

            if (Input.HasKeyboard && Input.IsKeyPressed(Keys.E))
                qASIC.qDebug.Log($"Interaction key pressed, hit succeeded: {hit.Succeeded}", "green");
        }

        public HitResult Cast(TransformComponent center, Simulation simulation) =>
            simulation.Raycast(center.WorldMatrix.TranslationVector, center.WorldMatrix.TranslationVector + center.WorldMatrix.Forward * length, filterFlags: layer);
    }
}
