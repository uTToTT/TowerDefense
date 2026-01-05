using System;
using UnityEngine;

public static class EventBus
{
    public static Action<int> AddMoney;
    public static Action<int> TakeMoney;
    public static Action<int, int> GetStar;
    public static Action<int> HitBase;
    public static Action GameOver;
    public static Action GameWin;
    public static Action FirstTowerWasBuilt;
    public static Action onCellSelected;
    public static Action<Enemy> onShowEnemyInfo;
    public static Action onNextSceneLoad;
    public static Action onRewardPause;
    public static Action onAid;
    public static Action onPanelLoseDisable;
    public static Action onRemoveAds;
    public static Action OnSwitchOnSoundMusic;
    public static Action OnMusicInstantinate;
    public static Action<int> OnWaveStart;
    public static Action onLangChanged;
}
