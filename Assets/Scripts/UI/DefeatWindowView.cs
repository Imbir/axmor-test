using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DefeatWindowView : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _bestScoreText;

        public bool IsActive => gameObject.activeSelf;

        private void Awake()
        {
            ViewHolder.AddView(this);
            gameObject.SetActive(false);
        }

        public void Show(int score, int bestScore)
        {
            _scoreText.text = $"Score: {score}";
            _bestScoreText.text = $"Best: {bestScore}";
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
