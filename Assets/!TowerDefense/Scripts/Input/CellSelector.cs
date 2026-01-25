using System.Collections.Generic;
using UnityEngine;

public class CellSelector : MonoBehaviour
{
    

    public static List<Vector2Int> GetOccupiedCells(
    Vector2Int anchor,
    TowerShapeSO shape)
    {
        var result = new List<Vector2Int>();

        foreach (var offset in shape.OccupiedCells)
        {
            result.Add(new Vector2Int(
                anchor.x + offset.X,
                anchor.y + offset.Y
            ));
        }

        return result;
    }
}
