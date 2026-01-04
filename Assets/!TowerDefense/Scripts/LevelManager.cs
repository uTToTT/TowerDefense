using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Image[] _starsImages;
    [SerializeField] private Image _lockImage;
    [SerializeField] private int _level;

    private Button _button;
    private TextMeshProUGUI _lvlText;

    private void Start()
    {
        _button = GetComponent<Button>();
        _lvlText = GetComponentInChildren<TextMeshProUGUI>();

        _lvlText.text = _level.ToString();

        if (IsOpen(_level))
        {
            _button.interactable = true;
            _lockImage.gameObject.SetActive(false);

            if (PlayerPrefs.GetInt("StarsForLevel" + _level) > 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("StarsForLevel" + _level); i++)
                {
                    _starsImages[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            _button.interactable = false;
        }
    }

    private bool IsOpen(int lvl)
    {
        if (_level == 1)
        {
            return true;
        }
        else
        {
            if (PlayerPrefs.GetInt("StarsForLevel" + (lvl - 1)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
