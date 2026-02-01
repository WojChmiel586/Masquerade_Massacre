using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Collections.Generic;
using DefaultNamespace;

public class PlayerScript : MonoBehaviour
{
    public GameObject m_ScopeOverlay;
    public GameObject m_Scope;
    public GameObject m_ScopeSprite;
    public GameObject m_UnscopedCursor;
    public CinemachineCamera m_UnscopedCamera;
    public CinemachineCamera m_ScopedCamera;
    public PlayerInput m_input;
    public LayerMask m_ScopeLayer;
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
    Camera m_MainCam;
    public ContactFilter2D m_ContactFilter;

    Vector3 delta;

    void Start()
    {
        m_Action = m_input.actions.FindAction("Look");
        m_MainCam = Camera.main;
        m_ContactFilter.layerMask = m_ScopeLayer;

    }


    void Update()
    {
        MouseAim();
    }

    public void ZoomIn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_scopedIn = true;
        }
        if (context.canceled)
        {
            m_scopedIn = false;
        }


        m_ScopeOverlay.SetActive(m_scopedIn);
        m_UnscopedCamera.gameObject.SetActive(!m_scopedIn);
        m_ScopedCamera.gameObject.SetActive(m_scopedIn);
        m_ScopeSprite.SetActive(m_scopedIn);
        m_UnscopedCursor.SetActive(!m_scopedIn);

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
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (!m_scopedIn)
        {
            return;
        }
        if (context.performed)
        {
            AudioManager.instance.ShootSFX();
            Collider2D gunTarget = null;
            List<Collider2D> colliders = new List<Collider2D>();
            int test = Physics2D.OverlapCircle(m_Mousepos, 0.1f, m_ContactFilter, colliders);
            if (colliders.Count > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (gunTarget == null)
                    {
                        gunTarget = collider;
                    }
                    else
                    {
                        if (collider.transform.localScale.x > gunTarget.transform.localScale.x)
                        {
                            gunTarget = collider;
                        }
                    }

                }
                gunTarget.transform.GetComponentInParent<PatrolAgent2D>().m_FlagForDeletion = true;

            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_Mousepos, 0.1f);
    }
}
