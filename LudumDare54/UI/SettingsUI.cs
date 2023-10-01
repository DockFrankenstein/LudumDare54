using System;
using Stride.Core.Mathematics;

using Myra.Graphics2D.UI;
using System.Runtime;

namespace LudumDare54.UI
{
    public class SettingsUI : UICanvas
    {
        Window window;

        GameSettings.GameSettingsData settingsData = new GameSettings.GameSettingsData();
        GameSettings settings;

        DebugOptionsWindow debugWindow;
        TextButton applySettingsButton;
        TextButton discardSettingsButton;
        ComboBox fullscreenToggle;

        public override void Start()
        {
            base.Start();
            settings = ((CustomGame)Game).GameSettings;
        }

        public override void OnDrawUI()
        {
            bool settingsModified = settingsData != settings.SettingsData;
            applySettingsButton.Enabled = settingsModified;
            discardSettingsButton.Enabled = settingsModified;

            base.OnDrawUI();
        }

        public override void OnChangeState(bool isActive)
        {
            switch (isActive)
            {
                case true:
                    settingsData = settings.SettingsData;

                    var windowPoint = new Point(Game.Window.ClientBounds.Width * 3 / 4 - (window.Width ?? 0) / 2,
                        Game.Window.ClientBounds.Height / 2 - (window.Height ?? 0) / 2);

                    ResetSettingsUI();

                    window.ShowModal(CanvasDesktop, windowPoint);
                    break;
                case false:
                    window.Close();
                    debugWindow.Close();
                    break;
            }
        }

        void ResetSettingsUI()
        {
            fullscreenToggle.SelectedIndex = settingsData.fullscreen ? 0 : 1;
        }

        public override void InitializeUI()
        {
            window = new Window()
            {
                Title = "Settings",
                Content = CreateSettingsContent(),
                MinWidth = 300,
                MinHeight = 300,
                Width = 600,
                Height = 700,
                TitleFont = MyraRenderer.Ft_SpartanRegular.GetFont(20),
            };

            window.Closed += (_, _) =>
            {
                Active = false;
            };
        }

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

                window.Close();
            };

            discardSettingsButton = CreateButton("Discard", 1);
            discardSettingsButton.Click += (_, _) =>
            {
                settingsData = settings.SettingsData;
                ResetSettingsUI();
            };

            var closeButton = CreateButton("Close", 2);
            closeButton.Click += (_, _) =>
            {
                settingsData = settings.SettingsData;
                window.Close();
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

            fullscreenToggle = new ComboBox()
            {
                GridColumn = 1,
                GridRow = 0,
            };

            fullscreenToggle.Items.Add(CreateListItem("True", () => settingsData.fullscreen = true));
            fullscreenToggle.Items.Add(CreateListItem("False", () => settingsData.fullscreen = false));

            grid.Widgets.Add(fullscreenToggle);

            debugWindow = new DebugOptionsWindow();

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
