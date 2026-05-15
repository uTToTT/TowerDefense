using System;
using UnityEngine;

public class AudioService : IDisposable
{
    private readonly SoundRegistry _registry;
    private readonly AudioPlayerFactory _factory;

    public AudioService(SoundRegistry registry, AudioPlayerFactory factory)
    {
        _registry = registry;
        _factory = factory;
        _factory.Init();
        _registry.Init();
    }

    public void Play(SoundId id, Vector2 pos = default)
    {
        if (!_registry.TryGet(id, out var config)) return;

        var player = _factory.Create();
        player.transform.position = pos;
        player.Apply(config);
        player.OnCompleted += OnPlayerCompleted;
        player.Play();
    }

    private void OnPlayerCompleted(AudioPlayer player)
    {
        player.OnCompleted -= OnPlayerCompleted;
        _factory.Return(player);
    }

    public void Restart() => _factory.ReturnAll();
    public void Dispose() => _factory.ReturnAll();
}