using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 
/// TODO:
/// - MapLoader
/// - MapDebugger
/// 
/// </summary>

namespace TToTT.TowerDefense.Map
{
    public class MapManager
    {
        private readonly MapController _mapController;
       

        [SerializeField] private bool _debug; // relocate
        [SerializeField] private bool _drawPorts; // relocate

        private bool _isDrawMapObjectPorts; // relocate
        private MapObject _selectedMapObject; // relocate

        #region Init

        public MapManager(
            MapController mapController)
        {
            _mapController = mapController;
        }

        public void Init()
        {

        }

        #endregion

        #region GameLoop

        public void Tick(float dt)
        {
        }

        public void Restart()
        {

        }

        #endregion

        #region Unity API

        private void OnDrawGizmos()
        {
            if (!_drawPorts) return;
            if (_isDrawMapObjectPorts &&
                _selectedMapObject.Shape.Ports != null &&
                _selectedMapObject.Shape.Ports.Length > 0)
            {
                DrawPorts(_selectedMapObject);
            }
        }

        #endregion

       

        public CellData Raycast()
        {
            if (GameLoop.Instance.PlayerInputController.IsPointerOverUI())
                return null;

            var worldPos = GameLoop.Instance.PlayerInputController.GetPointerPosition();
            var mapPos = MapUtils.WorldToMap(worldPos, Grid);

            return GetCellData(mapPos);
        }

        #region Ports

        public void ShowMapObjectPorts(MapObject mapObject)
        {
            _isDrawMapObjectPorts = true;
            _selectedMapObject = mapObject;
        }

        public void HideMapObjectPorts()
        {
            _isDrawMapObjectPorts = false;
            _selectedMapObject = null;
        }

        public static List<WorldPort> GetWorldPorts(MapObject obj)
        {
            var result = new List<WorldPort>();

            foreach (var port in obj.Shape.Ports)
            {
                var worldCell = new Vector2Int(
                    obj.MapPos.x + port.Cell.X,
                    obj.MapPos.y + port.Cell.Y
                );

                result.Add(new WorldPort
                {
                    Owner = obj,
                    Cell = worldCell,
                    Direction = port.Direction,
                    Type = port.Type
                });
            }

            return result;
        }

        public void ResolveConnections(MapObject placedObject)
        {
            var ports = GetWorldPorts(placedObject);

            foreach (var port in ports)
            {
                var targetCell = port.Cell + port.Direction.ToOffset();

                var cellData = GetCellData(targetCell);
                if (cellData?.MapObject == null ||
                    cellData?.MapObject is not MapObject otherObject)
                    continue;

                var otherPorts = GetWorldPorts(otherObject);

                foreach (var otherPort in otherPorts)
                {
                    if (MapUtils.ArePortsConnected(port, otherPort))
                    {
                        Debug.Log($"[{port.Cell}]&[{otherPort.Cell}] | [{port.Type}] Connected");
                        //ApplyBuff(port, otherPort);
                    }
                }
            }
        }

        public static List<KeyValuePair<Vector2Int, PortDirection>> GetPortCells(
           Vector2Int anchor,
           MapObjectShape shape)
        {
            var result = new List<KeyValuePair<Vector2Int, PortDirection>>();

            foreach (var offset in shape.Ports)
            {
                var cell = new Vector2Int(
                    anchor.x + offset.Cell.X,
                    anchor.y + offset.Cell.Y);

                result.Add(new KeyValuePair<Vector2Int, PortDirection>(
                    cell,
                    offset.Direction));
            }

            return result;
        }

        private void DrawPorts(MapObject mapObject)
        {
            var ports = GetPortCells(mapObject.MapPos, mapObject.Shape);

            for (int i = 0; i < ports.Count; i++)
            {
                var portOrigin = MapUtils.MapToWorld(ports[i].Key, Grid);
                var portEnd = MapUtils.MapToWorld(ports[i].Key + ports[i].Value.ToOffset(), Grid);

                Debug.DrawLine(portOrigin, portEnd, Color.red);
            }
        }

        #endregion
    }
}