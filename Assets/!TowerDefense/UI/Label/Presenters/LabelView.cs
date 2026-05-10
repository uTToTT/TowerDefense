using TMPro;
using UnityEngine;

namespace TToTT.TowerDefense.UI.Label
{
    [RequireComponent(typeof(TMP_Text))]
    public class LabelView : MonoBehaviour, ILabelView
    {
        [SerializeField] private TMP_Text _text;

        #region Init

        private void Reset()
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();
        }

        #endregion

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}