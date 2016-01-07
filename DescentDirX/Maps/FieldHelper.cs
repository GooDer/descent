using DescentDirX.Maps.Tiles.Common;
using System.Collections.Generic;

namespace DescentDirX.Maps
{
    class FieldHelper
    {
        /// <summary>
        /// Private constructor - this helper should not be instantiated
        /// </summary>
        private FieldHelper()
        {

        }

        public static List<Field> GetAllAdjectedFieldsWithCharacter(Field field)
        {
            List<Field> result = new List<Field>();

            foreach (var res in GetAllAdjectedFields(field))
            {
                if (res.GetCharacterOnSelf() != null)
                {
                    result.Add(res);
                }
            }

            return result;
        }

        public static List<Field> GetAllAdjectedFields(Field field)
        {
            List<Field> result = new List<Field>();
            if (field.LeftField != null)
            {
                result.Add(field.LeftField);
            }

            if (field.RightField != null)
            {
                result.Add(field.RightField);
            }

            if (field.BottomField != null)
            {
                result.Add(field.BottomField);
            }

            if (field.TopField != null)
            {
                result.Add(field.TopField);
            }

            if (field.TopLeftField != null)
            {
                result.Add(field.TopLeftField);
            }

            if (field.TopRightField != null)
            {
                result.Add(field.TopRightField);
            }

            if (field.BottomLeftField != null)
            {
                result.Add(field.BottomLeftField);
            }

            if (field.BottomRightField != null)
            {
                result.Add(field.BottomRightField);
            }
            return result;
        }
    }
}
