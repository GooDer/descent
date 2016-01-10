using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;
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

        /// <summary>
        /// Get all fields with any character on in adjected to given field
        /// </summary>
        /// <param name="field">field from which check adjected fields</param>
        /// <returns>list of fields with character on it</returns>
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

        /// <summary>
        /// Get all fields adjected to given field
        /// </summary>
        /// <param name="field">field from which we want all adjected fields</param>
        /// <returns>list of all adjected fields</returns>
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

        /// <summary>
        /// Rotate field to reflect tile rotation
        /// </summary>
        /// <param name="field">field to rotate</param>
        /// <param name="rotation">rotation - 0 / 90 / 180 / 270 degrees</param>
        /// <param name="width">tile width</param>
        /// <param name="height">tile height</param>
        public static void RotateField(Field field, TileRotation rotation, int width, int height)
        {
            var newVector = MapVector.GetInversedVector(new Vector2(field.Rect.X, field.Rect.Y), width, height, rotation);
            var newX = (int)newVector.Value.X;
            var newY = (int)newVector.Value.Y;

            var tempOpenLeft = field.OpenLeft;
            var tempOpenRight = field.OpenRight;
            var tempOpenTop = field.OpenTop;
            var tempOpenBottom = field.OpenBottom;

            var tempTopField = field.TopField;
            var tempBottomField = field.BottomField;
            var tempLeftField = field.LeftField;
            var tempRightField = field.RightField;
            var tempTopRightField = field.TopRightField;
            var tempTopLeftField = field.TopLeftField;
            var tempBottomLeftField = field.BottomLeftField;
            var tempBottomRightField = field.BottomRightField;

            switch (rotation)
            {
                case TileRotation.DEGREE_90:
                    newX -= field.Rect.Width;
                    field.OpenLeft = tempOpenBottom;
                    field.OpenRight = tempOpenTop;
                    field.OpenTop = tempOpenLeft;
                    field.OpenBottom = tempOpenRight;

                    field.TopField = tempLeftField;
                    field.BottomField = tempRightField;
                    field.LeftField = tempBottomField;
                    field.RightField = tempTopField;

                    field.TopLeftField = tempBottomLeftField;
                    field.TopRightField = tempTopLeftField;
                    field.BottomRightField = tempTopRightField;
                    field.BottomLeftField = tempBottomRightField;
                    break;
                case TileRotation.DEGREE_180:
                    newX -= field.Rect.Width;
                    newY -= field.Rect.Height;
                    field.OpenLeft = tempOpenRight;
                    field.OpenRight = tempOpenLeft;
                    field.OpenTop = tempOpenBottom;
                    field.OpenBottom = tempOpenTop;

                    field.LeftField = tempRightField;
                    field.RightField = tempLeftField;
                    field.TopField = tempBottomField;
                    field.BottomField = tempTopField;

                    field.TopLeftField = tempBottomRightField;
                    field.TopRightField = tempBottomLeftField;
                    field.BottomRightField = tempTopLeftField;
                    field.BottomLeftField = tempTopRightField;
                    break;
                case TileRotation.DEGREE_270:
                    newY -= field.Rect.Height;
                    field.OpenLeft = tempOpenTop;
                    field.OpenRight = tempOpenBottom;
                    field.OpenTop = tempOpenRight;
                    field.OpenBottom = tempOpenLeft;

                    field.LeftField = tempTopField;
                    field.RightField = tempBottomField;
                    field.TopField = tempRightField;
                    field.BottomField = tempLeftField;

                    field.TopLeftField = tempTopRightField;
                    field.TopRightField = tempBottomRightField;
                    field.BottomRightField = tempBottomLeftField;
                    field.BottomLeftField = tempTopLeftField;
                    break;
            }

            field.Rect = new Rectangle(newX, newY, field.Rect.Width, field.Rect.Height);
            field.OriginRect = field.Rect;
        }
    }
}
