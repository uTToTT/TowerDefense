using TToTT.TowerDefense.Map;
using UnityEngine;

public class EnemySpawner
{
    private readonly EnemyFactoryRegistry _factory;
    private readonly MapRoutes _routes;
    private readonly EnemyManager _enemyManager;

    private int _spawnedCount = 0;

    #region Init

    public EnemySpawner(
        EnemyFactoryRegistry factory,
        MapRoutes routes,
        EnemyManager enemyManager)
    {
        _factory = factory;
        _factory.Init();
        _routes = routes;
        _enemyManager = enemyManager;
    }

    #endregion

    private void Spawn(Group group)
    {
        Enemy enemy = _factory.Create(group.EnemyType);

        PathLane lane;

        if (group.Lane == PathLane.LeftRight)
            lane = _spawnedCount % 2 == 0 ? PathLane.Left : PathLane.Right;
        else
            lane = group.Lane;




        _enemyManager.Register(enemy);

        _spawnedCount++;

        // TODO: impement IDebugger
        // Debug.Log(
        //    $"Spawn {group.EnemyType} | Route: {group.Route} | Lane: {group.Lane}"
        //);
    }

    private void InitEnemy(Group group, Enemy enemy)
    {
        if (!_routes.TryGetRoute(group.RouteId, out var route))
        {
            // TODO: implement IDebugger
            Debug.LogError("Warning! Not found route. Select default");
        }

        var spawnPos = new Vector3()

        enemy.transform.position =
         MapUtils.GridToWorld(
             _routes.TryGetRoute(group.RouteId).entrance,
             MapManager.Instance.Grid);
        enemy.HPMultiply(group.HpMultiplier);
        enemy.MoneyDropMultiply(group.MoneyDropMultiplier);
        enemy.SetLane(lane);
        enemy.BuildRoute(MapManager.Instance.GetRoutePoints(group.RouteId));

        enemy.OnDeath += OnDeath;
    }

    private void OnDeath(Enemy enemy)
    {
        enemy.OnDeath -= OnDeath;
        _factory.Return(enemy);
        EnemyManager.Instance.Unregister(enemy);
    }
}
