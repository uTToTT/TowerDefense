using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class InterfaceContorller : MonoBehaviour 
{
    [SerializeField] private List<FrameData> _frameDatas;

    public void OpenFrame(FrameType type)
    {
        FindFrameData(type).Frame.SetActive(true);
    }

    public void CloseFrame(FrameType type)
    {
        FindFrameData(type).Frame.SetActive(false);
    }

    public void CloseAll()
    {
        foreach (var data in _frameDatas)
            data.Frame.SetActive(false);
    }

    private FrameData FindFrameData(FrameType type) =>
        _frameDatas.FirstOrDefault(w => w.Type == type);
}
