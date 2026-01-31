using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerScript : MonoBehaviour
{
    public GameObject m_ScopeOverlay;
    public GameObject m_Scope;
    public CinemachineCamera m_UnscopedCamera;
    public CinemachineCamera m_ScopedCamera;
    public Camera m_Camera;
    public PlayerInput m_input;
    InputAction m_Action;

    //Camera Scope values
    const float CAMERA_UNSCOPED_SIZE = 5.4f;
    const float CAMERA_SCOPED_SIZE = 1.5f;
    bool m_scopedIn = false;
    Vector3 m_Mousepos = Vector3.zero;
    [Range(0.005f, 0.1f)]
    public float m_UnscopedSensitivity = 0.05f;
    [Range(0.005f, 0.1f)]
    public float m_ScopedSensitivty = 0.05f;

    Vector3 delta;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        m_Action = m_input.actions.FindAction("Look");

    }


    void Update()
    {
        MouseAim();
    }

    public void ZoomIn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Zoomed in");
            m_ScopeOverlay.SetActive(true);
            m_scopedIn = true;

        }

        if (context.canceled)
        {
            Debug.Log("Zoom out");
            m_ScopeOverlay.SetActive(false);
            m_scopedIn = false;
        }
        m_UnscopedCamera.gameObject.SetActive(!m_scopedIn);
        m_ScopedCamera.gameObject.SetActive(m_scopedIn);

    }

    public void MouseAim()
    {

        delta = m_Action.ReadValue<Vector2>();
        if (m_scopedIn)
        {
            delta *= m_ScopedSensitivty;
            m_Mousepos += delta;
        }
        else
        {
            delta *= m_UnscopedSensitivity;
            m_Mousepos += delta;
        }

        m_Mousepos.x = Mathf.Clamp(m_Mousepos.x, -9.6f, 9.6f);
        m_Mousepos.y = Mathf.Clamp(m_Mousepos.y, -5.4f, 5.4f);
        m_Scope.transform.position = m_Mousepos;
        //Debug.Log(delta);
    }
}
