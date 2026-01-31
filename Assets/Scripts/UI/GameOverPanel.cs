using UnityEngine.UIElements;

namespace UI
{
    public class GameOverPanel : UiPanel
    {
        public Button retryButton;
        public Button quitButton;

        public override void OnShow()
        {
            base.OnShow();
            retryButton = uiObject.Q<Button>("Button_PlayAgain");
            retryButton.clickable.clicked += OnRetryButtonClicked;  
            quitButton = uiObject.Q<Button>("Button_Quit");
            quitButton.clickable.clicked += OnQuitButtonClicked;
        }

        public override void OnHide()
        {
            base.OnHide();
            if (retryButton != null) retryButton.clickable.clicked -= OnRetryButtonClicked;  
            if (quitButton != null) quitButton.clickable.clicked -= OnQuitButtonClicked;
        }

        private void OnRetryButtonClicked()
        {
            HidePanel();
            //  Restart level
        }

        private void OnQuitButtonClicked()
        {
            HidePanel();
            //  Load to menu
        }
    }
}