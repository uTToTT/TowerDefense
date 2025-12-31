using UnityEngine;
using UnityEngine.Audio;

public class MenuMusic : MonoBehaviour
{
    public AudioMixerSnapshot Normal;
    public AudioMixerSnapshot InMenu;

    private void OnEnable()
    {
        InMenu.TransitionTo(0.25f);
    }

    private void OnDisable()
    {
        Normal.TransitionTo(0.5f);
    }
}
