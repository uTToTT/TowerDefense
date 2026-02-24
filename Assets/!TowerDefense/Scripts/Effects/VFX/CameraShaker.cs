using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private float _strength = 0.5f;
    [SerializeField] private int _vibrato = 20;
    [SerializeField] private float _randomness = 90f;
    [SerializeField] private bool _fadeOut = true;

    private Tween _currentTween;
    private Vector3 _initialLocalPos;

    public void Init()
    {
        _initialLocalPos = transform.localPosition;
    }

    public void Shake()
    {
        _currentTween?.Kill(false);

        transform.localPosition = _initialLocalPos;

        _currentTween = transform
            .DOShakePosition(
                _duration,
                _strength,
                _vibrato,
                _randomness,
                false,
                _fadeOut)
            .SetUpdate(true) 
            .OnKill(() =>
            {
                transform.localPosition = _initialLocalPos;
            });
    }
}