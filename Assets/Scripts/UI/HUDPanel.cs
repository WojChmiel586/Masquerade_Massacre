using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class HUDPanel : UiPanel
    {
        public Button startButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            startButton = uiObject.Q<Button>("button-dossier");
            startButton.RegisterCallback<ClickEvent>(OnOpenButtonClicked);
        }

        private void OnDisable()
        {
            uiObject.enabledSelf = false;
            startButton?.UnregisterCallback<ClickEvent>(OnOpenButtonClicked);
        }
        
        private void OnOpenButtonClicked(ClickEvent evt)
        {
            UIManager.Instance.ShowPanel("MissionPanel");
        }

    }
}