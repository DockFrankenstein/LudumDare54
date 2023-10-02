using Stride.Core.Mathematics;
using Stride.Input;

using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;

namespace LudumDare54.UI
{
    public class PauseMenu : UICanvas 
    {
        public const float FADE_OPACITY = 0.5f;
        public const string CURSOR_STATE_NAME = "pause";

        public CreditsUI credits;
        public SettingsUI settings;

        public override void Update()
        {
            if (Input.HasKeyboard && Input.IsKeyPressed(Keys.Escape))
                Active = !Active;
        }

        public override void OnChangeState(bool isActive)
        {
            CursorManager.ChangeState(CURSOR_STATE_NAME, isActive);
        }

        public override void InitializeUI()
        {
            var panel = new Panel()
            {
                Background = new SolidBrush(new Color(0, 0, 0, 128)),
            };

            var mainGrid = new Grid()
            {
                
            };

            mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 4f));
            mainGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 6f));

            var title = new Label()
            {
                Text = "Menu",
                Font = MyraRenderer.Ft_SpartanSemiBold.GetFont(100),
                TextColor = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Myra.Graphics2D.Thickness(0, 10),
            };


            var buttonGrid = new Grid()
            {
                GridRow = 1,
                RowSpacing = 8,
            };

            buttonGrid.RowsProportions.Add(new Proportion());
            buttonGrid.RowsProportions.Add(new Proportion());
            buttonGrid.RowsProportions.Add(new Proportion());
            buttonGrid.RowsProportions.Add(new Proportion());
            buttonGrid.RowsProportions.Add(new Proportion());
            buttonGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));

            var continueButton = CreateButton("Continue");
            continueButton.Click += (_, _) => Active = !Active;
            
            var settingsButton = CreateButton("Settings");
            settingsButton.GridRow = 1;
            settingsButton.Click += (_, _) =>
            {
                settings.Active = !settings.Active;
            };

            var creditsButton = CreateButton("Credits");
            creditsButton.GridRow = 2;
            creditsButton.Click += (_, _) => credits.Active = !credits.Active;

            var restartButton = CreateButton("Restart");
            restartButton.GridRow = 3;
            restartButton.Click += (_, _) => SceneManager.Instance.ReloadScene();

            var exitButton = CreateButton("Exit");
            exitButton.GridRow = 4;
            exitButton.Click += (_, _) => ((CustomGame)Game).Exit();

            buttonGrid.Widgets.Add(continueButton);
            buttonGrid.Widgets.Add(settingsButton);
            buttonGrid.Widgets.Add(creditsButton);
            buttonGrid.Widgets.Add(restartButton);
            buttonGrid.Widgets.Add(exitButton);

            mainGrid.Widgets.Add(title);
            mainGrid.Widgets.Add(buttonGrid);

            panel.Widgets.Add(mainGrid);
            CanvasDesktop.Root = panel;
        }

        private static TextButton CreateButton(string text) =>
            new TextButton()
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Height = 36,
                Font = MyraRenderer.Ft_SpartanRegular.GetFont(36),
                Background = null,
                OverBackground = null,
                DisabledBackground = null,
                FocusedBackground = null,
                PressedBackground = null,
                TextColor = Color.White,
                OverTextColor = new Color(115, 222, 255),
                PressedTextColor = new Color(0, 195, 255),
            };
    }
}
