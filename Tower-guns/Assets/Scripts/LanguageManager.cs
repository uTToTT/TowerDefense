using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private int _languageID;

    public void SetUkrainian()
    {
        _languageID = 0;
        PlayerPrefs.SetInt("language", _languageID);

        AcceptLang();
    }

    public void SetEnglish()
    {
        _languageID = 1;
        PlayerPrefs.SetInt("language", _languageID);

        AcceptLang();
    }

    private void AcceptLang()
    {
        EventBus.onLangChanged?.Invoke();
    }
}
