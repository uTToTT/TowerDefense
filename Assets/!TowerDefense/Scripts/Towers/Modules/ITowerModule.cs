public interface ITowerModule
{
    void Tick(float deltaTime);
    bool TryApplyConfig(TowerModuleConfig config);
}


