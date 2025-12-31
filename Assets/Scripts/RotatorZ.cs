using UnityEngine;

public class RotatorZ : MonoBehaviour
{
    [SerializeField] private float _speedRotation;

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, _speedRotation * Time.deltaTime);
    }
}
