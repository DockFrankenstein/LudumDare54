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

namespace LudumDare54
{
    public class FloatingText : SyncScript
    {
        public Player.PlayerLook look;

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            if (look == null) return;
            look.XRotation.WorldMatrix.DecomposeXYZ(out Vector3 rotation);
            Entity.Transform.RotationEulerXYZ = rotation;
        }
    }
}