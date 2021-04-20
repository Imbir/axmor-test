using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BestScoreView : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public int Value
        {
            set => _text.text = $"Best: {value.ToString()}";
        }

        private void Awake()
        {
            ViewHolder.AddView(this);
        }
    }
}
