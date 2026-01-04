using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ObjectPool<T> : IDisposable
    where T : IPoolable
{
    private readonly Queue<T> _pool = new();
    private readonly HashSet<T> _active = new();
    private readonly List<T> _allObjects = new();

    private readonly ObjectPoolParameters _parameters;

    private readonly Func<T> _preload;
    private readonly Action<T> _get;
    private readonly Action<T> _return;

    private readonly bool _lazyLoad;

    public int TotalPreload { get; private set; }
    public int TotalGet { get; private set; }
    public int ActiveCount => _active.Count;
    public int PoolCount => _pool.Count;
    public int TotalCount => _active.Count + _pool.Count;

    public List<T> AllObjects => _allObjects;

    public ObjectPool(
        ObjectPoolParameters parameters,
        Func<T> preloadAction,
        Action<T> getAction,
        Action<T> returnAction,
        bool lazyLoad = true)
    {
        if (preloadAction == null)
            throw new ArgumentNullException(nameof(preloadAction));
        if (getAction == null)
            throw new ArgumentNullException(nameof(getAction));
        if (returnAction == null)
            throw new ArgumentNullException(nameof(returnAction));

        _parameters = parameters;

        _preload = preloadAction;
        _get = getAction;
        _return = returnAction;

        _lazyLoad = lazyLoad;

        if (lazyLoad == false)
            InitObjects();
    }

    public T Get()
    {
        T item;

        if (_pool.Count > 0)
        {
            item = _pool.Dequeue();
        }
        else
        {
            try
            {
                item = _preload();
                _allObjects.Add(item);
                TotalPreload++;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during Preload() action: {ex}");
                return default;
            }
        }

        try
        {
            _get(item);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during Get() action: {ex}");
            return default;
        }

        _active.Add(item);

        item.IsActive = true;

        TotalGet++;

        return item;
    }

    public void Return(T item)
    {
        if (item == null) return;

        try
        {
            _return(item);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during Return() action: {ex}");
            return;
        }

        _pool.Enqueue(item);
        _active.Remove(item);

        item.IsActive = false;

        CheckPoolCapacity();
    }

    public void ReturnAll()
    {
        var toReturn = _active.ToArray();

        foreach (var item in toReturn)
        {
            Return(item);
        }
    }

    public void Dispose()
    {
        ReturnAll();

        foreach (var item in _pool)
        {
            item.Dispose();
        }

        _pool.Clear();
        _active.Clear();
        _allObjects.Clear();
    }

    private void CheckPoolCapacity()
    {
        if (_pool.Count > _parameters.MaxSize)
        {
            int countToDispose = _pool.Count - _parameters.MaxSize;

            for (int i = 0; i < countToDispose; i++)
            {
                var item = _pool.Dequeue();
                item.Dispose();
            }
        }
    }

    private void InitObjects()
    {
        for (int i = 0; i < _parameters.DefaultCapacity; i++)
        {
            var item = _preload();
            item.IsActive = false;
            _return(item);
            _pool.Enqueue(item);
            _allObjects.Add(item);
        }
    }
}
