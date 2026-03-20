using UnityEngine;

[CreateAssetMenu(fileName = "ProductConfig", menuName = "TD/Shop/Product")]
public class ProductConfig : ScriptableObject
{
    [SerializeField] private MapObjectType _productType;
    [SerializeField, Range(1, 100)] private int _cost;
    [SerializeField, Range(0, 100)] private float _weight;

    public MapObjectType ProductType => _productType;
    public int Cost => _cost;
    public float Weight => _weight;
}
