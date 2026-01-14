using UnityEngine;

public sealed class FreezeModule : ITowerModule, IOnHitEffect
{
    private readonly FreezeModuleConfig _config;

    public FreezeModule(FreezeModuleConfig config)
    {
        _config = config;
    }

    public bool TryApplyConfig(TowerModuleConfig config)
        => config is FreezeModuleConfig;

    public void Tick(float dt) { }

    public void OnHit(HitContext hit)
    {
        throw new System.NotImplementedException();
        //enemy.ApplySlow(
        //    _config.SlowMultiplier,
        //    _config.Duration);
    }
}
