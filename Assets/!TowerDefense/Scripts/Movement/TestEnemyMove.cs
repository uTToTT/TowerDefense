using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMove : MonoBehaviour
{
    [SerializeField] private MapData _mapData;
    [SerializeField] private RouteId _routeId = RouteId.A;
    [SerializeField] private float _speed = 2f;

    private List<Vector3> _pathPoints;
    private Coroutine _routine;

    private void Start()
    {
        BuildPath();
    }

    [ContextMenu(nameof(BuildPath))]
    private void BuildPath()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
        }
        _pathPoints = new List<Vector3>();

        var route = _mapData.GetRoute(_routeId);

        foreach (var p in route.points)
        {
            var pos = MapUtils.GridToWorld(p, _mapData);
            _pathPoints.Add(pos);
        }

        _routine = StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        foreach (var target in _pathPoints)
        {
            while ((target - transform.position).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
                yield return null;
            }
        }

        Debug.Log("Enemy reached the end!");
    }

    // ================== Gizmos ==================
    private void OnDrawGizmos()
    {
        if (_pathPoints == null || _pathPoints.Count == 0)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _pathPoints.Count; i++)
        {
            Gizmos.DrawSphere(_pathPoints[i], 0.1f);

            if (i < _pathPoints.Count - 1)
            {
                Gizmos.DrawLine(_pathPoints[i], _pathPoints[i + 1]);
            }
        }
    }
}
