using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Route
{
    public RouteId routeId;
    public Vector2Int entrance;
    public Vector2Int exit;
    public List<Vector2Int> points;
}
