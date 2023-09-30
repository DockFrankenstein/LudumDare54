using Stride.Rendering;
using Stride.Graphics;
using Stride.Rendering.Compositing;
using RenderContext = Stride.Rendering.RenderContext;
using FontStashSharp;
using System.IO;

namespace LudumDare54.UI
{
    public class MyraRenderer : SceneRendererBase
    {
        public static FontSystem Ft_SpartanBlack;
        public static FontSystem Ft_SpartanBold;
        public static FontSystem Ft_SpartanExtraBold;
        public static FontSystem Ft_SpartanExtraLight;
        public static FontSystem Ft_SpartanLight;
        public static FontSystem Ft_SpartanMedium;
        public static FontSystem Ft_SpartanRegular;
        public static FontSystem Ft_SpartanSemiBold;
        public static FontSystem Ft_SpartanThin;

        public static void InitializeFonts()
        {
            Ft_SpartanBlack = new FontSystem();
            Ft_SpartanBlack.AddFont(File.ReadAllBytes("Resources/Spartan-Black.ttf"));

            Ft_SpartanBold = new FontSystem();
            Ft_SpartanBold.AddFont(File.ReadAllBytes("Resources/Spartan-Bold.ttf"));

            Ft_SpartanExtraBold = new FontSystem();
            Ft_SpartanExtraBold.AddFont(File.ReadAllBytes("Resources/Spartan-ExtraBold.ttf"));

            Ft_SpartanExtraLight = new FontSystem();
            Ft_SpartanExtraLight.AddFont(File.ReadAllBytes("Resources/Spartan-ExtraLight.ttf"));

            Ft_SpartanLight = new FontSystem();
            Ft_SpartanLight.AddFont(File.ReadAllBytes("Resources/Spartan-Light.ttf"));

            Ft_SpartanMedium = new FontSystem();
            Ft_SpartanMedium.AddFont(File.ReadAllBytes("Resources/Spartan-Medium.ttf"));

            Ft_SpartanRegular = new FontSystem();
            Ft_SpartanRegular.AddFont(File.ReadAllBytes("Resources/Spartan-Regular.ttf"));

            Ft_SpartanSemiBold = new FontSystem();
            Ft_SpartanSemiBold.AddFont(File.ReadAllBytes("Resources/Spartan-SemiBold.ttf"));

            Ft_SpartanThin = new FontSystem();
            Ft_SpartanThin.AddFont(File.ReadAllBytes("Resources/Spartan-Thin.ttf"));
        }

        protected override void InitializeCore()
        {
            base.InitializeCore();
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            drawContext.CommandList.Clear(GraphicsDevice.Presenter.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer);

            var canvases = new System.Collections.Generic.List<UICanvas>(UICanvas.ActiveCanvases);
            foreach (var item in canvases)
            {
                item.OnDrawUI();
                item.CanvasDesktop.Render();
            }
        }
    }
}