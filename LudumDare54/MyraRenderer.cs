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
        public static FontSystem Ft_SpartanRegular;

        public static void InitializeFonts()
        {
            Ft_SpartanRegular = new FontSystem();
            Ft_SpartanRegular.AddFont(File.ReadAllBytes("Resources/Spartan-Regular.ttf"));
        }

        protected override void InitializeCore()
        {
            base.InitializeCore();
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            drawContext.CommandList.Clear(GraphicsDevice.Presenter.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer);

            foreach (var item in UICanvas.ActiveCanvases)
            {
                item.OnDrawUI();
                item.CanvasDesktop.Render();
            }
        }
    }
}