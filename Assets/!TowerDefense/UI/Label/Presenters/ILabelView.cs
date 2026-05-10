using System;

namespace TToTT.TowerDefense.UI.Label
{
    public interface ILabelView
    {
        void SetText(string text);
        void SetText<T>(T value, string format = null) where T : IFormattable
             => SetText(format != null
                 ? value.ToString(format, null)
                 : value.ToString());
    }
}