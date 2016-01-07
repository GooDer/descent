
namespace DescentDirX.Characters.Monsters
{
    class MonsterFactory
    {
        public static Monster create(MonsterEnum type, int numOfPlayers, int act, bool master)
        {
            switch (type)
            {
                case MonsterEnum.GOBLIN_ARCHER:
                    if (master)
                    {
                        return new MasterGoblinArcher();
                    }
                    else
                    {
                        return new GoblinArcher();
                    }
                case MonsterEnum.ETTIN:
                    if (master)
                    {
                        return new MasterEttin();
                    }
                    else
                    {
                        return new Ettin();
                    }
            }

            return null;
        }

        public static MonsterGroup<T> createGroup<T>(MonsterEnum type, int numOfPlayers, int act = 1) where T : Monster
        {
            MonsterGroup<T> group = null;
            switch (type)
            {
                case MonsterEnum.GOBLIN_ARCHER :
                    group = new MonsterGroup<T>(1, 2);

                    if (numOfPlayers > 2)
                    {
                        group = new MonsterGroup<T>(1, 3);
                    }
                    if (numOfPlayers > 3)
                    {
                        group = new MonsterGroup<T>(1, 4);
                    }

                    group.AddMonster((T)create(type, numOfPlayers, act, true));
                    group.AddMonster((T)create(type, numOfPlayers, act, false));
                    group.AddMonster((T)create(type, numOfPlayers, act, false));
                    group.AddMonster((T)create(type, numOfPlayers, act, false));
                    group.AddMonster((T)create(type, numOfPlayers, act, false));
                    break;
                case MonsterEnum.ETTIN :
                    if (numOfPlayers == 2)
                    {
                        group = new MonsterGroup<T>(0, 1);
                    }
                    else if (numOfPlayers == 3)
                    {
                        group = new MonsterGroup<T>(1, 0);
                    }
                    else if (numOfPlayers == 4)
                    {
                        group = new MonsterGroup<T>(1, 1);
                    }
                    group.AddMonster((T)create(type, numOfPlayers, act, false));
                    group.AddMonster((T)create(type, numOfPlayers, act, true));
                    break;
            }
            return group;
        }

        public static MonsterGroup<Ettin> CreateEttinMaulerGroup<T>(int numOfPlayers, int act = 1)
        {
            MonsterGroup<Ettin> group = null;

            if (numOfPlayers == 2)
            {
                group = new MonsterGroup<Ettin>(0, 1);
                group.AddMonster(new Ettin(name:  "Mauler", maxLives: 5 + numOfPlayers * 2));
            }
            else if (numOfPlayers == 3)
            {
                group = new MonsterGroup<Ettin>(1, 0);
                group.AddMonster(new MasterEttin(name: "Mauler", maxLives: 8 + numOfPlayers * 2));
            }
            else if (numOfPlayers == 4)
            {
                group = new MonsterGroup<Ettin>(1, 1);
                group.AddMonster(new Ettin());
                group.AddMonster(new MasterEttin(name: "Mauler", maxLives: 8 + numOfPlayers * 2));
            }

            return group;
        }
    }
}
