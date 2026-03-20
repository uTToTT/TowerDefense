using System;
using UnityEngine;

[Serializable]
public class Wave 
{
    [SerializeField] private Group[] _groups;

    public Group[] Groups => _groups;
}
