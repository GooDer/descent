using DescentDirX.Characters;
using DescentDirX.Characters.Actions;
using DescentDirX.Items.Weapons.Common;

namespace DescentDirX.Gameplay
{
    class CombatResolver
    {
        public Character Attacker { get; private set; }
        public Character Defender { get; private set; }

        public AttackResult Attack { get; private set; }
        public DefenseResult Defense { get; private set; }

        public CombatResolver(Character attacker, Character defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public void DoAttack()
        {
            Attack = ((IAbleToAttack)Attacker).RollAttack();
            Defense = Defender.RollDefense();

            if (!Attack.AttackMissed() && Attack.GetRange() >= Attacker.DistanceFrom(Defender))
            {
                var dmg = Attack.GetDamage();
                var def = Defense.GetDefense();
                int result = dmg - def;

                if (result < 0)
                {
                    result = 0;
                }

                Defender.TakeDamage(result);
            }

            Attacker.ActualAction = new MoveAction();
        }
    }
}
