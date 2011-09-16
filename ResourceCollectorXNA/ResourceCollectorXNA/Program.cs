using System;
using Microsoft.Xna.Framework;
namespace ResourceCollectorXNA
{
#if WINDOWS || XBOX
    static class Program
    {[STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MyGame game = new MyGame())
            {
                game.Run();
            }
        }
    }
#endif
}

