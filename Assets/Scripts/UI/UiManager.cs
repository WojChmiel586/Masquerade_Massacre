using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour {
    // Singleton instance
    public static UIManager Instance;

    // Dictionary to store references to each panel
    private Dictionary<string, UiPanel> panels;

    public UnityAction UiInitComplete;

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
    private void Start()
    {
        var children = GetComponentsInChildren<UiPanel>().ToList();
        children.ForEach(c =>RegisterPanel(c.PanelName, c));
        UiInitComplete?.Invoke();
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
        //  Button to open the mission menu, which pauses the game and
        //  Identifies a new assassin (target)
        if (!context.started) return;
        if (panels.ContainsKey("MissionPanel"))
        {
            panels["MissionPanel"].ToggleUi();
        }
    }
}
