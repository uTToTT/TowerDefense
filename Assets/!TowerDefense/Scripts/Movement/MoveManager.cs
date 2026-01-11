using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private bool _isMoveActive;
    //[SerializeField] private TMP_Text _counter;
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

    public void Tick()
    {
        if (_isMoveActive == false) return;

        UpdateColleciton();
        Move();
    }

    private void Move()
    {
        int total = _movables.Count;
        if (total <= 0) return;

        foreach (var item in _movables)
        {
            item.Move();
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

            //_counter.text = $"Movables: {_movables.Count}";
            _toRemove.Clear();
        }

        if (_toAdd.Count > 0)
        {
            foreach (var item in _toAdd)
            {
                _movables.Add(item);
            }

            //_counter.text = $"Movables: {_movables.Count}";
            _toAdd.Clear();
        }
    }
}
