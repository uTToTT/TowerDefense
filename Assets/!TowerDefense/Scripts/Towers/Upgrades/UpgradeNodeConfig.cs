using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Tower/Upgrade Node")]
public class UpgradeNodeConfig : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _cost;
    [HorizontalLine]

    [SerializeField] private UpgradeNodeConfig[] _next;
    [SerializeField] private UpgradeCondition[] _conditions;
    [SerializeField] private TowerModuleConfig[] _addModuleConfigs;
    [SerializeField] private TowerModuleConfig[] _modifyModuleConfigs;

    public string Id => _id;
    public Sprite Icon => _icon;
    public int Cost => _cost;

    public IReadOnlyCollection<UpgradeNodeConfig> Next => _next;
    public IReadOnlyCollection<UpgradeCondition> Conditions => _conditions;
    public IReadOnlyCollection<TowerModuleConfig> AddModuleConfigs => _addModuleConfigs;
    public IReadOnlyCollection<TowerModuleConfig> ModifyModuleConfigs => _modifyModuleConfigs;
}
