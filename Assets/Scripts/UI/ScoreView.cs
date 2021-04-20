using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public int Value
        {
            set => _text.text = $"Score: {value.ToString()}";
        }

        private void Awake()
        {
            ViewHolder.AddView(this);
        }
    }
}
