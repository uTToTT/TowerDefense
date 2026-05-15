using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, IPoolable, IEntityLifecycle
{
    public event Action<AudioPlayer> OnCompleted;

    private AudioSource _source;

    private void Awake()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _source.playOnAwake = false;
    }

    public void Apply(SoundConfig config)
    {
        _source.clip = config.GetClip();
        _source.volume = config.Volume;
        _source.pitch = config.RandomPitch;
        _source.loop = config.Loop;
    }

    public void Play() => _source.Play();
    public void Stop() => _source.Stop();

    private void Update()
    {
        if (!_source.isPlaying && !_source.loop)
            OnCompleted?.Invoke(this);
    }

    public bool IsActive { get; set; }
    public void OnPreload() { }
    public void OnActivated() { }
    public void OnDeactivated() { }
    public void OnReturned() { _source.Stop(); }
    public void OnDestroyed() { }
    public void Dispose() { }
}