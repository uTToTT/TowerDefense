using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _starCount;
    private int _starCounter;

    private void Awake()
    {
        CalculateTotalStarCount();
        CheckBonuses();

        foreach (var item in _starCount)
        {
            if (item != null)
            {
                item.text = _starCounter.ToString();
            }
        }
    }

    private void CalculateTotalStarCount()
    {
        _starCounter = 0;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            _starCounter += GetStarsForLevel(i + 1);
        }

        PlayerPrefs.SetInt("StarCount", _starCounter);
    }

    private void CheckBonuses()
    {
        CalculateTotalStarCount();

        int i = 0;

        while (true)
        {
            if (!PlayerPrefs.HasKey("StarReq" + i))
            {
                break;
            }

            if (_starCounter >= PlayerPrefs.GetInt("StarReq" + i))
            {
                PlayerPrefs.SetInt("StarBuff" + i, 1);
            }
            else
            {
                PlayerPrefs.SetInt("StarBuff" + i, 0);
            }

            i++;
        }
    }

    public void SaveStarsForLevel(int stars, int levelIndex)
    {
        if (PlayerPrefs.GetInt("StarsForLevel" + levelIndex) < stars)
        {
            PlayerPrefs.SetInt("StarsForLevel" + levelIndex, stars);
            PlayerPrefs.Save();
        }

        CheckBonuses();
        CalculateTotalStarCount();
    }

    public int GetStarsForLevel(int levelIndex)
    {
        return PlayerPrefs.GetInt("StarsForLevel" + levelIndex);
    }

    private void OnEnable()
    {
        EventBus.GetStar += SaveStarsForLevel;
    }

    private void OnDisable()
    {
        EventBus.GetStar -= SaveStarsForLevel;
    }
}
