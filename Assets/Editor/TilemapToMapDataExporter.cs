using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToMapDataExporter
{
    [MenuItem("TD/Export Map")]
    public static void Export()
    {
        var path = GameObject.Find("PathTilemap")?.GetComponent<Tilemap>();
        var build = GameObject.Find("BuildableTilemap")?.GetComponent<Tilemap>();
        var block = GameObject.Find("BlockedTilemap")?.GetComponent<Tilemap>();

        Tilemap[] maps = { path, build, block };

        int xMin = int.MaxValue;
        int yMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMax = int.MinValue;

        foreach (var map in maps)
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

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector3Int(xMin + x, yMin + y, 0);

                if (path != null && path.HasTile(pos))
                    mapData.cells[y * width + x] = CellType.Path;
                else if (build != null && build.HasTile(pos))
                    mapData.cells[y * width + x] = CellType.Buildable;
                else
                    mapData.cells[y * width + x] = CellType.Blocked;
            }
        }

        AssetDatabase.CreateAsset(
            mapData,
            "Assets/!TowerDefense/!Data/Maps/NewMap.asset"
        );
        AssetDatabase.SaveAssets();

        Debug.Log($"Map exported: {width}x{height}");
    }
}
