using UnityEngine;

public class StarBonusesInfoManager : MonoBehaviour
{
    [SerializeField] private StarBonusesInfo[] _buffs;

    public void UpdateInfoOfBonuses()
    {
        int starCount = PlayerPrefs.GetInt("StarCount");

        for (int i = 0; i < _buffs.Length; i++)
        {
            PlayerPrefs.SetInt("StarReq" + i, _buffs[i].StarReq);

            if (_buffs[i].StarReq <= starCount)
            {
                _buffs[i].Unlock();
                PlayerPrefs.SetInt(_buffs[i].BuffType, 1);
            }
            else
            {
                _buffs[i].Lock();
                PlayerPrefs.SetInt(_buffs[i].BuffType, 0);
            }
        }
    }
}
