using UnityEngine;
using DG.Tweening;

public sealed class TowerRecoil : MonoBehaviour
{
    [SerializeField] private float _recoilDistance = 0.15f;
    [SerializeField] private float _recoilDuration = 0.08f;
    [SerializeField] private Ease _easeOut = Ease.OutQuad;
    [SerializeField] private Ease _easeBack = Ease.OutQuad;

    private Vector3 _baseLocalPosition;
    private Tween _recoilTween;

    private void Awake()
    {
        _baseLocalPosition = transform.localPosition;
    }

    public void PlayRecoil()
    {
        _recoilTween?.Kill(false);

        transform.localPosition = _baseLocalPosition;

        Vector3 recoilOffset = -Vector3.up * _recoilDistance;

        _recoilTween = transform
            .DOLocalMove(_baseLocalPosition + recoilOffset, _recoilDuration)
            .SetEase(_easeOut)
            .OnComplete(() =>
            {
                transform
                    .DOLocalMove(_baseLocalPosition, _recoilDuration)
                    .SetEase(_easeBack);
            });
    }
}