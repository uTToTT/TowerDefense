using System;

namespace TToTT.TowerDefense.UI.Button
{
    public interface IUIButton
    {
        event Action OnClick;
        event Action OnClickImmidiately;
    }
}