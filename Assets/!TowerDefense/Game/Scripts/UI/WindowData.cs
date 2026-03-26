using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WindowData 
{
    [SerializeField] private WindowType _type;
    [SerializeField] private List<GameObject> _elements;

    public WindowType Type => _type;
    public IReadOnlyCollection<GameObject> Elements => _elements;
}
