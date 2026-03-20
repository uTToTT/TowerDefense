using TMPro;
using UnityEngine;

public class StarBonusesInfo : MonoBehaviour
{
    [SerializeField] private GameObject _lockFrame;
    [SerializeField] private GameObject _buffFrame;
    [SerializeField] private int _starReq;
    [Space]
    [SerializeField] private string _buffType;
    [Space]
    [SerializeField] private TextMeshProUGUI _textStarReq;

    public GameObject LockFrame => _lockFrame;
    public GameObject BuffFrame => _buffFrame;
    public int StarReq => _starReq;
    public string BuffType => _buffType;

    private void OnValidate()
    {
        _textStarReq.text = _starReq.ToString();
    }

    public void Lock()
    {
        _lockFrame.SetActive(true);
        _buffFrame.SetActive(false);
    }

    public void Unlock()
    {
        _lockFrame.SetActive(false);
        _buffFrame.SetActive(true);
    }
}
