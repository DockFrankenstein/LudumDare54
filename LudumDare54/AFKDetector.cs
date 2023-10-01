using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using LudumDare54.Player;
using LudumDare54.UI;
using qASIC;
using Stride.Physics;
using static BulletSharp.Dbvt;
using Stride.Core.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LudumDare54
{
    public class AFKDetector : AsyncScript
    {
        public const string URL = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

        Vector3 pos;
        Vector3 rot;

        double time;

        StaticColliderComponent trigger;

        PlayerLook look;

        public EndingTrigger ending;

        public PauseMenu pause;

        public override async Task Execute()
        {
            trigger = Entity.Get<StaticColliderComponent>();

            foreach (var item in trigger.Collisions)
            {
                var collider = trigger == item.ColliderA
                    ? item.ColliderB
                    : item.ColliderA;

                var look = collider.Entity.Get<PlayerLook>();

                if (look == null) continue;
                RegisterNewObject(look);
                break;
            }

            trigger.Collisions.CollectionChanged += Collisions_CollectionChanged;

            while (Game.IsRunning)
            {
                await Script.NextFrame();

                if (look == null)
                    continue;

                if (!pause.Active)
                    time += Game.UpdateTime.Elapsed.TotalSeconds;

                if (ending.Active)
                    continue;

                if (time > 60)
                {
                    ending.Active = true;
                    OpenURL(URL);
                    continue;
                }

                var newPos = look.XRotation.WorldMatrix.TranslationVector;
                look.XRotation.WorldMatrix.DecomposeXYZ(out var newRot);

                if (newPos != pos ||
                    newRot != rot)
                {
                    time = 0.0;
                    pos = newPos;
                    rot = newRot;
                }
            }
        }

        private void OpenURL(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    return;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                    return;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                    return;
                }
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

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (newLook == null)
                        return;

                    RegisterNewObject(look);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (newLook != look)
                        return;

                    look = null;
                    break;
            }
        }

        private void RegisterNewObject(PlayerLook newLook)
        {
            look = newLook;
            time = 0.0;
            pos = look.XRotation.WorldMatrix.TranslationVector;
            look.XRotation.WorldMatrix.DecomposeXYZ(out var newRot);
            rot = newRot;
        }
    }
}
