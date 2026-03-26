using System.Collections.Generic;

public class TowerManager 
{
    private List<Tower> _builtTowers = new();

    public List<Tower> Towers => _builtTowers;

    #region Life cycle

    public void Restart()
    {
        foreach (var tower in _builtTowers)
        {
            MapManager.Instance.RemoveMapObject(tower);
            GameLoop.Instance.BuildManager.Return(tower);
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
