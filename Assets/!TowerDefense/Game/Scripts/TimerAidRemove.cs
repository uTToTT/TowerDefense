using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerAidRemove : MonoBehaviour
{
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private float _delay;
    [SerializeField] private Image _barTime;

    private float _currTimeToRemove;

    private IEnumerator TimerToRemove()
    {
        while (_currTimeToRemove > 0)
        {
            _currTimeToRemove -= Time.deltaTime;
            _barTime.fillAmount = _currTimeToRemove / _delay;

            yield return null;
        }

        DisableButton();
    }

    private void DisableButton()
    {
        _buttonContinue.gameObject.SetActive(false);
    }

    private void StartTimer()
    {
        _barTime.fillAmount = 1;
        _currTimeToRemove = _delay;

        StartCoroutine(TimerToRemove());
    }

    private void OnEnable()
    {
        StartTimer();
    }
}
