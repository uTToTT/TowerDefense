using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToMapDataExporter
{
    [MenuItem("TD/Export Map")]
    public static void Export()
    {
        var path = GameObject.Find("PathTilemap").GetComponent<Tilemap>();
        var build = GameObject.Find("BuildableTilemap").GetComponent<Tilemap>();
        var block = GameObject.Find("BlockedTilemap").GetComponent<Tilemap>();

        var pathBounds = path.cellBounds;
        var buildBounds = build.cellBounds;
        var blockBounds = block.cellBounds;

        int xMin = Mathf.Min(pathBounds.xMin, buildBounds.xMin, blockBounds.xMin);
        int yMin = Mathf.Min(pathBounds.yMin, buildBounds.yMin, blockBounds.yMin);

        int xMax = Mathf.Max(pathBounds.xMax, buildBounds.xMax, blockBounds.xMax);
        int yMax = Mathf.Max(pathBounds.yMax, buildBounds.yMax, blockBounds.yMax);

        Debug.Log($"pathBounds{}");

        var mapData = ScriptableObject.CreateInstance<MapData>();

        mapData.width = xMax - xMin;
        mapData.height = yMax - yMin;

        mapData.cells = new CellType[mapData.width * mapData.height];

        var bounds = path.cellBounds;

        for (int y = 0; y < mapData.height; y++)
        {
            for (int x = 0; x < mapData.width; x++)
            {
                var pos = new Vector3Int(xMin + x, yMin + y, 0);

                if (path.HasTile(pos))
                    mapData.cells[y * mapData.width + x] = CellType.Path;
                else if (build.HasTile(pos))
                    mapData.cells[y * mapData.width + x] = CellType.Buildable;
                else
                    mapData.cells[y * mapData.width + x] = CellType.Blocked;
            }
        }


        AssetDatabase.CreateAsset(mapData, "Assets/!TowerDefense/!Data/Maps/NewMap.asset");
        AssetDatabase.SaveAssets();
    }
}
