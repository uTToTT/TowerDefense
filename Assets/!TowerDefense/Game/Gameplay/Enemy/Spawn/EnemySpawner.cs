using TToTT.TowerDefense.Map;

namespace TToTT.TowerDefense.Enemies
{
    public class EnemySpawner
    {
        private readonly EnemyFactoryRegistry _factory;
        private readonly RouteController _routes;
        private readonly EnemyManager _enemyManager;
        private readonly GridController _grid;

        private int _spawnedCount = 0;

        #region Init

        public EnemySpawner(
            EnemyFactoryRegistry factory,
            RouteController routes,
            EnemyManager enemyManager,
            GridController grid)
        {
            _factory = factory;
            _factory.Init();
            _routes = routes;
            _enemyManager = enemyManager;
            _grid = grid;
        }

        #endregion

        #region Game loop

        public void Restart()
        {
            _factory.ReturnAll();
            _spawnedCount = 0;
        }

        #endregion

        public void Spawn(Group group)
        {
            var enemy = _factory.Create(group.EnemyType);

            InitEnemy(group, enemy);

            _enemyManager.Register(enemy);
            _spawnedCount++;

            // TODO: impement IDebugger
            // Debug.Log(
            //    $"Spawn {group.EnemyType} | Route: {group.Route} | Lane: {group.Lane}"
            //);
        }

        private void InitEnemy(Group group, Enemy enemy)
        {
            #region Edge cases

            if (!_routes.TryGetRoute(group.RouteId, out var route))
            {
                // TODO: implement IDebugger
                throw new System.Exception("Not found route");
            }

            if (!_routes.TryGetRoutePoints(group.RouteId, out var routePoints))
            {
                // TODO: implement IDebugger
                throw new System.Exception("Not found route");
            }

            #endregion

            PathLane lane;

            if (group.Lane == PathLane.LeftRight)
                lane = _spawnedCount % 2 == 0 ? PathLane.Left : PathLane.Right;
            else
                lane = group.Lane;

            var spawnPos = MapUtils.GridToWorld(route.entrance, _grid.Grid);

            enemy.transform.position = spawnPos;
            enemy.HPMultiply(group.HpMultiplier);
            enemy.MoneyDropMultiply(group.MoneyDropMultiplier);
            enemy.SetLane(lane);
            enemy.BuildRoute(routePoints);

            enemy.OnDeath += OnDeath;
        }

        private void OnDeath(Enemy enemy)
        {
            enemy.OnDeath -= OnDeath;
            _factory.Return(enemy);
            _enemyManager.Unregister(enemy);
        }
    }
}