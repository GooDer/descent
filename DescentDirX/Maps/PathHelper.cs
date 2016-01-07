using DescentDirX.Maps.Tiles.Common;
using System.Collections.Generic;

namespace DescentDirX.Maps
{
    delegate HashSet<Field> FieldsRetriver(Stack<Field> fields, int radius);

    class PathHelper
    {
        private HashSet<Field> alreadyVisited;
        private Dictionary<int, HashSet<Field>> searchResult;

        public static int passes = 0;

        public PathHelper()
        {
            alreadyVisited = new HashSet<Field>();
            searchResult = new Dictionary<int, HashSet<Field>>();
        }

        public Dictionary<int, HashSet<Field>> GetFieldsInRadius(Field original, int radius, bool includeSelf = false)
        {
            return GetFieldsInRadius(original, radius, GetAllAdjectedFields, includeSelf);
        }

        private Dictionary<int, HashSet<Field>> GetFieldsInRadius(Field original, int radius, FieldsRetriver retriver, bool includeSelf = false)
        {
            if (includeSelf)
            {
                searchResult.Add(0, new HashSet<Field>() { original });
            }
            else
            {
                searchResult.Add(0, new HashSet<Field>() { });
            }

            alreadyVisited.Add(original);
            HashSet<Field> radiusfields;

            Stack<Field> stack = new Stack<Field>();
            stack.Push(original);
            var i = 1;

            while (stack.Count > 0 && i <= radius)
            {
                if (!searchResult.ContainsKey(i))
                {
                    searchResult.Add(i, new HashSet<Field>() { });
                }
                searchResult.TryGetValue(i, out radiusfields);

                HashSet<Field> tmp = retriver(stack, i);
                radiusfields.UnionWith(tmp);

                foreach (var f in tmp)
                {
                    stack.Push(f);
                    alreadyVisited.Add(f);
                }
                i++;
            }

            return searchResult;
        }

        public Dictionary<int, HashSet<Field>> GetFieldsInRadiusForMove(Field original, int radius)
        {
            return GetFieldsInRadius(original, radius, GetAllAdjectedFieldsForMove, false);
        }

        private HashSet<Field> GetAllAdjectedFields(Stack<Field> fields, int radius)
        {
            HashSet<Field> result = new HashSet<Field>();

            Field item;
            while (fields.Count > 0)
            {
                item = fields.Pop();
                alreadyVisited.Add(item);

                if (item.BottomField != null)
                {
                    if (!alreadyVisited.Contains(item.BottomField))
                    {
                        result.Add(item.BottomField);
                    }
                }

                if (item.LeftField != null)
                {
                    if (!alreadyVisited.Contains(item.LeftField))
                    {
                        result.Add(item.LeftField);
                    }
                }

                if (item.RightField != null)
                {
                    if (!alreadyVisited.Contains(item.RightField))
                    {
                        result.Add(item.RightField);
                    }
                }

                if (item.TopField != null)
                {
                    if (!alreadyVisited.Contains(item.TopField))
                    {
                        result.Add(item.TopField);
                    }
                }

                if (item.TopRightField != null)
                {
                    if (!alreadyVisited.Contains(item.TopRightField))
                    {
                        result.Add(item.TopRightField);
                    }
                }

                if (item.BottomRightField != null)
                {
                    if (!alreadyVisited.Contains(item.BottomRightField))
                    {
                        result.Add(item.BottomRightField);
                    }
                }

                if (item.BottomLeftField != null)
                {
                    if (!alreadyVisited.Contains(item.BottomLeftField))
                    {
                        result.Add(item.BottomLeftField);
                    }
                }

                if (item.TopLeftField != null)
                {
                    if (!alreadyVisited.Contains(item.TopLeftField))
                    {
                        result.Add(item.TopLeftField);
                    }
                }
            }

            return result;
        }

        private HashSet<Field> GetAllAdjectedFieldsForMove(Stack<Field> fields, int radius)
        {
            HashSet<Field> result = new HashSet<Field>();

            Field item;
            while (fields.Count > 0)
            {
                item = fields.Pop();
                alreadyVisited.Add(item);

                if (item.BottomField != null && item.BottomField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.BottomField, radius, result);
                }

                if (item.LeftField != null && item.LeftField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.LeftField, radius, result);
                }

                if (item.RightField != null && item.RightField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.RightField, radius, result);
                }

                if (item.TopField != null && item.TopField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.TopField, radius, result);
                }

                if (item.TopRightField != null && item.TopRightField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.TopRightField, radius, result);
                }

                if (item.BottomRightField != null && item.BottomRightField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.BottomRightField, radius, result);
                }

                if (item.BottomLeftField != null && item.BottomLeftField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.BottomLeftField, radius, result);
                }

                if (item.TopLeftField != null && item.TopLeftField.GetCharacterOnSelf() == null)
                {
                    processField(item, item.TopLeftField, radius, result);
                }
            }

            return result;
        }

        private void processField(Field item, Field adjected, int radius, HashSet<Field> result)
        {
            HashSet<Field> waterFields = new HashSet<Field>();

            if (!alreadyVisited.Contains(adjected))
            {
                result.Add(adjected);
            }
        }
    }
}