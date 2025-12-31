using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfxExplosion;
    [SerializeField] private GameObject _gameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().Death();

            if (_vfxExplosion != null)
            {
                _vfxExplosion.gameObject.transform.SetParent(null);
                _vfxExplosion.Play();
                Destroy(_vfxExplosion.gameObject, 3f);
            }

            Destroy(_gameObject);
        }
    }
}
