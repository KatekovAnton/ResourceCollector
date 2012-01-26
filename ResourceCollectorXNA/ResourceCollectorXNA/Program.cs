using System;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;


namespace ResourceCollectorXNA
{
#if WINDOWS || XBOX
   public  static class Program
   {
       public static string help
       {
           get
           {
               return "Please insert HELP here";
           }
       }

        public static MyGame game;
    
       
       [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args)
                {
                    if (arg == "-git")
                    {
                        string git_commander = @"C:\Users\shpengler\Desktop\git\GitCommander\GitCommander\bin\Debug\GitCommander.exe";
                        Process.Start(git_commander, @"-w C:\Users\shpengler\Desktop\git\ResourceCollector");
                    }
                }
            }
            catch { }
            game = new MyGame();
            game.Run();
        }
    }
#endif
}

