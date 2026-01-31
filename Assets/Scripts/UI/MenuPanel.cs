using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MenuPanel : UiPanel
    {
        public Button startButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            startButton = uiObject.Q<Button>("Button_PlayAgain");
            startButton.RegisterCallback<ClickEvent>(OnRetryButtonClicked); 
        }

        private void OnDisable()
        {
            uiObject.enabledSelf = false;
            startButton?.UnregisterCallback<ClickEvent>(OnRetryButtonClicked);
        }

        public override void OnShow()
        {
            base.OnShow();
           
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void OnRetryButtonClicked(ClickEvent evt)
        {
            HidePanel();
            GameController.Instance.CurrentGameState = GameState.Playing;
        }

    }
}