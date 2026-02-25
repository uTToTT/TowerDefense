using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private List<Tower> _builtTowers = new();

    public List<Tower> Towers => _builtTowers;

    #region Life cycle

    public void Init()
    {
        
    }

    public void Restart()
    {
        foreach (var tower in _builtTowers)
        {
            MapManager.Instance.RemoveMapObject(tower);
            GameManager.Instance.BuildManager.Return(tower);
        }

        _builtTowers.Clear();
    }

    public void Tick(float dt)
    {
        foreach (var tower in _builtTowers)
            tower.Tick(dt);
    }

    #endregion

    #region Registering

    public void Register(Tower tower) => _builtTowers.Add(tower); 
    public void Unregister(Tower tower) => _builtTowers.Remove(tower); 

    #endregion
}
