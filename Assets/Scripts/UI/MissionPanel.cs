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
            //returnButton = uiObject.
        }

        public override void OnHide()
        {
            //     base.OnHide();
            // }
        }

        [SerializeField] private InputAction.CallbackContext test;

        private void LateUpdate()
        {
        }
    }
}