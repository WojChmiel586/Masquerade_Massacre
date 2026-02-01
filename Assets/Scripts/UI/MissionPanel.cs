using System;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    public class MissionPanel : UiPanel
    {
        public Button returnButton;
        private ProgressBar assassinTimer;
        private ProgressBar roundTimer;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (returnButton == null) returnButton = uiObject.Q<Button>("ReturnButton");
            returnButton.RegisterCallback<ClickEvent>(OnReturnButtonClicked);
            assassinTimer =  uiObject.Q<ProgressBar>("bar-assassintime");
            roundTimer =  uiObject.Q<ProgressBar>("bar-timeleft");
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
            AudioManager.instance.OpenDossierSFX();
        }
        public override void OnHide()
        {
            base.OnHide();

            //  Because UI gets closed immediately after loading, don't change game state yet
            if (GameController.Instance.CurrentGameState is GameState.Paused) GameController.Instance.CurrentGameState = GameState.Playing;
        }

        private void FixedUpdate()
        {
            if (uiObject.visible)
            {
                // Round timer
                roundTimer.lowValue = 0;
                var currentRoundTime=  GameController.Instance.RoundTimer;
                roundTimer.highValue = GameController.Instance.RoundTimerLimit;
                roundTimer.value = (int)currentRoundTime;
                roundTimer.title = $"You must defend the MVP for {(int)currentRoundTime} ";
                
                // Assassin timer
                assassinTimer.lowValue = 0;
                var highVal = GameController.Instance.AssassinTimerLimit;
                var currentTime = GameController.Instance.AssassinTimer;
                assassinTimer.highValue = highVal;
                assassinTimer.value = (int)currentTime;
                assassinTimer.title = $"{(int)currentTime} remaining before target strikes!";
            }
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

            var bodySprite = uiObject.Q<Image>("image-bodytype");
            var maskColourSprite =  uiObject.Q<Image>("image-maskcolour");
            var maskTrimSprite =  uiObject.Q<Image>("image-masktrim");
            
            //  Set values

            switch (activity)
            {
                case -1:
                    trait1.text = "Likes: Roaming around";
                    activityImage.visible = false;
                    break;
                case 0:
                    trait1.text = "Likes: Snooker";
                    activityImage.visible = true;
                    break;
                case 1:
                    trait1.text = "Likes: Punch";
                    activityImage.visible = true;
                    break;
                case 2:
                    trait1.text = "Likes: Turkey";
                    activityImage.visible = true;
                    break;
            }
            targetMaskShape.image = targetData.MaskSprite.texture;
            if (targetData.ActivitySprite != null) activityImage.image = targetData.ActivitySprite.texture;
            bodySprite.image = targetData.BodySprite.texture;
            maskColourSprite.tintColor = targetData.MaskColour;
            maskTrimSprite.tintColor = targetData.MaskTrim;
        }
    }
}