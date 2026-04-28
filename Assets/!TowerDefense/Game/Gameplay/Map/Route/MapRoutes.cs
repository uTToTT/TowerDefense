using System.Collections.Generic;

public class MapRoutes
{
    private Dictionary<RouteId,Route> _routesMap = new();

    public void SetRoutes(List<Route> routes)
    {
        _routesMap.Clear();
        foreach (var route in routes)
            _routesMap[route.routeId] = route;
    }

    public bool TryGetRoute(RouteId id, out Route route) =>
        _routesMap.TryGetValue(id, out route);
}
