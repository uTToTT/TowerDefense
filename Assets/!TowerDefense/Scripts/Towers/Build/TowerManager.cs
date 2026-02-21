using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private List<Tower> _builtTowers = new();

    #region Life cycle

    public void Init()
    {
        
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
