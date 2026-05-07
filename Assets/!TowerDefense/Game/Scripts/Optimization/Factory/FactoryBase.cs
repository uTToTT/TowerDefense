using NaughtyAttributes;
using System;
using UnityEngine;

public abstract class FactoryBase<T> : ScriptableObject,
    IDisposable where T : MonoBehaviour, IPoolable, IEntityLifecycle
{
    [SerializeField] private T _prefab;
    [Space]
    [SerializeField] private int _capacity = 10;
    [SerializeField] private int _maxCount = 100;

    [Header("Info")]
    [SerializeField, ReadOnly] public int _totalPreload;
    [SerializeField, ReadOnly] public int _totalGet;
    [SerializeField, ReadOnly] public int _activeCount;
    [SerializeField, ReadOnly] public int _poolCount;
    [SerializeField, ReadOnly] public int _totalCount;

    private ObjectPool<T> _pool;

    protected T Prefab => _prefab;

    private void OnValidate()
    {
        if (_pool == null) return;

        _totalPreload = _pool.TotalPreload;
        _totalGet = _pool.TotalGet;
        _activeCount = _pool.ActiveCount;
        _poolCount = _pool.PoolCount;
        _totalCount = _pool.TotalCount;
    }

    public void Init()
    {
        if (_pool != null) return;

        var poolParam = new ObjectPoolParameters(_capacity, _maxCount);
        _pool = new ObjectPool<T>(poolParam, OnPreload, OnGet, OnReturn);
    }

    public T Create()
    {
        return _pool.Get();
    }

    public void Return(T item)
    {
        _pool.Return(item);
    }

    public void ReturnAll()
    {
        _pool.ReturnAll();
    }

    public void Dispose()
    {
        foreach (var entity in _pool.AllObjects)
            entity.OnDestroyed();

        _pool?.Dispose();
    }

    #region ==== Pool handlers ====

    protected virtual T OnPreload()
    {
        var entity = Instantiate(_prefab);
        entity.OnPreload();
        return entity;
    }

    protected virtual void OnGet(T entity)
    {
        entity.gameObject.SetActive(true);
        entity.OnActivated();
    }

    protected virtual void OnReturn(T entity)
    {
        entity.gameObject.SetActive(false);
        entity.OnDeactivated();
        entity.OnReturned();
    }

    #endregion
}
