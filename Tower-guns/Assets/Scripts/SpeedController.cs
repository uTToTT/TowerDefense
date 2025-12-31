using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Button _decreaseButton;
    [SerializeField] private Button _increaseButton;
    [SerializeField] private TextMeshProUGUI _textSpeed;

    private int _speedIndex;

    private void Start()
    {
        _speedIndex = 1;
        SetSpeed();
    }

    public void IncreaseSpeed()
    {
        if (_speedIndex < 2)
        {
            _speedIndex++;
        }

        CheckInteractableButtons();
        SetSpeed();
    }

    public void DecreaseSpeed()
    {
        if (_speedIndex > 0)
        {
            _speedIndex--;
        }

        CheckInteractableButtons();
        SetSpeed();
    }

    private void CheckInteractableButtons()
    {
        //_increaseButton.interactable = true;
        //_decreaseButton.interactable = true;

        if (_speedIndex == 2)
        {
            _increaseButton.interactable = false;
        }
        else
        {
            _increaseButton.interactable = true;
        }

        if (_speedIndex == 0)
        {
            _decreaseButton.interactable = false;
        }
        else
        {
            _decreaseButton.interactable = true;
        }
    }

    public void SetLowSpeed()
    {
        //Debug.Log("Low speed");
        Time.timeScale = 0.5f;
        _textSpeed.text = "0.5x";
        _speedIndex = 0;

        CheckInteractableButtons();
    }

    public void SetSpeed()
    {
        if (_speedIndex == 0)
        {
            Time.timeScale = 0.5f;
            _textSpeed.text = "0.5x";
        }
        else if (_speedIndex == 1)
        {
            Time.timeScale = 1f;
            _textSpeed.text = "1.0x";
        }
        else if (_speedIndex == 2)
        {
            Time.timeScale = 2f;
            _textSpeed.text = "2.0x";
        }
        else
        {
            Time.timeScale = 0.5f;
            _textSpeed.text = "0.5x";
            _speedIndex = 0;
        }
    }
}
