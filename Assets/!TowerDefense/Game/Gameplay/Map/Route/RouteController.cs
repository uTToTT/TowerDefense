using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class RouteController : IDisposable
    {
        private readonly Dictionary<RouteId, List<Vector3>> _cachedPoints = new();

        private readonly GridController _gridController;
        private readonly MapRoutes _routes;

        #region Init

        public RouteController(           
            GridController gridController, MapRoutes routes)
        {
            _gridController = gridController;
            _routes = routes;
        }

        public void Dispose()
        {
            _cachedPoints.Clear();
        }

        #endregion

        public void SetRoutes(MapData map)
        {
            _routes.SetRoutes(map.routes);
            _cachedPoints.Clear();

            foreach (var route in map.routes)
            {
                var points = route.points
                    .Select(p => MapUtils.MapToWorld(p, _gridController.Grid))
                    .ToList();

                _cachedPoints[route.routeId] = points;
            }
        }

        public bool TryGetRoute(RouteId id, out Route route) =>
            _routes.TryGetRoute(id, out route);

        public bool TryGetRoutePoints(RouteId id, out List<Vector3> points) =>
            _cachedPoints.TryGetValue(id, out points);
    }
}