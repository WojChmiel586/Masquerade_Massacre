using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour {
    // Singleton instance
    public static UIManager Instance;

    // Dictionary to store references to each panel
    private Dictionary<string, UiPanel> panels;

    private void Awake() {
        // Ensure singleton instance
        if (Instance == null) {
            Instance = this;
            panels = new Dictionary<string, UiPanel>();
        }
        else {
            Destroy(gameObject);  // Ensure only one UIManager exists
        }
    }

    
    // Register a panel with a unique name
    public void RegisterPanel(string panelName, UiPanel panel) {
        if (!panels.ContainsKey(panelName)) {
            panels.Add(panelName, panel);
            panel.HidePanel();  // Hide panel by default
        }
    }

    // Show a specific panel by name
    public void ShowPanel(string panelName) {
        if (panels.ContainsKey(panelName)) {
            // Hide all panels before showing the new one
            HideAllPanels();
            panels[panelName].ShowPanel();
        }
        else {
            Debug.LogWarning("Panel " + panelName + " not found!");
        }
    }

    // Hide a specific panel
    public void HidePanel(string panelName) {
        if (panels.ContainsKey(panelName)) {
            panels[panelName].HidePanel();
        }
        else {
            Debug.LogWarning("Panel " + panelName + " not found!");
        }
    }

    // Hide all panels
    private void HideAllPanels() {
        foreach (var panel in panels.Values) {
            panel.HidePanel();
        }
    }
    public void MissionButton(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (panels.ContainsKey("MissionPanel"))
        {
            panels["MissionPanel"].ToggleUi();
        }
    }
}
