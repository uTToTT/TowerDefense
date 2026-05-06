using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class ShopContext : MonoBehaviour
    {
        [SerializeField] public ShopConfig Config;
        [SerializeField] public ProductSlot[] Slots;
        [SerializeField] public ButtonWrapper RerollButton;
    }
}