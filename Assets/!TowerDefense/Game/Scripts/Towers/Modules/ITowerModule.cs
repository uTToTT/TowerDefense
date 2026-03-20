public interface ITowerModule
{
    ModuleType ModuleType { get; }
    void Tick(float deltaTime);
    bool TryApplyConfig(TowerModuleConfig config);
}


