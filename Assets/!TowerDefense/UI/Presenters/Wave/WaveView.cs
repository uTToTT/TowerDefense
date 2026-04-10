using TMPro;
using UnityEngine;

public class WaveView : MonoBehaviour, IWaveView
{
    [SerializeField] private TMP_Text _wave;

    public void SetWave(int curr, int max)
    {
        _wave.SetText($"{curr}/{max}");
    }
}
