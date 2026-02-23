using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonWrapper : MonoBehaviour
{
    public event Action OnClick;

    [SerializeField] private Button _button;
    [HorizontalLine]
    [SerializeField] private Image _popup;
    [SerializeField] private Image _border;

    [HorizontalLine]
    [Header("Design")]
    [SerializeField] private Color _defaultPopupColor = Color.wheat;
    [SerializeField] private Color _clickedPopupColor = Color.wheat;
    [Space]
    [SerializeField] private Color _defaultBorderColor = Color.wheat;
    [SerializeField] private Color _clickedBorderColor = Color.wheat;

    [HorizontalLine]
    [SerializeField] private float _pressedTime = 0.2f;

    private float _elapsedTime;

    #region Unity API

    private void Reset()
    {
        InitButton();
        ConfigureButton();
    }

    private void Start()
    {
        InitEvents();
        SetDefault();
    }

    private void Update()
    {
        if (_elapsedTime > 0)
        {
            _elapsedTime -= Time.deltaTime;

            if (_elapsedTime <= 0)
                SetDefault();
        }
    }

    #endregion

    private void Click() => OnClick?.Invoke();

    private void SetPressed()
    {
        _popup.color = _clickedPopupColor;
        _border.color = _clickedBorderColor;

        _elapsedTime = _pressedTime;
    }

    private void SetDefault()
    {
        _popup.color = _defaultPopupColor;
        _border.color = _defaultBorderColor;
    }

    private void InitEvents()
    {
        _button.onClick.AddListener(() => Click());

        OnClick += SetPressed;
    }

    private void InitButton()
    {
        _button = GetComponent<Button>();
    }

    private void ConfigureButton()
    {
        _button.transition = Selectable.Transition.None;
    }
}
