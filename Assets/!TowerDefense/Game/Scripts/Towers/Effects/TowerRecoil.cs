using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public sealed class TowerRecoil : MonoBehaviour
{
    [SerializeField] private float _recoilDistance = 0.15f;
    [SerializeField] private float _recoilDurationOut = 0.08f;
    [SerializeField] private float _recoilDurationBack = 0.08f;
    [SerializeField] private Ease _easeOut = Ease.OutQuad;
    [SerializeField] private Ease _easeBack = Ease.OutQuad;
    [SerializeField] private Transform _towerTransform;

    private Vector3 _baseLocalPosition;
    private Tween _recoilTween;

    private void Awake()
    {
        _baseLocalPosition = _towerTransform.localPosition;
    }

    [Button("Test Recoil")]
    private void TestRecoil()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Recoil works only in Play Mode");
            return;
        }

        // Инициализируем если Awake ещё не вызывался
        if (_baseLocalPosition == Vector3.zero)
            _baseLocalPosition = _towerTransform.localPosition;

        PlayRecoil();
    }

    public void PlayRecoil()
    {
        _recoilTween?.Kill(false);
        _towerTransform.localPosition = _baseLocalPosition;
        Vector3 recoilDirection = -_towerTransform.up;
        Vector3 recoilOffset = recoilDirection * _recoilDistance;
        _recoilTween = _towerTransform
            .DOLocalMove(_baseLocalPosition + recoilOffset, _recoilDurationOut)
            .SetEase(_easeOut)
            .OnComplete(() =>
            {
                _towerTransform
                    .DOLocalMove(_baseLocalPosition, _recoilDurationBack)
                    .SetEase(_easeBack);
            });
    }
}