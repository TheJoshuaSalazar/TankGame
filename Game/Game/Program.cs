using System;

namespace Game
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (TankGame game = new TankGame())
            {
                game.Run();
            }
        }
    }
#endif
}

