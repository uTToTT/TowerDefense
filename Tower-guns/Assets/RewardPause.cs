using UnityEngine;

public class RewardPause : MonoBehaviour
{
    [SerializeField] private GameObject _panelPause;

    private void Awake()
    {
        EventBus.onRewardPause += SetRewardPause;
    }

    public void SetRewardPause()
    {
        Debug.Log("Pause");
        if (_panelPause != null)
        {
            _panelPause.SetActive(true);
        }
        Time.timeScale = 0;
    }
}
