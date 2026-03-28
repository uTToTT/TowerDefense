using System;

namespace TToTT.TowerDefense.Map
{
    /// <summary>
    /// 
    /// TODO:
    /// - Route Controller/Provider
    /// - MapValidator
    /// - MapDataController
    /// - Dispose()
    /// 
    /// </summary>

    public class MapController : IDisposable
    {
        private readonly PlacementController _placementController;
        private readonly GridController _gridController;
        private readonly MapComposer _mapComposer;
        private readonly MapValidator _mapValidator;
        private readonly MapDataService _mapDataService;

        #region Init

        public MapController(
            PlacementController placementController,
            GridController gridController,
            MapComposer mapComposer,
            MapValidator mapValidator,
            MapDataService dataService)
        {
            _placementController = placementController;
            _gridController = gridController;
            _mapComposer = mapComposer;
            _mapValidator = mapValidator;
            _mapDataService = dataService;
        }

        public void Dispose()
        {
            _placementController.Dispose();
            _gridController.Dispose();
            _mapComposer.Dispose();
            _mapValidator.Dispose();
            _mapDataService.Dispose();
        }

        #endregion

        public void BuildMap(MapData mapData)
        {
            _mapDataService.SetMapData(mapData);
            _gridController.CenterGrid(mapData.width, mapData.height);
            _mapComposer.Build(mapData, _gridController.Grid);
        }
    }
}