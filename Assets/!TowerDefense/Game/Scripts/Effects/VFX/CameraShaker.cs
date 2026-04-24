using UnityEngine;
using DG.Tweening;

public class CameraShaker
{
    private float _duration = 0.3f;
    private float _strength = 0.5f;
    private int _vibrato = 20;
    private float _randomness = 90f;
    private bool _fadeOut = true;

    private Tween _currentTween;
    private Vector3 _initialLocalPos;
    private readonly Camera _camera;

    public CameraShaker(Camera camera)
    {
        _camera = camera;
        _initialLocalPos = _camera.transform.localPosition;
    }

    public void Shake()
    {
        _currentTween?.Kill(false);

        _camera.transform.localPosition = _initialLocalPos;

        _currentTween = _camera.transform
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
                _camera.transform.localPosition = _initialLocalPos;
            });
    }
}