using DescentDirX.Helpers;
using System;

namespace DescentDirX.Dices
{
    abstract class DefenseDice : IDefenseDice
    {
        private int defense;
        protected ImageListEnum ResultImage { get; set; }

        public int Defense {
            get { return defense; }
            protected set {
                LastDefense = Defense;
                defense = value;
            }
        }

        protected int LastDefense { get; set; }

        public int GetDefense(bool getBetterResult = false)
        {
            if (getBetterResult)
            {
                return Math.Max(Defense, LastDefense);
            }
            return Defense;
        }

        public ImageListEnum GetResultImage()
        {
            return ResultImage;
        }

        public abstract void RollDefense();

        public abstract ImageListEnum GetIcon();
    }
}
