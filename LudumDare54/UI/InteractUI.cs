using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myra;
using Myra.Graphics2D.UI;

namespace LudumDare54.UI
{
    public class InteractUI : UICanvas
    {
        public override void InitializeUI()
        {
            var grid = new Grid();

            grid.RowsProportions.Add(new Proportion(ProportionType.Part, 15f));
            grid.RowsProportions.Add(new Proportion(ProportionType.Part, 1f));
            grid.RowsProportions.Add(new Proportion(ProportionType.Part, 4f));

            var label = new Label()
            {
                Text = "Press [E] to interact",
                Font = MyraRenderer.Ft_SpartanMedium.GetFont(36),
                HorizontalAlignment = HorizontalAlignment.Center,
                GridRow = 1,
            };

            grid.Widgets.Add(label);
            CanvasDesktop.Root = grid;
        }
    }
}
