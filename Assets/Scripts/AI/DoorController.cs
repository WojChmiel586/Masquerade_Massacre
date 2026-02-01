using UnityEngine;

public class DoorController : MonoBehaviour
{
	[SerializeField]
	float m_TimeToHoldDoorOpen;
	[SerializeField ]
	SpriteRenderer m_DoorSpriteRenderer;
	float m_Timer;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if ( m_Timer > 0.0f )
		{
			m_Timer -= Time.deltaTime;
		}
		else if ( m_DoorSpriteRenderer.enabled )
		{
			m_DoorSpriteRenderer.enabled = false;
		}
	}

	public void OpenDoor()
	{
		m_Timer = m_TimeToHoldDoorOpen;
		if( !m_DoorSpriteRenderer.enabled )
		{
			// Play door open sound
			m_DoorSpriteRenderer.enabled = true;
		}
	}
}
