using System;
using System.Collections.Generic;
using UnityEngine;

namespace TToTT.TowerDefense.Map
{
    public class RouteController : IDisposable
    {
        private readonly MapDataService _mapDataService;
        private readonly GridController _gridController;

        #region Init

        public RouteController(
            MapDataService mapDataService,
            GridController gridController)
        {
            _mapDataService = mapDataService;
            _gridController = gridController;
        }

        public void Dispose()
        {

        }

        #endregion

        public Route GetRoute(RouteId routeId) =>
           _mapDataService.GetRoute(routeId);

        public List<Vector3> GetRoutePoints(RouteId routeId)
        {
            var points = new List<Vector3>();
            var route = GetRoute(routeId);

            foreach (var point in route.points)
            {
                points.Add(MapUtils.GridToWorld(point, _gridController.Grid));
            }

            return points;
        }
    }
}