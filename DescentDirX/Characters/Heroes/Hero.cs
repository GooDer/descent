using DescentDirX.Abilities;
using DescentDirX.Classes;
using DescentDirX.Gameplay;
using DescentDirX.Helpers;
using DescentDirX.Items;
using DescentDirX.Items.Weapons.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DescentDirX.Characters.Actions;

namespace DescentDirX.Characters.Heroes
{
    abstract class Hero : Character, IHeroClass, IAbleToAttack
    {
        public int DefaultMaxFatigue { get; private set; }
        public int ActualMaxFatigue { get; private set; }
        public int TakenFatigue { get; private set; } = 0;

        public int Might { get; protected set; }
        public int Knowledge { get; protected set; }
        public int Willpower { get; protected set; }
        public int Awareness { get; protected set; }

        public bool UsedRest { get; private set; }

        public IHeroClass HeroClass { get; private set; }

        public Collection<Item> Items { get; private set; }

        public Collection<IWeapon> EquipedWeapons { get; private set; }

        public Hero(string name, int maxLives, int maxSpeed, int stamina) : base(name, maxLives, maxSpeed)
        {
            DefaultMaxFatigue = stamina;
            ActualMaxFatigue = stamina;
            Items = new Collection<Item>();
            EquipedWeapons = new Collection<IWeapon>();
        }

        public virtual void SetHeroClass<T>(T heroClass) where T : IHeroClass
        {
            HeroClass = heroClass;
            Init();
        }

        protected override void Init()
        {
            foreach (var item in HeroClass.getClassEquipment())
            {
                Items.Add(item);
                if (item is IWeapon)
                {
                    EquipedWeapons.Add(item as IWeapon);
                }
            }
        }

        public IWeapon GetEquipedWeapon()
        {
            if (EquipedWeapons.Count == 0)
            {
                var unarmed = new Unarmed();
                return unarmed;
            }

            return EquipedWeapons[0];
        }

        public AttackResult RollAttack()
        {
            return GetEquipedWeapon().RollAttack();
        }

        public bool CanAttackOn(Character character)
        {
            var weapon = GetEquipedWeapon();
            return DistanceFrom(character) <= weapon.GetReach();
        }

        public bool UseFatigue(int amount)
        {
            if (!CanSpendFatigue(amount))
            {
                return false;
            }

            TakenFatigue += amount;
            return true;
        }

        public void RecoverFatigue(int amount)
        {
            TakenFatigue -= amount;
            if (TakenFatigue < 0)
            {
                TakenFatigue = 0;
            }
        }

        public bool CanSpendFatigue(int amount)
        {
            if (amount > (ActualMaxFatigue - TakenFatigue))
            {
                return false;
            }
            return true;
        }

        public IList<SurgeAbility> GetSurgeAbilities()
        {
            var result = new Collection<SurgeAbility>();
            foreach (var item in EquipedWeapons)
            {
                var usableItem = item as IHasSurgeAbility;

                if (usableItem != null)
                {
                    foreach (var ability in usableItem.GetSurgeAbilities())
                    {
                        result.Add(ability);
                    }
                }
            }

            return result;
        }

        public bool Rest()
        {
            if (UseAction())
            {
                UsedRest = true;
                return true;
            }

            return false;
        }

        public override void Upkeep()
        {
            base.Upkeep();
            if (UsedRest)
            {
                RecoverFatigue(ActualMaxFatigue);
            }
        }

        public bool HasFullFatigue()
        {
            return ActualMaxFatigue == TakenFatigue;
        }

        public bool UseFatigueToMove(int amount)
        {
            if (!CanSpendFatigue(amount))
            {
                return false;
            }

            TakenFatigue += amount;
            RemainingMovement += amount;

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} {1}\nfatigue: {2}/{3}\nmight: {4}\nknowledge: {5}\nwillpower: {6}\nawareness: {7}".Replace("\n", Environment.NewLine)
                , HeroClass, base.ToString(), TakenFatigue, ActualMaxFatigue, Might, Knowledge, Willpower, Awareness);
        }

        public Collection<Item> getClassEquipment()
        {
            return HeroClass.getClassEquipment();
        }

        public List<IHeroClass> GetPossibleClasses()
        {
            List<IHeroClass> classes = new List<IHeroClass>();

            if (this is IWarrior)
            {
                classes.Add(new Knight());
            } else if (this is IMage)
            {
                classes.Add(new Runemaster());
            }

            return classes;
        }

        public override List<IAction> GetPossibleActions()
        {
            List<IAction> actions = new List<IAction>();
            List<IAction> availableAction = new List<IAction>() { new AttackAction(), new MoveAction(), new MoveByFatigueAction(), new RestAction(), new EndCharacterTurnAction() };

            foreach (var action in availableAction)
            {
                if (action.CanPerformAction(this, GameplayProgress.Instance.Scenario))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }

        public override SideEnum GetSide()
        {
            return SideEnum.HEROS;
        }

        //////////// STATUS TEXT METHODS

        public string FatigueState()
        {
            return TakenFatigue + " / " + ActualMaxFatigue;
        }


        //////////// ABSTRACT INTERFACE

        public abstract ImageListEnum GetHeroSheet();
        public abstract ImageListEnum GetImage();
    }
}
