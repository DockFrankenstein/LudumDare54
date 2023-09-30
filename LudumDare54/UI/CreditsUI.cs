using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Core.Serialization;

using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;

namespace LudumDare54.UI
{
    public class CreditsUI : UICanvas
    {
        public Window window;

        public override void OnChangeState(bool isActive)
        {
            switch (isActive)
            {
                case true:
                    window.ShowModal(CanvasDesktop);
                    break;
                case false:
                    window.Close();
                    break;
            }
        }

        public override void InitializeUI()
        {
            var grid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Top,
            };

            var scroll = new ScrollViewer()
            {
                ShowHorizontalScrollBar = false,
                Content = grid,
            };

            window = new Window()
            {
                TitleFont = MyraRenderer.Ft_SpartanRegular.GetFont(20),
                Content = scroll,
                Width = 500,
                Height = 600,
                Title = "Credits",
            };

            for (int i = 0; i < 4; i++)
                grid.RowsProportions.Add(new Proportion());

            grid.Widgets.Add(CreateLabel("Game created by Dock Frankenstein for Ludum Dare 54", 0));
            grid.Widgets.Add(CreateLabel("Made with Stride 3D", 1));
            grid.Widgets.Add(CreateLabel("UI library: Myra", 2));
            grid.Widgets.Add(CreateLabel("Textures from cgbookcase.com", 3));
        }

        private Label CreateLabel(string text, int gridRow) =>
            new Label()
            {
                Text = text,
                GridRow = gridRow,
                Font = MyraRenderer.Ft_SpartanRegular.GetFont(20),
                Wrap = true,
                Padding = new Myra.Graphics2D.Thickness(4, 4),
            };
    }
}
