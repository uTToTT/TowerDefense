using UnityEngine;

[CreateAssetMenu(menuName = "TD/Shop/Shop Config")]
public class ShopConfig : ScriptableObject
{
    [SerializeField] private int _startBalance = 25;

    [SerializeField] private int _startRerollCost = 5;
    [SerializeField] private int _rerollDelta = 2;
    [SerializeField] private ProductConfig[] _products;

    public int StartBalance => _startBalance;
    public int StartRerollCost => _startRerollCost;
    public int RerollDelta => _rerollDelta;
    public ProductConfig[] Products => _products;
}