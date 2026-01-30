using System.Collections.Generic;
using UnityEngine;

public static class MapUtils
{
    public static Vector3 SnapToGrid(Vector3 worldPos, Grid grid)
    {
        var mapPos = WorldToMap(worldPos, grid);
        var snapped = MapToWorld(mapPos, grid);
        return snapped;
    }

    /* =========================
     * MAP <-> WORLD
     * (логика карты)
     * (0,0 Ч левый нижний угол)
     * ========================= */

    public static Vector2Int WorldToMap(Vector3 worldPos, Grid grid)
    {
        Vector3 local = worldPos - grid.transform.position;

        int x = Mathf.FloorToInt(local.x / grid.cellSize.x);
        int y = Mathf.FloorToInt(local.y / grid.cellSize.y);

        return new Vector2Int(x, y);
    }

    public static Vector3 MapToWorld(Vector2Int mapPos, Grid grid)
    {
        return grid.transform.position +
               new Vector3(
                   (mapPos.x + 0.5f) * grid.cellSize.x,
                   (mapPos.y + 0.5f) * grid.cellSize.y,
                   0f
               );
    }

    /* =========================
     * GRID <-> WORLD
     * (Unity Grid)
     * ========================= */

    public static Vector3 GridToWorld(Vector2Int cell, Grid grid) =>
        grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));

    public static Vector3 GridToWorld(int x, int y, Grid grid) =>
        grid.GetCellCenterWorld(new Vector3Int(x, y, 0));

    public static Vector3Int WorldToGrid(Vector3 worldPos, Grid grid) =>
        grid.WorldToCell(worldPos);


    /* =========================
     * Moving with snap to grid
     * ========================= */

    public static void SnapToGridUnderPointer(Transform transform)
    {
        var worldPos = GameManager.Instance.PlayerInputController.GetPointerPosition();
        transform.position = SnapToGrid(worldPos, MapManager.Instance.Grid);
    }


    /* =========================
     * Ports
     * ========================= */
    public static List<WorldPort> GetWorldPorts(IMapObject obj)
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

    public static bool ArePortsConnected(WorldPort a, WorldPort b)
    {
        if (a.Owner == b.Owner)
            return false;

        if (a.Type != b.Type)
            return false;

        if (a.Direction.Opposite() != b.Direction)
            return false;

        return a.Cell + a.Direction.ToOffset() == b.Cell;
    }

    public static void ResolveConnections(IMapObject placedObject)
    {
        var ports = GetWorldPorts(placedObject);

        foreach (var port in ports)
        {
            var targetCell = port.Cell + port.Direction.ToOffset();

            var cellData = MapManager.Instance.GetCellData(targetCell);
            if (cellData?.MapObject == null)
                continue;

            var otherObject = cellData.MapObject;
            var otherPorts = GetWorldPorts(otherObject);

            foreach (var otherPort in otherPorts)
            {
                if (ArePortsConnected(port, otherPort))
                {
                    //ApplyBuff(port, otherPort);
                }
            }
        }
    }
}

