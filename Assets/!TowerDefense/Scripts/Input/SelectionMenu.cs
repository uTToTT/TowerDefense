using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    public event Action OnClose;
    public event Action<UpgradeNodeConfig> OnUpgrade;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _upgradeButtonsParent;
    [SerializeField] private TowerUpgradeButton _upgradeButtonPrefab;

    private List<TowerUpgradeButton> _buttons = new();

    public void CreateUpgradeButtons(IReadOnlyCollection<UpgradeNodeConfig> upgrades)
    {
        ClearButtons();

        foreach (var upgrade in upgrades)
        {
            var button = Instantiate(_upgradeButtonPrefab, _upgradeButtonsParent);
            button.Setup(upgrade);
            button.OnClicked += HandleUpgradeSelected;

            _buttons.Add(button);
        }
    }

    public void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseMenu);
    }

    private void CloseMenu()
    {
        OnClose?.Invoke();
        Disable();
    }

    private void HandleUpgradeSelected(TowerUpgradeButton button, UpgradeNodeConfig config)
    {
        OnUpgrade?.Invoke(config);
        Debug.Log($"Upgrade {config.name}");
        CreateUpgradeButtons(config.Next);
    }

    private void ClearButtons()
    {
        for (int i = _buttons.Count -1; i >= 0; i--)
        {
            Destroy(_buttons[i].gameObject);
        }

        _buttons.Clear();
    }
}
