using UnityEngine;

using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
    // Singleton instance
    public static UIManager Instance;

    // Dictionary to store references to each panel
    private Dictionary<string, GameObject> panels;

    private void Awake() {
        // Ensure singleton instance
        if (Instance == null) {
            Instance = this;
            panels = new Dictionary<string, GameObject>();
        }
        else {
            Destroy(gameObject);  // Ensure only one UIManager exists
        }
    }

    // Register a panel with a unique name
    public void RegisterPanel(string panelName, GameObject panel) {
        if (!panels.ContainsKey(panelName)) {
            panels.Add(panelName, panel);
            panel.SetActive(false);  // Hide panel by default
        }
    }

    // Show a specific panel by name
    public void ShowPanel(string panelName) {
        if (panels.ContainsKey(panelName)) {
            // Hide all panels before showing the new one
            HideAllPanels();
            panels[panelName].SetActive(true);
        }
        else {
            Debug.LogWarning("Panel " + panelName + " not found!");
        }
    }

    // Hide a specific panel
    public void HidePanel(string panelName) {
        if (panels.ContainsKey(panelName)) {
            panels[panelName].SetActive(false);
        }
        else {
            Debug.LogWarning("Panel " + panelName + " not found!");
        }
    }

    // Hide all panels
    private void HideAllPanels() {
        foreach (var panel in panels.Values) {
            panel.SetActive(false);
        }
    }
}
