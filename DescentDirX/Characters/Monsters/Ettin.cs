using System.Collections.ObjectModel;
using DescentDirX.Dices;
using DescentDirX.Helpers;
using DescentDirX.Maps;
using DescentDirX.Maps.Tiles.Common;

namespace DescentDirX.Characters.Monsters
{
    class Ettin : Monster, IMonsterDrawable
    {
        public Ettin(string name="Ettin", int maxSpeed = 3, int maxLives = 5) : base(name, maxLives, maxSpeed)
        {
        }

        protected override void Init()
        {
            AddAttackDices(new AttackDice[] { new BlueDice(), new RedDice() });
            AddDefenseDices(new DefenseDice[] { new GreyDice(), new GreyDice() });
            envs.Add(EnviromentEnum.CAVE);
            envs.Add(EnviromentEnum.MOUNTAIN);
        }

        public ImageListEnum GetMonsterSheetBack()
        {
            return ImageListEnum.MONSTERS_ETTIN_1_BACK;
        }

        public ImageListEnum GetMonsterSheetFront()
        {
            return ImageListEnum.MONSTERS_ETTIN_1_FRONT;
        }

        public override ImageListEnum GetMapToken()
        {
            return ImageListEnum.MONSTERS_ETTIN_TOKEN;
        }

        public override Collection<Field> GetPlaceableFields(Field field)
        {
            if (field == null)
            {
                return null;
            }

            Collection<Field> fields = base.GetPlaceableFields(field);
            if (field.RightField != null && field.ParentTile == field.RightField.ParentTile)
            {
                fields.Add(field.RightField);
            }

            if (field.BottomField != null && field.ParentTile == field.BottomField.ParentTile)
            {
                fields.Add(field.BottomField);
            }

            if (field.BottomRightField != null && field.ParentTile == field.BottomRightField.ParentTile)
            {
                fields.Add(field.BottomRightField);
            }

            return fields.Count < 4 ? null : fields;
        }

        public override ImageListEnum GetPortrait()
        {
            return ImageListEnum.MONSTERS_ETTIN_1_PORTRAIT;
        }

        public override int GetReach()
        {
            return 2;
        }
    }
}
