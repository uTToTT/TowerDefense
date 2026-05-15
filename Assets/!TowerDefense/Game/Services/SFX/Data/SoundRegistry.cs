using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/SFX/Sound Registry")]
public class SoundRegistry : ScriptableObject
{
    [SerializeField] private SoundEntry[] _entries;

    [Serializable]
    private class SoundEntry
    {
        public SoundId Id;
        public SoundConfig Config;
    }

    private Dictionary<SoundId, SoundConfig> _map;

    public void Init()
    {
        _map = _entries.ToDictionary(e => e.Id, e => e.Config);
    }

    public bool TryGet(SoundId id, out SoundConfig config) =>
        _map.TryGetValue(id, out config);
}