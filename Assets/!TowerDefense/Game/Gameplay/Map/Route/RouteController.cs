using System;
using System.Collections.Generic;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class RouteController : IDisposable
    {
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

        }

        #endregion

        public void SetRoutes(MapData map) => 
            _routes.SetRoutes(map.routes);

        public bool TryGetRoute(RouteId id, out Route route) =>
            _routes.TryGetRoute(id, out route);

        public bool TryGetRoutePoints(RouteId id, out List<Vector3> points)
        {
            points = null;
            if (!TryGetRoute(id, out var route)) return false;

            points = new List<Vector3>();
            foreach (var point in route.points)
            {
                points.Add(MapUtils.GridToWorld(point, _gridController.Grid));
            }

            return true;
        }
    }
}