using System;
using UnityEngine;

[Serializable]
public class WindowData 
{
    [SerializeField] private GameObject _window;
    [SerializeField] private WindowType _type;

    public GameObject Window => _window;
    public WindowType Type => _type;
}
