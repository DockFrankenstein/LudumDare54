using LudumDare54.Player;
using Stride.Engine;

namespace LudumDare54
{
    public abstract class Interactable : SyncScript, IInteractable
    {
        public override void Update() { }

        public abstract void Interact(PlayerInteract player);
    }
}
