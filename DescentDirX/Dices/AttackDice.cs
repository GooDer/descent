using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    abstract class AttackDice
    {
        public int Damage { get; protected set; }
        public int Surges { get; protected set; }
        public int Range { get; protected set; }
        protected ImageListEnum ResultImage { get; set; }

        public abstract void RollAttack();

        public override string ToString()
        {
            return "Damage: " + Damage + ", Surges: " + Surges + ", Range: " + Range;
        }

        public ImageListEnum GetResultImage()
        {
            return ResultImage;
        }

        public abstract ImageListEnum GetIcon();
    }
}
