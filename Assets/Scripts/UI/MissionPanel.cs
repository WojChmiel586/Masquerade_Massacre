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
        
        public override void OnShow()
        {
            base.OnShow();
            if (returnButton == null) returnButton = uiObject.Q<Button>("ReturnButton");
            GameController.Instance.CurrentGameState = GameState.Paused;
        }

        public override void OnHide()
        {
            base.OnHide();

            //  Because UI gets closed immediately after loading, don't change game state yet
            if (GameController.Instance.CurrentGameState is GameState.Paused) GameController.Instance.CurrentGameState = GameState.Playing;
            if (returnButton != null)
            {
                Debug.Log("Btw: " + returnButton.name);
            }
        }

        private void OnReturnButtonClicked()
        {
            HidePanel();
        }
        
    }
}