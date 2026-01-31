using System.Net.Mime;
using UnityEngine;
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
            retryButton.RegisterCallback<ClickEvent>(OnRetryButtonClicked);
            quitButton = uiObject.Q<Button>("Button_Quit");
            quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
        }

        public override void OnHide()
        {
            base.OnHide();
            retryButton?.UnregisterCallback<ClickEvent>(OnRetryButtonClicked);
            quitButton?.UnregisterCallback<ClickEvent>(OnQuitButtonClicked);
        }

        private void OnRetryButtonClicked(ClickEvent evt)
        {
            HidePanel();
            //  Restart level
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            HidePanel();
            Application.Quit();
            //  Load to menu
        }
    }
}