using System;
using UnityEngine;
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
        }

        public override void OnHide()
        {
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