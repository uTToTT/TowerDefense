using UnityEngine;
using UnityEngine.Audio;

public class MenuOutMusic : MonoBehaviour
{
    public AudioMixerSnapshot Normal;

    private void OnEnable()
    {
        Normal.TransitionTo(0.5f);
    }
}
