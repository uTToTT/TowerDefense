using UnityEngine;

[System.Serializable]
public class FrameData 
{
    [SerializeField] private GameObject _frame;
    [SerializeField] private FrameType _type;

    public GameObject Frame => _frame;
    public FrameType Type => _type;
}
