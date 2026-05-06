using System;
using TToTT.TowerDefense.Map;

namespace TToTT.TowerDefense.Enemies
{
    public class EnemySpawner
    {
        public event Action<Enemy> OnSpawned;
        public event Action<Enemy> OnDeath;

        private readonly EnemyFactoryRegistry _factory;
        private readonly RouteController _routes;
        private readonly GridController _grid;
        private readonly EconomyController _economyController;
        private readonly IPlayerTarget _playerTarget;

        private int _spawnedCount = 0;

        #region Init

        public EnemySpawner(
            EnemyFactoryRegistry factory,
            RouteController routes,
            GridController grid,
            EconomyController economyController,
            IPlayerTarget playerTarget)
        {
            _factory = factory;
            _factory.Init();
            _routes = routes;
            _grid = grid;
            _economyController = economyController;
            _playerTarget = playerTarget;
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

            OnSpawned?.Invoke(enemy);
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

            var hpBuff = new Buff
                (Tags.ENEMY_SPAWNER,
                Characteristics.HP,
                BuffType.Percent,
                group.HpAdditionalPercent);

            var moneyBuff = new Buff
                (Tags.ENEMY_SPAWNER,
                Characteristics.MONEY_DROP,
                BuffType.Percent,
                group.MoneyDropAdditionalPercent);

            if (group.Lane == PathLane.LeftRight)
                lane = _spawnedCount % 2 == 0 ? PathLane.Left : PathLane.Right;
            else
                lane = group.Lane;

            var spawnPos = MapUtils.GridToWorld(route.entrance, _grid.Grid);

            enemy.transform.position = spawnPos;

            enemy.BuffController.AddOrReplace(hpBuff);
            enemy.BuffController.AddOrReplace(moneyBuff);
            enemy.SetLane(lane);
            enemy.BuildRoute(routePoints);

            enemy.OnReachedFinish += _playerTarget.TakeDamage;
            enemy.OnDeath += Death;
        }

        private void Death(Enemy enemy)
        {
            enemy.OnReachedFinish -= _playerTarget.TakeDamage;
            enemy.OnDeath -= Death;

            _factory.Return(enemy);
            OnDeath?.Invoke(enemy);
            _economyController.AddMoney(enemy.MoneyDrop);
        }
    }
}