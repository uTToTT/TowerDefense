using TToTT.TowerDefense.UI;
using TToTT.TowerDefense.UI.Label;
using UnityEngine;

namespace TToTT.TowerDefense.Installers
{
    public class UIContext : MonoBehaviour
    {
        [SerializeField] public UIWindowsController WindowsController;
        [SerializeField] public MainMenuController MainMenu;
        [SerializeField] public GameplayInterfaceController Gameplay;
        [SerializeField] public LabelRegistry Labels;
    }
}