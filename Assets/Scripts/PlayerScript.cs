using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public GameObject m_ScopeOverlay;
    public GameObject m_Scope;
    public Camera m_Camera;

    //Camera Scope values
    const float CAMERA_UNSCOPED_SIZE = 5.4f;
    const float CAMERA_SCOPED_SIZE = 1.5f;
    bool m_scopedIn = false;
    float m_cameraCurrentSize = 0;
    float m_cameraTargetSize = 0;

    void Start()
    {
        m_cameraCurrentSize = CAMERA_UNSCOPED_SIZE;
        m_cameraTargetSize = CAMERA_UNSCOPED_SIZE;
    }


    void Update()
    {
        CameraUpdate();
    }

    public void ZoomIn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Zoomed in");
            m_cameraTargetSize = CAMERA_SCOPED_SIZE;
            m_ScopeOverlay.SetActive(true);
            m_scopedIn = true;



        }

        if (context.canceled)
        {
            Debug.Log("Zoom out");
            m_cameraTargetSize = CAMERA_UNSCOPED_SIZE;
            m_ScopeOverlay.SetActive(false);
            m_scopedIn = false;
        }

    }

    public void MouseAim(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        Vector2 worldPos = m_Camera.ScreenToWorldPoint(pos);
        m_Scope.transform.position = worldPos;
        if (m_scopedIn)
        {
            m_Camera.gameObject.transform.position = worldPos;
        }
        Debug.Log(worldPos);
    }


    void CameraUpdate()
    {
        if (m_cameraCurrentSize != m_cameraTargetSize)
        {
            m_cameraCurrentSize = Mathf.Lerp(m_cameraCurrentSize, m_cameraTargetSize, 0.05f);
            m_Camera.orthographicSize = m_cameraCurrentSize;
        }
    }
}
