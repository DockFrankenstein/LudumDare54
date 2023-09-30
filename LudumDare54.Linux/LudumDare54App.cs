using Stride.Engine;

namespace LudumDare54
{
    class LudumDare54App
    {
        static void Main(string[] args)
        {
            using (var game = new CustomGame())
            {
                game.Run();
            }
        }
    }
}
