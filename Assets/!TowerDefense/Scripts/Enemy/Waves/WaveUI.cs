using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [Header("Frame")]
    [SerializeField] private GameObject _frameWaveInfo;
    [SerializeField] private TextMeshProUGUI _textHP;
    [SerializeField] private TextMeshProUGUI _textArmor;
    [SerializeField] private Image _imageEnemy;
    [SerializeField] private float _delayWaveInfoShow;
    [Space]
    [Header("Prefabs")]
    [SerializeField] private Sprite _spriteFastEnemy;
    [SerializeField] private Sprite _spriteClassisEnemy;
    [SerializeField] private Sprite _spriteArmorEnemy;
    [SerializeField] private Sprite _spriteHeavyEnemy;
    [SerializeField] private Sprite _spriteKingEnemy;


    private void EnableWaveInfoFrame(Enemy currEnemy)
    {
        if (_frameWaveInfo != null)
        {
            _textHP.text = currEnemy.HP.ToString();
            _textArmor.text = (currEnemy.Shield * 100).ToString() + "%";
            //Debug.Log("Speed: " + currEnemy.Shield);

            if (currEnemy.GetEnemyType() == EnemyType.Fast)
            {
                _imageEnemy.sprite = _spriteFastEnemy;
            }
            else if (currEnemy.GetEnemyType() == EnemyType.Classic)
            {
                _imageEnemy.sprite = _spriteClassisEnemy;
            }
            else if(currEnemy.GetEnemyType() == EnemyType.Armor)
            {
                _imageEnemy.sprite = _spriteArmorEnemy;
            }
            else if(currEnemy.GetEnemyType() == EnemyType.Heavy)
            {
                _imageEnemy.sprite = _spriteHeavyEnemy;
            }
            else if(currEnemy.GetEnemyType() == EnemyType.King)
            {
                _imageEnemy.sprite = _spriteKingEnemy;
            }
        }

        StartCoroutine(WaveInfoFrameShow());   
    }

    IEnumerator WaveInfoFrameShow()
    {
        _frameWaveInfo.SetActive(true);
        yield return new WaitForSeconds(_delayWaveInfoShow);
        _frameWaveInfo.SetActive(false);
    }

    private void OnEnable()
    {
        EventBus.onShowEnemyInfo += EnableWaveInfoFrame;
    }

    private void OnDisable()
    {
        EventBus.onShowEnemyInfo -= EnableWaveInfoFrame;
    }
}
