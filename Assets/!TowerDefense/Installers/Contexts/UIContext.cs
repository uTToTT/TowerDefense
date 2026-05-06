using TToTT.TowerDefense.UI;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class UIContext : MonoBehaviour
    {
        [SerializeField] public UIWindowsController WindowsController;
        [SerializeField] public WalletView WalletView;
        [SerializeField] public MainMenuController MainMenu;
        [SerializeField] public GameplayInterfaceController Gameplay;
    }
}