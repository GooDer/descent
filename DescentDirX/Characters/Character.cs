using DescentDirX.Characters.Actions;
using DescentDirX.Characters.Conditions;
using DescentDirX.Dices;
using DescentDirX.Gameplay;
using DescentDirX.Helpers;
using DescentDirX.Maps;
using DescentDirX.Maps.Tiles.Common;
using DescentDirX.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DescentDirX.Characters
{
    abstract class Character
    {
        /// <summary>
        /// Character name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Default starting maximum lives without any buffs
        /// </summary>
        public int DefaultMaxLives { get; private set; }
        /// <summary>
        /// Actual maximum lives with all buffs
        /// </summary>
        public int ActualMaxLives { get; private set; }
        /// <summary>
        /// Damage taken so far -> TakenDamage == ActualMaxLives == Dead
        /// </summary>
        public int TakenDamage { get; private set; }

        public int MaxSpeed { get; private set; }
        public int ActSpeed { get; private set; }

        private int remainingMovement;
        public int RemainingMovement {
            get {
                return remainingMovement;
            }
            protected set {
                remainingMovement = value;
                PathHelper helper = new PathHelper();
                reachableFields = helper.GetFieldsInRadiusForMove(Position, remainingMovement);
                
                foreach (var key in reachableFields.Keys)
                {
                    foreach (var field in reachableFields[key])
                    {
                        field.MainColor = Color.Gray;
                    }
                }

                foreach (var field in GetFieldsForMovement())
                {
                    field.MainColor = Color.LimeGreen;
                }
            }
        }

        private Dictionary<int, HashSet<Field>> reachableFields = new Dictionary<int, HashSet<Field>>();

        public int RemainingActions { get; private set; }

        public bool Poisoned { get; protected set; }
        public bool Diseased { get; protected set; }
        public bool Stunned { get; protected set; }
        public bool Immobilized { get; protected set; }

        public bool Dead { get; protected set; }

        public Field Position { get; set; }

        private Collection<string> skills;

        private Collection<IDefenseDice> defenseDices = new Collection<IDefenseDice>();

        private List<ICondition> conditions;

        public IAction ActualAction { get; set; }

        public Character(string name, int maxLives, int maxSpeed)
        {
            Name = name;
            DefaultMaxLives = maxLives;
            MaxSpeed = maxSpeed;
            ActualMaxLives = maxLives;
            ActSpeed = maxSpeed;
        }

        public void AddDefenseDice(IDefenseDice dice)
        {
            defenseDices.Add(dice);
        }

        public void AddDefenseDices(IDefenseDice[] dices)
        {
            foreach (var dice in dices)
            {
                defenseDices.Add(dice);
            }
        }

        public DefenseResult RollDefense()
        {
            var result = new DefenseResult();
            foreach (var dice in defenseDices)
            {
                dice.RollDefense();
                result.DefenseDices.Add(dice);
            }

            return result;
        }

        public Collection<IDefenseDice> GetDefenseDices()
        {
            return defenseDices;
        }

        public virtual void TakeDamage(int damage)
        {
            TakenDamage += damage;

            if (TakenDamage >= ActualMaxLives)
            {
                TakenDamage = ActualMaxLives;
                Dead = true;
            }
        }

        public bool UseMovementAction()
        {
            if (UseAction())
            {
                RemainingMovement += ActSpeed;
                ActualAction = new MoveAction();
                return true;
            }

            return false;
        }

        public bool UseAttackAction()
        {
            if (UseAction())
            {
                ActualAction = new AttackAction();
                return true;
            }
            else if (RemainingMovement > 0)
            {
                ActualAction = new MoveAction();
            }

            return false;
        }

        protected abstract void Init();

        public virtual void Upkeep()
        {
            RemainingActions = 2;
            RemainingMovement = 0;
        }

        protected bool UseAction()
        {
            if (RemainingActions > 0)
            {
                RemainingActions--;
                return true;
            }

            return false;
        }

        public int DistanceFrom(Character character)
        {
            return Position.DistanceFrom(character.Position);
        }

        public override string ToString()
        {
            return string.Format("{0}\nspeed: {1}\nremaining movement: {2}\ndamage taken: {3}/{4}\nremaining actions: {5}".Replace("\n", Environment.NewLine)
                , Name, ActSpeed, RemainingMovement, TakenDamage, ActualMaxLives, RemainingActions);
        }

        public virtual Collection<Field> GetPlaceableFields(Field field)
        {
            if (field == null)
            {
                return null;
            }
            return new Collection<Field>() { field };
        }

        public void Draw(SpriteBatch spriteBatch, Field field = null)
        {
            if (Position != null)
            {
                spriteBatch.Draw(ImageProvider.Instance.GetImage(GetMapToken()), GameCamera.Instance.GetMovedVector(Position.GetVector()));
            }
            else if (field != null)
            {
                spriteBatch.Draw(ImageProvider.Instance.GetOpaqueImage(GetMapToken(), 100), GameCamera.Instance.GetMovedVector(field.GetVector()));
            }
        }

        public HashSet<Field> GetFieldsForMovement()
        {
            var result = new HashSet<Field>();
            if (reachableFields.ContainsKey(1))
            {
                reachableFields.TryGetValue(1, out result);
            }

            foreach (var field in FieldHelper.GetAllAdjectedFieldsWithCharacter(Position))
            {
                if (!CanGoThrough(field.GetCharacterOnSelf()))
                {
                    continue;
                }

                var x = (field.GetX() - Position.GetX()) * 2;
                var y = (field.GetY() - Position.GetY()) * 2;

                var found = GameplayProgress.Instance.Scenario.GetFieldAt(Position.GetX() + x, Position.GetY() + y);

                if (found != null && found.GetCharacterOnSelf() == null && RemainingMovement > 1)
                {
                    result.Add(found);
                }
            }
            return result;
        }

        public void MoveToField(Field field)
        {
            if (field != null && RemainingMovement >= Position.DistanceFrom(field))
            {
                var movementUsed = Position.DistanceFrom(field);
                Position.ObjectsOnField.Remove(this);
                field.ObjectsOnField.Add(this);
                Position = field;
                RemainingMovement -= movementUsed;
            }
        }


        //////////// STATUS TEXT METHODS

        public string LivesState()
        {
            if (Dead)
            {
                return "Dead";
            } else
            {
                return TakenDamage + " / " + ActualMaxLives;
            }
        }

        public string MovementState()
        {
            return RemainingMovement + " / " + ActSpeed;
        }

        public bool IsEnemy(Character other)
        {
            return GetSide() != other.GetSide();
        }

        public virtual bool CanGoThrough(Character other)
        {
            return !IsEnemy(other);
        }

        //////////// ABSTRACT INTERFACE

        public abstract ImageListEnum GetMapToken();
        public abstract ImageListEnum GetPortrait();
        public abstract List<IAction> GetPossibleActions();
        public abstract SideEnum GetSide();
    }
}
