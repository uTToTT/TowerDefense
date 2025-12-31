using UnityEngine;
using UnityEngine.Audio;

public class MusicSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _imgSoundOn;
    [SerializeField] private GameObject _imgSoundOff;
    [SerializeField] private AudioMixerGroup _soundMusicGroup;

    public void SwitchOnMusic()
    {
        if (PlayerPrefs.GetInt("MusicOn") == 0)
        {
            PlayerPrefs.SetInt("MusicOn", 1);
            _soundMusicGroup.audioMixer.SetFloat("MusicVolume", 0);

        }
        else if (PlayerPrefs.GetInt("MusicOn") == 1)
        {
            PlayerPrefs.SetInt("MusicOn", 0);
            _soundMusicGroup.audioMixer.SetFloat("MusicVolume", -80);
        }

        InitializeImage();
    }

    public void InitializeMusic()
    {
        if (PlayerPrefs.GetInt("MusicOn") == 0)
        {
            _soundMusicGroup.audioMixer.SetFloat("MusicVolume", -80);
        }
        else if (PlayerPrefs.GetInt("MusicOn") == 1)
        {
            _soundMusicGroup.audioMixer.SetFloat("MusicVolume", 0);
        }
    }

    private void InitializeImage()
    {
        if (PlayerPrefs.GetInt("MusicOn") == 1)
        {
            _imgSoundOff.SetActive(false);
            _imgSoundOn.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("MusicOn") == 0)
        {
            _imgSoundOff.SetActive(true);
            _imgSoundOn.SetActive(false);
        }
    }

    private void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("MusicOn") == 0);
        InitializeImage();
        InitializeMusic();
    }
}
