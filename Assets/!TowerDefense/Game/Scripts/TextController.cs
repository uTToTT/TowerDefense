using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textPlaceFirstTower;

    private void Start()
    {
        _textPlaceFirstTower.gameObject.SetActive(true);
    }

    private void DisableText(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(false);
    }

    private void DisablePlacTowerString()
    {
        DisableText(_textPlaceFirstTower);
    }

    private void OnEnable()
    {
        EventBus.FirstTowerWasBuilt += DisablePlacTowerString;
    }

    private void OnDisable()
    {
        EventBus.FirstTowerWasBuilt -= DisablePlacTowerString;
    }
}
