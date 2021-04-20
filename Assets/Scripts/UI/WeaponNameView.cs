using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponNameView : MonoBehaviour
    {
        [SerializeField] private Text _text;
        
        public string Value
        {
            set => _text.text = value;
        }
        
        private void Awake()
        {
            ViewHolder.AddView(this);
        }
    }
}
