using System;

[Serializable]
public readonly struct ObjectPoolParameters
{
    public int DefaultCapacity { get; }
    public int MaxSize { get; }

    public ObjectPoolParameters(int defaultCapacity, int maxSize)
    {
        if (defaultCapacity < 0)
            throw new ArgumentException($"Value must be greater than zero.", nameof(defaultCapacity));
        if (maxSize < 0)
            throw new ArgumentException($"Value must be greater than zero.", nameof(maxSize));
        if (maxSize < defaultCapacity)
            throw new ArgumentException($"Max size can not be smaller than default capacity.", nameof(maxSize));

        DefaultCapacity = defaultCapacity;
        MaxSize = maxSize;
    }
}
