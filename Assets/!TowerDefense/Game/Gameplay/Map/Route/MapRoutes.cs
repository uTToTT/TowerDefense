using System.Collections.Generic;

public class MapRoutes
{
    private Dictionary<RouteId,Route> _routesMap = new();

    public void SetRoutes(List<Route> routes)
    {
        foreach (Route route in routes)
        {
            _routesMap.Add(route.routeId, route);
        }
    }

    public bool TryGetRoute(RouteId id, out Route route)
    {
        route = new Route();
        if (!_routesMap.ContainsKey(id)) return false;
        route = _routesMap[id];

        return true;
    }
}
