using DescentDirX.Dices;
using DescentDirX.Gameplay;
using System.Collections.ObjectModel;

namespace DescentDirX.Items.Weapons.Common
{
    abstract class Weapon<T> : Item, IWeapon
    {
        public const int MAX_REACH = 99;

        public byte Hand { get; private set; }
        public WeaponTypeEnum[] Type { get; private set; }

        private Collection<AttackDice> attackDices = new Collection<AttackDice>();

        public Weapon(string name, int buyPrice, int sellPrice, byte hand, WeaponTypeEnum[] type) : base (name, buyPrice, sellPrice)
        {
            Hand = hand;
            Type = type;
        }

        public void AddAttackDice(AttackDice dice)
        {
            attackDices.Add(dice);
        }

        public void AddAttackDices(AttackDice[] dices)
        {
            foreach (var dice in dices)
            {
                attackDices.Add(dice);
            }
        }

        public AttackResult RollAttack()
        {
            var result = new AttackResult();
            foreach (var dice in attackDices)
            {
                dice.RollAttack();
                result.AttackDices.Add(dice);
            }

            return result;
        }

        public abstract int GetReach();
    }
}
