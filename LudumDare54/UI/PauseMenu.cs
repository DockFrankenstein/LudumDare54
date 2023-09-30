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
    public class PauseMenu : UICanvas
    {
        public const float FADE_OPACITY = 0.5f;
        public const string CURSOR_STATE_NAME = "pause";

        GameSettings.GameSettingsData settingsData = new GameSettings.GameSettingsData();
        GameSettings settings;

        TextButton applySettingsButton;
        TextButton discardSettingsButton;

        Window settingsWindow;

        public override void Start()
        {
            base.Start();
            settings = ((CustomGame)Game).GameSettings;
        }

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
                Text = "Paused",
                Font = MyraRenderer.Ft_SpartanSemiBold.GetFont(100),
                TextColor = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Myra.Graphics2D.Thickness(0, 10),
            };

            settingsWindow = new Window()
            {
                Title = "Settings",
                Content = CreateSettingsContent(),
                MinWidth = 300,
                MinHeight = 300,
                Width = 600,
                Height = 700,
                TitleFont = MyraRenderer.Ft_SpartanRegular.GetFont(20),
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
            buttonGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));

            var continueButton = CreateButton("Continue");
            continueButton.Click += (_, _) => Active = !Active;
            
            var settingsButton = CreateButton("Settings");
            settingsButton.GridRow = 1;
            settingsButton.Click += (_, _) => settingsWindow.ShowModal(CanvasDesktop, 
                new Point(Game.Window.ClientBounds.Width * 3 / 4 - (settingsWindow.Width ?? 0) / 2, 
                Game.Window.ClientBounds.Height / 2 - (settingsWindow.Height ?? 0) / 2));

            var restartButton = CreateButton("Restart");
            restartButton.GridRow = 2;
            restartButton.Click += (_, _) => SceneManager.Instance.ReloadScene();

            var exitButton = CreateButton("Exit");
            exitButton.GridRow = 3;
            exitButton.Click += (_, _) => ((CustomGame)Game).Exit();

            buttonGrid.Widgets.Add(continueButton);
            buttonGrid.Widgets.Add(settingsButton);
            buttonGrid.Widgets.Add(restartButton);
            buttonGrid.Widgets.Add(exitButton);

            mainGrid.Widgets.Add(title);
            mainGrid.Widgets.Add(buttonGrid);

            panel.Widgets.Add(mainGrid);
            CanvasDesktop.Root = panel;
        }

        public override void OnDrawUI()
        {
            bool settingsModified = settingsData != settings.SettingsData;
            applySettingsButton.Enabled = settingsModified;
            discardSettingsButton.Enabled = settingsModified;

            base.OnDrawUI();
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

        private Widget CreateSettingsContent()
        {
            var outsideGrid = new Grid();

            outsideGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));
            outsideGrid.RowsProportions.Add(new Proportion());
            outsideGrid.RowsProportions.Add(new Proportion());


            var scroll = new ScrollViewer()
            {
                GridRow = 0,
                Content = CreateSettings(),
                ShowHorizontalScrollBar = false,
            };

            outsideGrid.Widgets.Add(scroll);


            var buttonGrid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                GridRow = 2,
                ColumnSpacing = 8,
            };

            applySettingsButton = CreateButton("Apply", 0);
            applySettingsButton.Click += (_, _) =>
            {
                settings.SettingsData = settingsData;
                ((CustomGame)Game).GameSettings.Save();
                ((CustomGame)Game).GameSettings.LoadSettings();

                settingsWindow.Close();
            };

            discardSettingsButton = CreateButton("Discard", 1);
            discardSettingsButton.Click += (_, _) => settingsData = settings.SettingsData;
            var closeButton = CreateButton("Close", 2);
            closeButton.Click += (_, _) =>
            {
                settingsData = settings.SettingsData;
                settingsWindow.Close();
            };

            buttonGrid.Widgets.Add(applySettingsButton);
            buttonGrid.Widgets.Add(discardSettingsButton);
            buttonGrid.Widgets.Add(closeButton);

            outsideGrid.Widgets.Add(new HorizontalSeparator() 
            { 
                GridRow = 1, 
                Margin = new Myra.Graphics2D.Thickness(0, 4) 
            });

            outsideGrid.Widgets.Add(buttonGrid);

            return outsideGrid;

            TextButton CreateButton(string text, int gridColumn = 0) =>
                new TextButton()
                {
                    Text = text,
                    GridColumn = gridColumn,
                    Font = MyraRenderer.Ft_SpartanRegular.GetFont(20),
                    Padding = new Myra.Graphics2D.Thickness(8, 8),
                    Width = 100,
                };
        }

        private Widget CreateSettings()
        {
            var grid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Top,
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 4f));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 10f));

            grid.Widgets.Add(CreateSettingsLabel("Fullscreen", 0));
            grid.Widgets.Add(CreateSettingsLabel("UI Debug", 1));

            var fullscreenToggle = new ComboBox()
            {
                GridColumn = 1,
                GridRow = 0,
            };

            fullscreenToggle.Items.Add(CreateListItem("True", () => settingsData.fullscreen = true));
            fullscreenToggle.Items.Add(CreateListItem("False", () => settingsData.fullscreen = false));

            fullscreenToggle.SelectedIndex = settingsData.fullscreen ? 0 : 1;

            grid.Widgets.Add(fullscreenToggle);

            var debugWindow = new DebugOptionsWindow();

            var uiDebugButton = new TextButton()
            {
                Text = "Open",
                Font = MyraRenderer.Ft_SpartanRegular.GetFont(20),
                GridColumn = 1,
                GridRow = 1,
                Padding = new Myra.Graphics2D.Thickness(8, 8),
            };

            uiDebugButton.Click += (_, _) => debugWindow.ShowModal(CanvasDesktop);

            grid.Widgets.Add(uiDebugButton);

            return grid;

            ListItem CreateListItem(string text, Action onChange)
            {
                var item = new ListItem()
                {
                    Text = text,
                };

                item.SelectedChanged += (_, _) =>
                {
                    if (item.IsSelected)
                        onChange?.Invoke();
                };

                return item;
            }

            Label CreateSettingsLabel(string text, int gridRow) =>
                new Label()
                {
                    Text = text,
                    GridRow = gridRow,
                    GridColumn = 0,
                    Font = MyraRenderer.Ft_SpartanRegular.GetFont(20),
                };
        }
    }
}
