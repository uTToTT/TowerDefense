using UnityEngine;

public static class MapUtils
{
    public static Vector3 GridToWorld(int x, int y, MapData map)
    {
        float xOffset = (map.width - 1) * 0.5f;
        float yOffset = (map.height - 1) * 0.5f;

        return new Vector3(
            (x - xOffset) * map.cellSize,
            (y - yOffset) * map.cellSize,
            0f
        );
    }
}
