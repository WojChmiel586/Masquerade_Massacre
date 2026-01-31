using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    public class MissionPanel : UiPanel
    {
        public Button returnButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (returnButton == null) returnButton = uiObject.Q<Button>("ReturnButton");
            returnButton.RegisterCallback<ClickEvent>(OnReturnButtonClicked);
        }

        private void OnDisable()
        {
            returnButton?.UnregisterCallback<ClickEvent>(OnReturnButtonClicked);
        }

        public override void OnShow()
        {
            base.OnShow();
           
            GameController.Instance.CurrentGameState = GameState.Paused;
        }
        public override void OnHide()
        {
            base.OnHide();

            //  Because UI gets closed immediately after loading, don't change game state yet
            if (GameController.Instance.CurrentGameState is GameState.Paused) GameController.Instance.CurrentGameState = GameState.Playing;
        }

        private void OnReturnButtonClicked(ClickEvent evt)
        {
            HidePanel();
        }
        
    }
}