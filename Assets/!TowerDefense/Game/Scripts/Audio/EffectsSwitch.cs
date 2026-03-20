using UnityEngine;
using UnityEngine.Audio;

public class EffectsSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _imgSoundOn;
    [SerializeField] private GameObject _imgSoundOff;
    [SerializeField] private AudioMixerGroup _soundEffectsGroup;

    public void SwitchOnSoundEffects()
    {
        if (PlayerPrefs.GetInt("SoundEffectsOn") == 0)
        {
            PlayerPrefs.SetInt("SoundEffectsOn", 1);
            _soundEffectsGroup.audioMixer.SetFloat("EffectsVolume", 0);
        }
        else if (PlayerPrefs.GetInt("SoundEffectsOn") == 1)
        {
            PlayerPrefs.SetInt("SoundEffectsOn", 0);
            _soundEffectsGroup.audioMixer.SetFloat("EffectsVolume", -80);
        }

        InitializeImage();
    }

    public void InitializeMusic()
    {
        if (PlayerPrefs.GetInt("SoundEffectsOn") == 0)
        {
            _soundEffectsGroup.audioMixer.SetFloat("EffectsVolume", -80);
        }
        else if (PlayerPrefs.GetInt("SoundEffectsOn") == 1)
        {
            _soundEffectsGroup.audioMixer.SetFloat("EffectsVolume", 0);
        }
    }

    private void InitializeImage()
    {
        if (PlayerPrefs.GetInt("SoundEffectsOn") == 1)
        {
            _imgSoundOff.SetActive(false);
            _imgSoundOn.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("SoundEffectsOn") == 0)
        {
            _imgSoundOff.SetActive(true);
            _imgSoundOn.SetActive(false);
        }
    }
    
    void Start()
    {
        if (PlayerPrefs.GetInt("SoundEffectsOn") != 1 && PlayerPrefs.GetInt("SoundEffectsOn") != 0)
        {
            PlayerPrefs.SetInt("SoundEffectsOn", 1);
        }

        InitializeImage();
        InitializeMusic();
    }
}
