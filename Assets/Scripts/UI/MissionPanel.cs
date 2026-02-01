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
            AssignTargetData();
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

        public void AssignTargetData()
        {
            var targetData = GameController.Instance.TargetController.GuestData;
            var activity = GameController.Instance.TargetController.GetIdentifiers().m_iActivity;
            var targetMaskShape = uiObject.Q<Image>("img-targetMask");
            var trait1 = uiObject.Q<Label>("Trait_1");
            var activityImage = uiObject.Q<Image>("image-activity");
            
            //  Set values

            switch (activity)
            {
                case -1:
                    trait1.text = "Likes: Roaming around";
                    break;
                case 0:
                    trait1.text = "Likes: Snooker";
                    break;
                case 1:
                    trait1.text = "Likes: Punch";
                    break;
                case 2:
                    trait1.text = "Likes: Turkey";
                    break;
            }
            targetMaskShape.image = targetData.MaskSprite.texture;
            activityImage.image = targetData.ActivitySprite.texture;

        }
    }
}