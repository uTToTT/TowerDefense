using UnityEngine;

public class RangeTower : MonoBehaviour
{
    [SerializeField] private GameObject _spriteRange;

    private Vector3 _defaultScale;

    private void Start()
    {
        _defaultScale = transform.localScale;
    }

    public void SetDefaultRange()
    {
        transform.localScale = _defaultScale;
    }

    public void SetScale(float currMaxAtackRadius)
    {
        _spriteRange.transform.localScale = new Vector3(2 * currMaxAtackRadius, 2 * currMaxAtackRadius, 2 * currMaxAtackRadius);
    }
}
