using System;
using TMPro;
using UnityEngine;

public class TextLang : MonoBehaviour
{
    [SerializeField] private int _language = 0;
    [SerializeField] private string[] _text;
    [SerializeField] private bool _updateInEditor;
    private TextMeshProUGUI _textLine;

    private void OnValidate()
    {
        if (_updateInEditor && _text != null)
        {
            if (_language >= 0 && _language < _text.Length)
            {
                _textLine = GetComponent<TextMeshProUGUI>();

                if (_text[_language] != "")
                {
                    _textLine.text = _text[_language].Replace("\\n", Environment.NewLine);
                }
            }
        }
    }

    void Start()
    {
        UpdateCurrLang();
    }

    private void UpdateCurrLang()
    {
        _language = PlayerPrefs.GetInt("language", _language);
        _textLine = GetComponent<TextMeshProUGUI>();
        _textLine.text = _text[_language].Replace("\\n", Environment.NewLine); // null ref
    }

    private void OnEnable()
    {
        EventBus.onLangChanged += UpdateCurrLang;
        UpdateCurrLang();
    }

    private void OnDisable()
    {
        EventBus.onLangChanged -= UpdateCurrLang;
    }
}
