using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/SFX/Sound Config")]
public class SoundConfig : ScriptableObject
{
    [SerializeField] private AudioClip[] _clips; 
    [SerializeField, Range(0f, 1f)] private float _volume = 1f;
    [SerializeField, MinMaxSlider(0.5f, 2f)] private Vector2 _pitchRange = new(0.9f, 1.1f);
    [SerializeField] private bool _loop = false;

    public AudioClip GetClip() =>
        _clips[Random.Range(0, _clips.Length)];

    public float Volume => _volume;
    public float RandomPitch => Random.Range(_pitchRange.x, _pitchRange.y);
    public bool Loop => _loop;
}