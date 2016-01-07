using DescentDirX.Dices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DescentDirX.Gameplay
{
    class AttackResult
    {
        public List<AttackDice> AttackDices { get; private set; }

        public int BonusDamage { get; set; }
        public int BonusRange { get; set; }
        public int PierceBonus { get; set; }

        public AttackResult()
        {
            AttackDices = new List<AttackDice>();
        }

        public int GetSurges()
        {
            var result = 0;
            foreach (var dice in AttackDices)
            {
                result += dice.Surges;
            }
            return result;
        }

        public int GetDamage()
        {
            var result = 0;
            foreach (var dice in AttackDices)
            {
                result += dice.Damage;
            }
            return result + BonusDamage;
        }

        public int GetRange()
        {
            var result = 0;
            foreach (var dice in AttackDices)
            {
                result += dice.Range;
            }
            return result + BonusRange;
        }

        public bool AttackMissed()
        {
            foreach (var dice in AttackDices)
            {
                if (dice is IHitDice)
                {
                    return (dice as IHitDice).Missed();
                }
            }

            return true;
        }

        public void RerollDice(int index)
        {
            if (AttackDices[index] != null)
            {
                AttackDices[index].RollAttack();
            }
        }

        public override string ToString()
        {
            var result = "";
            foreach (var dice in AttackDices)
            {
                if (dice != AttackDices.First())
                {
                    result += Environment.NewLine;
                }
                result += "Rolled on " + dice;
            }

            return result;
        }
    }
}
