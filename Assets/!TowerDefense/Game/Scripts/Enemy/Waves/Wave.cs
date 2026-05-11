using System;
using UnityEngine;

namespace TToTT.TowerDefense.Enemies.Wave
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private Group[] _groups;

        public Group[] Groups => _groups;
    }
}