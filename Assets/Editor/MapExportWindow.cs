using UnityEditor;
using UnityEngine;

public class MapExportWindow : EditorWindow
{
    [MenuItem("TD/Map Exporter")]
    public static void Open()
    {
        GetWindow<MapExportWindow>("Map Exporter");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Export Map"))
        {
            TilemapToMapDataExporter.Export();
        }
    }
}
