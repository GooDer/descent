using DescentDirX.Dices;
using DescentDirX.Gameplay;
using DescentDirX.Items.Weapons.Common;
using DescentDirX.Maps;
using System.Collections.ObjectModel;
using DescentDirX.Characters.Actions;
using System.Collections.Generic;

namespace DescentDirX.Characters.Monsters
{
    abstract class Monster : Character, IWeapon
    {
        private Collection<AttackDice> attackDices = new Collection<AttackDice>();
        protected Collection<EnviromentEnum> envs = new Collection<EnviromentEnum>();

        public Monster(string name, int maxLives, int maxSpeed) 
            : base(name, maxLives, maxSpeed)
        {
            Init();
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

        public void AddEnviroment(EnviromentEnum env)
        {
            envs.Add(env);
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

        public Collection<EnviromentEnum> GetEnviroments()
        {
            return envs;
        }

        public override SideEnum GetSide()
        {
            return SideEnum.OVERLORD;
        }

        public override List<IAction> GetPossibleActions()
        {
            List<IAction> actions = new List<IAction>();
            return actions;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Dead)
            {
                Position.ObjectsOnField.Remove(this);
                Position = null;
            }
        }

        public abstract int GetReach();
    }
}
