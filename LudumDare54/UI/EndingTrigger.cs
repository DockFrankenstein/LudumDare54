using LudumDare54.UI;
using Stride.Core.Mathematics;

using Myra.Graphics2D.UI;
using System.Collections.Generic;
using Myra.Graphics2D.Brushes;
using System;

namespace LudumDare54
{
    public class EndingTrigger : UICanvas
    {
        public const double TEXT_FADE_DURATION = 2.0;
        public const float BACKGROUND_FADE_DURATION = 1f;

        public Door door;

        public Color color = Color.White;
        public List<string> messages { get; } = new List<string>();

        List<Label> _labels;
        Panel _panel;

        public override void Start()
        {
            if (door != null)
                door.OnPlayerEnter += Door_OnPlayerEnter;

            base.Start();
        }

        private void Door_OnPlayerEnter(Player.PlayerMove obj)
        {
            Active = true;
            _animate = true;
        }

        bool _animate = false;
        double time = 0;

        public override void Update()
        {
            if (!_animate) return;
            time += Game.UpdateTime.Elapsed.TotalSeconds;

            _panel.Opacity = Math.Clamp((float)(time / BACKGROUND_FADE_DURATION), 0f, 1f);

            for (int i = 0; i < _labels.Count; i++)
                _labels[i].TextColor = new Color(0, 0, 0, (byte)Math.Round(Math.Clamp((time - BACKGROUND_FADE_DURATION - i * TEXT_FADE_DURATION) * 255.0 / TEXT_FADE_DURATION, 0.0, 255.0)));
        }

        public override void InitializeUI()
        {
            _panel = new Panel()
            {
                Background = new SolidBrush(new Color(color.R, color.G, color.B, color.A)),
                Opacity = 0f,
            };

            var grid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Center,
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));

            for (int i = 0; i < messages.Count; i++)
                grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            _labels = new List<Label>();
            for (int i = 0; i < messages.Count; i++)
            {
                var item = messages[i];

                var text = new Label()
                {
                    Text = item,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Font = MyraRenderer.Ft_SpartanRegular.GetFont(24),
                    TextColor = new Color(0),
                    Height = 24,
                    GridRow = i,
                };

                _labels.Add(text);
                grid.Widgets.Add(text);
            }

            _panel.Widgets.Add(grid);
            CanvasDesktop.Root = _panel;
        }
    }
}