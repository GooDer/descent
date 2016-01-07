using System;

namespace DescentDirX.Dices
{
    static class Dice
    {
        private static readonly Random rnd = new Random();

        public static int RollSixSideDice()
        {
            return rnd.Next(1, 7);
        }

        public static int RollTenSideDice()
        {
            return rnd.Next(1, 11);
        }
    }
}
