using UnityEditor;
using UnityEngine;
using System.IO;

public class MapExportWindow : EditorWindow
{
    private int mapIndex = 0;
    private Vector2 scroll;

    private static readonly string MapsFolder = "Assets/!TowerDefense/!Data/Maps";

    [MenuItem("TD/Map Exporter")]
    public static void Open()
    {
        GetWindow<MapExportWindow>("Map Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Export", EditorStyles.boldLabel);
        GUILayout.Space(6);

        mapIndex = EditorGUILayout.IntField("Map ID", mapIndex);

        string targetPath = $"{MapsFolder}/Map_{mapIndex}.asset";
        bool exists = File.Exists(targetPath);

        if (exists)
        {
            EditorGUILayout.HelpBox(
                "Map with this ID already exists",
                MessageType.Warning
            );
        }

        GUILayout.Space(6);

        GUI.enabled = !exists;
        if (GUILayout.Button("Export New Map", GUILayout.Height(30)))
        {
            TilemapToMapDataExporter.Export(mapIndex);
        }

        GUI.enabled = exists;
        if (GUILayout.Button("Overwrite Map", GUILayout.Height(30)))
        {
            TilemapToMapDataExporter.Export(mapIndex);
        }

        GUI.enabled = true;

        GUILayout.Space(10);
        DrawSavedMaps();
    }

    private void DrawSavedMaps()
    {
        GUILayout.Label("Saved Maps", EditorStyles.boldLabel);

        if (!Directory.Exists(MapsFolder))
        {
            EditorGUILayout.HelpBox("Maps folder not found", MessageType.Info);
            return;
        }

        var files = Directory.GetFiles(MapsFolder, "Map*.asset");

        scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(150));

        foreach (var file in files)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            GUILayout.BeginHorizontal();

            GUILayout.Label(name);

            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                var asset = AssetDatabase.LoadAssetAtPath<Object>(file);
                Selection.activeObject = asset;
                EditorGUIUtility.PingObject(asset);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
