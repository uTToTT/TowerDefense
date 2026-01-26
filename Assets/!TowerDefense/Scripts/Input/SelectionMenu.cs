using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    public event Action OnClose;
    public event Action OnUpgrade;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _upgradeButton;

    public void HighlightUpgrade() => _upgradeButton.interactable = true;
    public void DisableUpgrade() => _upgradeButton.interactable = false;

    public void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseMenu);
        _upgradeButton.onClick.AddListener(Upgrade);
    }

    private void CloseMenu()
    {
        OnClose?.Invoke();
        Disable();
    }

    private void Upgrade()
    {
        OnUpgrade?.Invoke();
    }
}
