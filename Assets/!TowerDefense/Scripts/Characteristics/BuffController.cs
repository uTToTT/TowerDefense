using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    private readonly Dictionary<string, Dictionary<string, Buff>> _buffs = new();

    public void Update(float deltaTime)
    {
        foreach (var characteristicPair in _buffs)
        {
            var buffMap = characteristicPair.Value;
            var expired = ListPool<string>.Get();

            foreach (var pair in buffMap)
            {
                Buff buff = pair.Value;
                buff.Tick(deltaTime);

                if (buff.IsExpired)
                    expired.Add(pair.Key);
            }

            foreach (var id in expired)
                buffMap.Remove(id);

            ListPool<string>.Release(expired);
        }
    }

    public void AddOrReplace(Buff buff)
    {
        if (!_buffs.TryGetValue(buff.Characteristic, out var buffMap))
        {
            buffMap = new Dictionary<string, Buff>();
            _buffs.Add(buff.Characteristic, buffMap);
        }

        buffMap[buff.ID] = buff;
    }

    public bool TryGet(string id, string characteristic, out Buff buff)
    {
        buff = default;

        return _buffs.TryGetValue(characteristic, out var map)
               && map.TryGetValue(id, out buff);
    }

    public bool Remove(string id, string characteristic)
    {
        return _buffs.TryGetValue(characteristic, out var map)
               && map.Remove(id);
    }

    public void Clear(string characteristic)
    {
        _buffs.Remove(characteristic);
    }

    public void ClearAll()
    {
        _buffs.Clear();
    }

    public float Calculate(string characteristic, float baseValue)
    {
        if (!_buffs.TryGetValue(characteristic, out var map))
            return baseValue;

        float flatAdd = 0f;
        float percentAdd = 0f;

        foreach (var buff in map.Values)
        {
            switch (buff.Type)
            {
                case BuffType.Flat:
                    flatAdd += buff.Value;
                    break;

                case BuffType.Percent:
                    percentAdd += buff.Value; 
                    break;
            }
        }

        return (baseValue + flatAdd) * (1f + percentAdd);
    }
}
