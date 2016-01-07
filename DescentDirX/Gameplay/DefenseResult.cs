using DescentDirX.Dices;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DescentDirX.Gameplay
{
    class DefenseResult
    {
        public Collection<IDefenseDice> DefenseDices { get; private set; }

        public DefenseResult()
        {
            DefenseDices = new Collection<IDefenseDice>();
        }

        public int GetDefense()
        {
            int result = 0;
            foreach (var dice in DefenseDices)
            {
                result += dice.GetDefense();
            }
            return result;
        }

        public void RerollAllDices()
        {
            foreach (var dice in DefenseDices)
            {
                dice.RollDefense();
            }
        }

        public void RerollSpecificDice(int index)
        {
            DefenseDices[index].RollDefense();
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var dice in DefenseDices)
            {
                if (dice != DefenseDices.First())
                {
                    result.AppendLine("");
                }
                result.Append("Rolled on " + dice + ": " + dice.GetDefense());
            }

            return result.ToString();
        }
    }
}
