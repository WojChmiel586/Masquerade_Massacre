using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
	public List<Collider2D> m_PatrolAreas = new();
	public List<Collider2D> m_SpawnAreas = new();

	public GameObject m_AgentPrefab;
	public int m_MaxAgents;

	PatrolManager2D m_PatrolManager;

	[SerializeField] InputAction.CallbackContext m_CallbackSpace;

	[Header( "Guest Components" )]
	public List<Sprite> m_Masks = new();
	public List<Sprite> m_Bodies = new();
	public List<Sprite> m_HandsL = new();
	public List<Sprite> m_HandsR = new();
	public List<UnityEngine.Color> m_MaskColours = new();

	int iCurrentTargetMaskValue;
	int iCurrentTargetBodyValue;
	int iMaskColorValue;
	int iTrimValue;

	void Awake()
	{
		m_PatrolManager = GetComponent<PatrolManager2D>();

		for ( int i = 0; i < m_MaxAgents; i++ )
		{
			DoorSpawn();
		}
	}

	void LateUpdate()
	{
		if ( m_PatrolManager.m_Agents.Count < m_MaxAgents )
		{
			DoorSpawn();
		}
	}

	PatrolAgent2D InstantSpawn( Vector2 xSpawnPosition, bool bTarget = false )
	{
		GameObject xNewAgent = Instantiate( m_AgentPrefab, xSpawnPosition, Quaternion.identity, this.transform );
		PatrolAgent2D xAgentPatrol = xNewAgent.GetComponent<PatrolAgent2D>();
		int iPatrolAreaIndex = Random.Range( 0, m_PatrolAreas.Count );
		xAgentPatrol.m_PatrolArea = m_PatrolAreas[ iPatrolAreaIndex ];
		
		int iMaskAttempt = -1;
		int iBodyType = -1;
		int iMaskColor = -1;
		int iMaskTrim = -1;

		bool bSpawn = false;
		while ( !bSpawn )
		{
			iMaskAttempt = Random.Range( 0, m_Masks.Count );
			iBodyType = Random.Range( 0, m_Bodies.Count );
			iMaskColor = Random.Range( 0, m_MaskColours.Count );
			iMaskTrim = Random.Range( 0, m_MaskColours.Count );

			if ( iCurrentTargetMaskValue == iMaskAttempt &&
			iCurrentTargetBodyValue == iBodyType &&
			iMaskColorValue == iMaskColor &&
			iTrimValue == iMaskTrim )
			{
				bSpawn = false;
			}
			else
			{
				bSpawn = true;
			}
		}

		Sprite xMask = m_Masks[ iMaskAttempt ];
		Sprite xBody = m_Bodies[ iBodyType ];
		Sprite xHandLeft = m_HandsL[ iBodyType ];
		Sprite xHandRight = m_HandsR[ iBodyType ];
		UnityEngine.Color xMaskColor = m_MaskColours[ iMaskColor ];
		UnityEngine.Color xMaskTrim = m_MaskColours[ iMaskTrim ];

		xNewAgent.GetComponent<GuestDesignController>().SetGuestElements(
			xMask, xBody, xHandLeft, xHandRight, xMaskColor, xMaskTrim );

		// Double check that the agent is assigned a patrol area
		if ( xAgentPatrol.m_PatrolArea == null )
		{
			xAgentPatrol.m_PatrolArea = m_PatrolAreas[ 0 ];
		}

		if( bTarget )
		{
			SpawnTarget( iMaskAttempt, iBodyType, iMaskColor, iMaskTrim );
		}

		m_PatrolManager.AddAgent( xAgentPatrol );

		xAgentPatrol.m_DoorToLeaveFrom = m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ].transform.position;

		return xAgentPatrol;
	}

	void DoorSpawn()
	{
		PatrolAgent2D xAgent = InstantSpawn( m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ].transform.position );
	}

	void StartDoorDespawn( PatrolAgent2D xDespawningAgent )
	{
		xDespawningAgent.DoorDespawn( m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ].transform.position );
	}

	public void OnJump()
	{
		InstantSpawn( m_SpawnAreas[ 0 ].transform.position, true );
	}

	public void SpawnTarget( int xMask, int xBody, int xMaskColor, int xTrim )
	{
		m_PatrolManager.DeleteSimilarToTargetFeatures( m_Masks[ xMask ], m_Bodies[ xBody ], m_MaskColours[ xMaskColor ], m_MaskColours[ xTrim ] );
		iCurrentTargetMaskValue = xMask;
		iCurrentTargetBodyValue = xBody;
		iMaskColorValue = xMaskColor;
		iTrimValue = xTrim;
	}
}
