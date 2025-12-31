using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] private GameObject _musicBG;
    [SerializeField] private GameObject[] _soundObjects;
   
    void Awake()
    {
        _soundObjects = GameObject.FindGameObjectsWithTag("Sound");

        if (_soundObjects.Length == 0)
        {
            _musicBG = Instantiate(_musicBG);
            _musicBG.name = "BGMusic1";
            DontDestroyOnLoad(_musicBG.gameObject);
        }
        else
        {
            _musicBG = GameObject.Find("BGMusic1");
        }
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("MusicOn") != 1 && PlayerPrefs.GetInt("MusicOn") != 0)
        {
            PlayerPrefs.SetInt("MusicOn", 1);
        }
    }
}
