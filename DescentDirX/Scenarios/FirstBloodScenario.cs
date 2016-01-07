using DescentDirX.Maps.Tiles.Common;
using DescentDirX.Maps.Tiles;
using DescentDirX.Maps;
using System.Collections.ObjectModel;
using DescentDirX.Items.Tokens;
using DescentDirX.Characters.Monsters;

namespace DescentDirX.Scenarios
{
    class FirstBloodScenario : Scenario
    {
        public const string NAME = "First Blood";

        private bool FirstGroupPrepared { get; set; }
        private bool SecondGroupPrepared { get; set; }

        private Collection<EnviromentEnum> Env { get; set; }

        public FirstBloodScenario() : base(NAME)
        {
            var exitA = CreateTile("ExitA", new TileExitA(TileRotation.DEGREE_90));
            var tile12A = CreateTile("12A", new Tile12A(TileRotation.DEGREE_270));
            var tile16A = CreateTile("16A", new Tile16A());
            var tile9A = CreateTile("9A", new Tile9A());
            var tileEnterA = CreateTile("EnterA", new TileEnterA(TileRotation.DEGREE_180));
            var tile8A = CreateTile("8A", new Tile8A(TileRotation.DEGREE_180));
            var tile26A = CreateTile("26A", new Tile26A(TileRotation.DEGREE_270));
            var tileEdgeA1 = CreateTile("Edge1", new TileEdgeA(TileRotation.DEGREE_90));
            var tileEdgeA2 = CreateTile("Edge2", new TileEdgeA(TileRotation.DEGREE_90));
            var tileEdgeA3 = CreateTile("Edge3", new TileEdgeA(TileRotation.DEGREE_270));

            Env = new Collection<EnviromentEnum>(new EnviromentEnum[] { EnviromentEnum.WILDRENESS, EnviromentEnum.MOUNTAIN });

            exitA.ConnectTileToBottom(tile12A);
            tile12A.ConnectTileToRight(tile16A);
            tile16A.ConnectTileToBottom(tile9A);
            tile9A.ConnectTileToRight(tileEnterA);
            tile16A.ConnectTileToTop(tile8A);
            tile16A.ConnectTileToRight(tile26A);
            tile9A.ConnectTileToLeft(tileEdgeA1);
            tile8A.ConnectTileToLeft(tileEdgeA2);
            tile26A.ConnectTileToRight(tileEdgeA3);

            PlayerSetup();

            FitMapToScreen();
        }

        protected void PlayerSetup()
        {
            TileForPick = GetTile("EnterA");
            MarkFields();
        }

        protected override void OverlordSetup()
        {
            PrepareNextMonsterGroup();
        }

        protected override void PrepareSearchTokens()
        {
            GetTile("Edge1").GetFieldAt(0, 0).ObjectsOnField.Add(new SearchToken(GetTile("Edge1").GetFieldAt(0, 0)));
            GetTile("Edge3").GetFieldAt(0, 0).ObjectsOnField.Add(new SearchToken(GetTile("Edge3").GetFieldAt(0, 0)));

            if (GetNumOfHeros() > 2)
            {
                GetTile("Edge2").GetFieldAt(0, 0).ObjectsOnField.Add(new SearchToken(GetTile("Edge2").GetFieldAt(0, 0)));
            }

            if (GetNumOfHeros() > 3)
            {
                GetTile("12A").GetFieldAt(0, 4).ObjectsOnField.Add(new SearchToken(GetTile("12A").GetFieldAt(0, 4)));
            }
        }

        private void PrepareFirstMonsterGroup()
        {
            TileForPick = GetTile("26A");
            var monsterGroup = MonsterFactory.createGroup<GoblinArcher>(MonsterEnum.GOBLIN_ARCHER, GetNumOfHeros(), 1);

            foreach (var monster in monsterGroup.GetMonsters())
            {
                UnplacedMonsters.Add(monster);
            }
            FirstGroupPrepared = true;
        }

        private void PrepareSecondMonsterGroup()
        {
            TileForPick = GetTile("8A");
            var monsterGroup = MonsterFactory.CreateEttinMaulerGroup<Ettin>(GetNumOfHeros());

            foreach (var monster in monsterGroup.GetMonsters())
            {
                UnplacedMonsters.Add(monster);
            }
            SecondGroupPrepared = true;
        }

        public override void PrepareNextMonsterGroup()
        {
            if (!FirstGroupPrepared)
            {
                PrepareFirstMonsterGroup();
                return;
            }

            if (!SecondGroupPrepared)
            {
                PrepareSecondMonsterGroup();
                return;
            }

            ClearMarkedFields();
        }
    }
}
