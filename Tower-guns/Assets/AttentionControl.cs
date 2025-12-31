using UnityEngine;

public class AttentionControl : MonoBehaviour
{
    [SerializeField] private GameObject _frameAttention;

    private float _tmpTimeScale;

    private void OpenFrameAttention()
    {
        _frameAttention.SetActive(true);

        _tmpTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void CloseFrameAttention()
    {
        _frameAttention.SetActive(false);

        Time.timeScale = _tmpTimeScale;
    }

    private void CheckWave(int wave)
    {
        if (wave == 30)
        {
            OpenFrameAttention();
        }
    }

    private void OnEnable()
    {
        EventBus.OnWaveStart += CheckWave;
    }
}
