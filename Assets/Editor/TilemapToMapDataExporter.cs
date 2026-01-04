using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToMapDataExporter
{
    public static void Export(int mapIndex = 0)
    {
        var routes = new (string name, RouteId routeId)[]
        {
        ("PathATilemap", RouteId.A),
        ("PathBTilemap", RouteId.B),
        ("PathCTilemap", RouteId.C)
        };

        var build = GameObject.Find("BuildableTilemap")?.GetComponent<Tilemap>();
        var block = GameObject.Find("BlockedTilemap")?.GetComponent<Tilemap>();
        var entrance = GameObject.Find("EntranceTilemap")?.GetComponent<Tilemap>();
        var exit = GameObject.Find("ExitTilemap")?.GetComponent<Tilemap>();

        int xMin = int.MaxValue;
        int yMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMax = int.MinValue;

        var allMaps = new List<Tilemap>
        {
            build, block, entrance, exit,
        };

        foreach (var r in routes)
        {
            var tm = GameObject.Find(r.name)?.GetComponent<Tilemap>();
            if (tm != null) allMaps.Add(tm);
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

        var mapData = ScriptableObject.CreateInstance<MapData>();
        mapData.width = width;
        mapData.height = height;
        mapData.cells = new CellType[width * height];
        mapData.flows = new List<FlowData>[width * height];

        for (int i = 0; i < mapData.flows.Length; i++)
            mapData.flows[i] = new List<FlowData>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector3Int(xMin + x, yMin + y, 0);
                int index = y * width + x;

                bool isPath = false;
                foreach (var r in routes)
                {
                    var tilemap = GameObject.Find(r.name)?.GetComponent<Tilemap>();
                    if (tilemap == null) continue;
                    if (!tilemap.HasTile(pos)) continue;

                    isPath = true;

                    var matrix = tilemap.GetTransformMatrix(pos);
                    float angle = Mathf.Round(matrix.rotation.eulerAngles.z / 90f) * 90f;
                    Direction dir = angle switch
                    {
                        0f => Direction.Right,
                        90f => Direction.Up,
                        180f => Direction.Left,
                        270f => Direction.Down,
                        _ => Direction.None
                    };

                    mapData.flows[index].Add(new FlowData { routeId = r.routeId, dir = dir });
                }

                if (entrance != null && entrance.HasTile(pos))
                    mapData.cells[index] = CellType.Entrance;
                else if (exit != null && exit.HasTile(pos))
                    mapData.cells[index] = CellType.Exit;
                else if (isPath)              
                    mapData.cells[index] = CellType.Path;
                else if (block != null && block.HasTile(pos))
                    mapData.cells[index] = CellType.Blocked;
                else 
                    mapData.cells[index] = CellType.Buildable;
            }
        }

        string path = $"Assets/!TowerDefense/!Data/Maps/Map_{mapIndex}.asset";
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
        Debug.Log($"Map exported: {width}x{height}");
    }
}
