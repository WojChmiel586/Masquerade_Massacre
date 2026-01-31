using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MenuPanel : UiPanel
    {
        public Button startButton;

        public override void OnShow()
        {
            base.OnShow();
            startButton = uiObject.Q<Button>("Button_PlayAgain");
            startButton.clickable.clicked += OnRetryButtonClicked;  
        }

        public override void OnHide()
        {
            base.OnHide();
            if (startButton != null) startButton.clickable.clicked -= OnRetryButtonClicked;  
        }

        private void OnRetryButtonClicked()
        {
            HidePanel();
            GameController.Instance.CurrentGameState = GameState.Playing;
        }

    }
}