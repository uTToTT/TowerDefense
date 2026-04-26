using UnityEngine;

public class CellContainer : MonoBehaviour
{
    public void SetChild(Transform child)
    {
        child.SetParent(transform);
    }
}
