using UnityEngine;
using DG.Tweening;

public sealed class TowerRecoil : MonoBehaviour
{
    [SerializeField] private float _recoilDistance = 0.15f;
    [SerializeField] private float _recoilDuration = 0.08f;
    [SerializeField] private Ease _easeOut = Ease.OutQuad;
    [SerializeField] private Ease _easeBack = Ease.OutQuad;
    [SerializeField] private Transform _towerTransform;

    private Vector3 _baseLocalPosition;
    private Tween _recoilTween;

    private void Awake()
    {
        _baseLocalPosition = _towerTransform.localPosition;
    }

    public void PlayRecoil()
    {
        _recoilTween?.Kill(false);

        _towerTransform.localPosition = _baseLocalPosition;

        Vector3 recoilOffset = -Vector3.up * _recoilDistance;

        _recoilTween = _towerTransform
            .DOLocalMove(_baseLocalPosition + recoilOffset, _recoilDuration)
            .SetEase(_easeOut)
            .OnComplete(() =>
            {
                _towerTransform
                    .DOLocalMove(_baseLocalPosition, _recoilDuration)
                    .SetEase(_easeBack);
            });
    }
}