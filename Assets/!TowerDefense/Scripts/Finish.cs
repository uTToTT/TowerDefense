using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject _prefabMine;
    private Enemy _tmpEnemy;

    private void Start()
    {
        if (PlayerPrefs.GetInt("StarBuff3") == 1)
        {
            if (_prefabMine != null)
            {
                GameObject tmp = Instantiate(_prefabMine, transform.position, Quaternion.identity);
                tmp.transform.rotation = transform.rotation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            _tmpEnemy = collision.GetComponent<Enemy>();

            //EventBus.HitBase?.Invoke((int)_tmpEnemy.GetDamage());
            WaveController.Instance.UnregisterEnemy(_tmpEnemy);
        }
    }
}
