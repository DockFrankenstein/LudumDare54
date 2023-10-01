using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using LudumDare54.Player;
using Stride.Core;

namespace LudumDare54
{
    public class ButtonInteractable : Interactable
    {
        public Entity EntityToToggle;

        public SpriteComponent SpriteComponent;
        public ISpriteProvider EnabledSprite;

        int _presses = 0;

        public override void Interact(PlayerInteract player)
        {
            SpriteComponent.SpriteProvider = EnabledSprite;
            if (_presses == 0)
                EntityToToggle.EnableAll(true, false);

            _presses++;
        }
    }
}
