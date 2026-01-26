using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;

    public event Action<TowerUpgradeButton, UpgradeNodeConfig> OnClicked;

    private UpgradeNodeConfig _upgrade;

    public void Setup(UpgradeNodeConfig upgrade)
    {
        _upgrade = upgrade;
        _image.sprite = upgrade.Icon;
    }

    public void SetHighlighted(bool value)
    {
        // визуальный highlight
    }

    private void OnEnable() => _button.onClick.AddListener(() => Click());
    private void OnDisable() => _button?.onClick.RemoveAllListeners();

    public void Click()
    {
        OnClicked?.Invoke(this, _upgrade);
        Debug.Log("Click");
    }
}
