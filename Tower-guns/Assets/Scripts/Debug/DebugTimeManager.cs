using UnityEngine;

public class DebugTimeManager : MonoBehaviour
{
    [SerializeField] private bool _timeStop;

    private bool _isTimeStoped;

    private void OnValidate()
    {
        if (_timeStop && !_isTimeStoped)
        {
            StopTime();

            _isTimeStoped = true;
        }
        else if (!_timeStop && _isTimeStoped) 
        {
            StartTime();

            _isTimeStoped = false;
        }
    }

    public void StartTime()
    {
        Time.timeScale = 1.0f;
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }
}
