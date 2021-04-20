using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class RestartButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public Button.ButtonClickedEvent ClickEvent => _button.onClick;

        private void Awake()
        {
            ViewHolder.AddView(this);
        }
    }
}
