using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController
{
    private readonly Queue<Vector3> _waypoints = new();

    #region ==== Properties ====
    public Vector3 Current => _waypoints.Peek();
    public Vector3 Previous { get; private set; }

    public bool HasPath => _waypoints.Count > 0;

    #endregion =================

    public void Enqueue(Vector3 point) => _waypoints.Enqueue(point);
    public Vector3 Peek() => _waypoints.Peek();

    public void Dequeue()
    {
        if (_waypoints.Count > 0)
            _waypoints.Dequeue();
    }

    public void Clear() => _waypoints.Clear();

    public static Vector2 PerpendicularLeft(Vector2 v)
       => new Vector2(-v.y, v.x);

    public static bool LineIntersection(
        Vector2 p1, Vector2 p2,
        Vector2 p3, Vector2 p4,
        out Vector2 intersection)
    {
        intersection = Vector2.zero;

        Vector2 r = p2 - p1;
        Vector2 s = p4 - p3;
        float denom = r.x * s.y - r.y * s.x;

        if (Mathf.Abs(denom) < 0.0001f)
            return false;

        float t = ((p3 - p1).x * s.y - (p3 - p1).y * s.x) / denom;
        intersection = p1 + t * r;
        return true;
    }

    public static List<Vector3> OffsetPath(
    List<Vector3> path,
    float offset,
    bool leftSide = true)
    {
        int count = path.Count;
        if (count < 2)
            return new List<Vector3>(path);

        List<Vector3> result = new(count);

        for (int i = 0; i < count; i++)
        {
            if (i == 0 || i == count - 1)
            {
                Vector3 dir = (path[Mathf.Clamp(i + 1, 0, count - 1)] -
                               path[Mathf.Clamp(i - 1, 0, count - 1)]).normalized;

                Vector3 normal = PerpendicularLeft(dir);
                if (!leftSide) normal = -normal;

                result.Add(path[i] + normal * offset);
                continue;
            }

            Vector2 prev = path[i - 1];
            Vector2 curr = path[i];
            Vector2 next = path[i + 1];

            Vector2 dirA = (curr - prev).normalized;
            Vector2 dirB = (next - curr).normalized;

            Vector2 nA = PerpendicularLeft(dirA);
            Vector2 nB = PerpendicularLeft(dirB);

            if (!leftSide)
            {
                nA = -nA;
                nB = -nB;
            }

            Vector2 a1 = prev + nA * offset;
            Vector2 a2 = curr + nA * offset;

            Vector2 b1 = curr + nB * offset;
            Vector2 b2 = next + nB * offset;

            if (LineIntersection(a1, a2, b1, b2, out Vector2 intersect))
                result.Add(intersect);
            else
                result.Add(curr + nA * offset);
        }

        return result;
    }

}
