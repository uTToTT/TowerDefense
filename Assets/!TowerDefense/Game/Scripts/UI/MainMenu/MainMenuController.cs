using System;
using UnityEngine;

namespace TToTT.TowerDefense.UI
{
    public class MainMenuController : InterfaceContorller
    {
        [SerializeField] private ButtonWrapper _playButton;

        public void Init(Action onPlay)
        {
            _playButton.OnClick += onPlay;
        }
    }
}