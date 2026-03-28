using System.Collections.Generic;
using TToTT.TowerDefense.Map;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private bool _isMoveActive;
    [SerializeField] private MapManager _mapManager;

    private readonly List<IMovable> _movables = new();
    private readonly List<IMovable> _toAdd = new();
    private readonly List<IMovable> _toRemove = new();

    public static MoveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Register(IMovable movable) => _toAdd.Add(movable);
    public void Unregister(IMovable movable) => _toRemove.Add(movable);

    public void Tick(float dt)
    {
        if (_isMoveActive == false) return;

        UpdateColleciton();
        Move(dt);
    }

    private void Move(float dt)
    {
        int total = _movables.Count;
        if (total <= 0) return;

        foreach (var item in _movables)
        {
            item.Move(dt);
        }
    }

    private void UpdateColleciton()
    {
        if (_toRemove.Count > 0)
        {
            foreach (var item in _toRemove)
            {
                _movables.Remove(item);
            }

            _toRemove.Clear();
        }

        if (_toAdd.Count > 0)
        {
            foreach (var item in _toAdd)
            {
                _movables.Add(item);
            }

            _toAdd.Clear();
        }
    }
}
