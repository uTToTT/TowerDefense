using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToMapDataExporter
{
    private const string ROUTE_A = "Route_A_TMap";
    private const string ROUTE_B = "Route_B_TMap";
    private const string ROUTE_C = "Route_C_TMap";

    private const string BUILDABLE = "Buildable_TMap";
    private const string BLOCKED = "Blocked_TMap";
    private const string ENTRANCE = "Entrance_TMap";
    private const string EXIT = "Exit_TMap";

    private const string MAP_DATA_PATH = "Assets/!TowerDefense/!Data/Maps/Map_{0}.asset";

    public static void Export(int mapIndex = 0)
    {
        var mapData = ScriptableObject.CreateInstance<MapData>();

        var routeIds = new (string name, RouteId routeId)[]
        {
            (ROUTE_A, RouteId.A),
            (ROUTE_B, RouteId.B),
            (ROUTE_C, RouteId.C)
        };

        var build = GameObject.Find(BUILDABLE)?.GetComponent<Tilemap>();
        var block = GameObject.Find(BLOCKED)?.GetComponent<Tilemap>();
        var entrance = GameObject.Find(ENTRANCE)?.GetComponent<Tilemap>();
        var exit = GameObject.Find(EXIT)?.GetComponent<Tilemap>();

        int xMin = int.MaxValue;
        int yMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMax = int.MinValue;


        var routeTmaps = new Dictionary<RouteId, Tilemap>();
        var allMaps = new List<Tilemap>
        {
            build, block, entrance, exit,
        };

        foreach (var r in routeIds)
        {
            var tm = GameObject.Find(r.name)?.GetComponent<Tilemap>();
            if (tm != null)
            {
                allMaps.Add(tm);
                routeTmaps.Add(r.routeId, tm);
            }
        }

        foreach (var map in allMaps)
        {
            if (map == null) continue;
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (!map.HasTile(pos)) continue;

                xMin = Mathf.Min(xMin, pos.x);
                yMin = Mathf.Min(yMin, pos.y);
                xMax = Mathf.Max(xMax, pos.x + 1);
                yMax = Mathf.Max(yMax, pos.y + 1);
            }
        }

        if (xMin == int.MaxValue)
        {
            Debug.LogError("No tiles found on any Tilemap");
            return;
        }

        int width = xMax - xMin;
        int height = yMax - yMin;

        mapData.width = width;
        mapData.height = height;

        mapData.cells = new CellType[width * height];
        mapData.routes = new List<Route>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector3Int(xMin + x, yMin + y, 0);
                int index = y * width + x;

                bool hasPath = false;

                foreach (var r in routeTmaps)
                {
                    if (r.Value != null && r.Value.HasTile(pos))
                    {
                        hasPath = true;
                        break;
                    }
                }

                if (entrance != null && entrance.HasTile(pos))
                {
                    mapData.cells[index] = CellType.Entrance;
                    mapData.entranceCount++;
                }
                else if (exit != null && exit.HasTile(pos))
                {
                    mapData.cells[index] = CellType.Exit;
                    mapData.exitCount++;
                }
                else if (hasPath)
                {
                    mapData.cells[index] = CellType.Path;
                    mapData.pathCount++;
                }
                else if (block != null && block.HasTile(pos))
                {
                    mapData.cells[index] = CellType.Blocked;
                    mapData.blockedCount++;
                }
                else
                {
                    mapData.cells[index] = CellType.Buildable;
                    mapData.blockedCount++;
                }
            }
        }

        int routeCount = Mathf.Max(mapData.entranceCount, mapData.exitCount);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector3Int(xMin + x, yMin + y, 0);
                int index = y * width + x;

                if (entrance == null || !entrance.HasTile(pos))
                    continue;

                var usedRoutes = new HashSet<RouteId>();

                for (int i = 0; i < routeCount; i++)
                {
                    Route route = new Route();
                    route.points = new();
                    route.entrance = (Vector2Int)pos;

                    bool hasPath = false;
                    RouteId routeId = RouteId.None;
                    var tmpPos = new Vector3Int(pos.x, pos.y);

                    foreach (var r in routeTmaps)
                    {
                        if (r.Value != null && r.Value.HasTile(pos))
                        {
                            if (usedRoutes.Add(r.Key) == false)
                            {
                                continue;
                            }
                            hasPath = true;
                            routeId = r.Key;

                            break;
                        }
                    }

                    if (hasPath)
                    {
                        var r = routeTmaps[routeId];

                        var size = width * height;

                        for (int j = 0; j < size; j++)
                        {
                            route.points.Add((Vector2Int)tmpPos);

                            var matrix = r.GetTransformMatrix(tmpPos);
                            float angle = Mathf.Round(matrix.rotation.eulerAngles.z / 90f) * 90f;

                            Direction dir = angle switch
                            {
                                0f => Direction.Right,
                                90f => Direction.Up,
                                180f => Direction.Left,
                                270f => Direction.Down,
                                _ => Direction.None
                            };

                            if (dir == Direction.Right)
                                tmpPos.x++;
                            else if (dir == Direction.Up)
                                tmpPos.y++;
                            else if (dir == Direction.Left)
                                tmpPos.x--;
                            else if (dir == Direction.Down)
                                tmpPos.y--;

                            if (mapData.IsInside(tmpPos.x, tmpPos.y) == false)
                            {
                                throw new System.ArgumentOutOfRangeException("Out of map");
                            }

                            if (exit != null && exit.HasTile(tmpPos))
                            {
                                route.exit = (Vector2Int)tmpPos;
                                mapData.routes.Add(route);
                                break;
                            }
                        }
                    }
                }
            }
        }

        SaveMap(mapIndex, mapData);
    }

    private static void SaveMap(int mapIndex, MapData mapData)
    {
        string path = string.Format(MAP_DATA_PATH, mapIndex);
        var existing = AssetDatabase.LoadAssetAtPath<MapData>(path);
        if (existing != null)
        {
            bool confirm = EditorUtility.DisplayDialog(
                "Map already exists",
                $"Map '{Path.GetFileName(path)}' already exists.\n\nOverwrite?",
                "Overwrite",
                "Cancel"
            );
            if (!confirm)
            {
                Debug.Log("Map export cancelled");
                return;
            }
            AssetDatabase.DeleteAsset(path);
        }

        AssetDatabase.CreateAsset(mapData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Map exported: {mapData.width}x{mapData.height}");
    }
}
