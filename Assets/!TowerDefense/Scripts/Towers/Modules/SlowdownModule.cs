using System;

public class SlowdownModule : ITowerModule
{
    private SlowdownModuleConfig _config;
    private TargetingModule _targetingModule;
    private Tower _tower;

    public ModuleType ModuleType => ModuleType.Slowdown;

    public SlowdownModule(SlowdownModuleConfig config, Tower tower)
    {
        _config = config;
        _tower = tower;

        _targetingModule = _tower.TargetingModule;
    }

    public void Tick(float deltaTime)
    {
        SlowdownTargets();
    }

    public bool TryApplyConfig(TowerModuleConfig config)
    {
        if (config is not SlowdownModuleConfig slowdownConfig)
            return false;

        _config = slowdownConfig;

        return true;
    }

    private void SlowdownTargets()
    {
        for (int i = _targetingModule.Targets.Count - 1; i >= 0; i--)
        {
            var enemy = _targetingModule.Targets[i];
            var buffController = enemy.BuffController;
            Buff buffOnEnemy;
            Buff newBuff = GetBuff();

            if (buffController.TryGet(Tags.GRAVITY, Characteristics.SPEED, out buffOnEnemy))
            {
                newBuff.Value = MathF.Max(newBuff.Value, buffOnEnemy.Value);
            }

            enemy.BuffController.AddOrReplace(newBuff);
        }
    }

    private Buff GetBuff()
    {
        var buff = new Buff
        {
            ID = Tags.GRAVITY,
            Type = BuffType.Percent,
            Characteristic = Characteristics.SPEED,
            Value = _config.Force * -1f,
            Duration = _config.Duration,
            TimeLeft = _config.Duration
        };

        return buff;
    }
}
