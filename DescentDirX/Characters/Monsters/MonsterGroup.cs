using System.Collections.Generic;

namespace DescentDirX.Characters.Monsters
{
    class MonsterGroup<T> where T : Monster
    {
        private List<T> monsters;
        private int numOfMasters;
        private int numOfMinions;

        public MonsterGroup(int numOfMasters, int numOfMinions)
        {
            monsters = new List<T>();
            this.numOfMasters = numOfMasters;
            this.numOfMinions = numOfMinions;
        }

        public int GetMonsterCount()
        {
            return monsters.Count;
        }

        public bool CanAddMaster()
        {
            int count = 0;
            foreach (var monster in monsters)
            {
                if (monster is IMonsterMaster)
                {
                    count++;
                }
            }

            return (count < numOfMasters);
        }

        public bool CanAddMinion()
        {
            int count = 0;
            foreach (var monster in monsters)
            {
                if (!(monster is IMonsterMaster))
                {
                    count++;
                }
            }

            return (count < numOfMinions);
        }

        public void AddMonster(T monster)
        {
            if ((monster is IMonsterMaster && CanAddMaster())
             || (!(monster is IMonsterMaster) && CanAddMinion()))
            {
                monsters.Add(monster);
            }
        }

        public List<T> GetMonsters()
        {
            return monsters;
        }
    }
}
