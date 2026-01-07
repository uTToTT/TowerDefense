using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMove : MonoBehaviour
{
    [SerializeField] private MapData mapData;
    [SerializeField] private RouteId routeId = RouteId.A;
    [SerializeField] private float speed = 2f;

    private List<Vector3> pathPoints;

    private void Start()
    {
        BuildPath();
        StartCoroutine(FollowPath());
    }

    private void BuildPath()
    {
        pathPoints = new List<Vector3>();

        for (int y = 0; y < mapData.height; y++)
        {
            for (int x = 0; x < mapData.width; x++)
            {
                
            }
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (var target in pathPoints)
        {
            while ((target - transform.position).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
        }

        Debug.Log("Enemy reached the end!");
        Destroy(gameObject);
    }

    // ================== Gizmos ==================
    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count == 0)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            Gizmos.DrawSphere(pathPoints[i], 0.1f);

            if (i < pathPoints.Count - 1)
            {
                Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
            }
        }
    }
}
