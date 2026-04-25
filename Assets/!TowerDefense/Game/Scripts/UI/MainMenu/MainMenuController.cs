using System;
using TToTT.TowerDefense.UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : InterfaceContorller
{
    [SerializeField] private ButtonWrapper _playButton;

    #region Init

    public void Init()
    {
        InitButtons();
    }

    #endregion

    private void InitButtons()
    {
        throw new NotImplementedException();
        _playButton.OnClick += () =>
        {
            //UIFlowController.Instance.OpenPreparing();
            //GameController.Instance.RestartGame();
        };
    }
}
