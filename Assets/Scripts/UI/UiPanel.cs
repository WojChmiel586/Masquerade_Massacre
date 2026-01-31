using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UiPanel : MonoBehaviour
{
    [SerializeField] protected string panelName;
    public string PanelName { get { return panelName; } }
    protected VisualElement uiObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        uiObject = GetComponent<UIDocument>().rootVisualElement;
    }

    void Start()
    {
        UIManager.Instance.RegisterPanel(panelName, this);
        HidePanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUi()
    {
        if (gameObject.activeSelf) 
            HidePanel();
        else 
            ShowPanel();
    }
    public void ShowPanel()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        OnHide();
    }

    // Called when the panel is shown
    public virtual void OnShow() {
        // Logic for when the panel is displayed
    }

    // Called when the panel is hidden
    public virtual void OnHide() {
        // Logic for when the panel is hidden
    }
}
