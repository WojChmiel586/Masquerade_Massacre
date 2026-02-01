using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
	public List<Collider2D> m_PatrolAreas = new();
	public List<Sprite> m_PatrolActivities = new();
	public List<DoorController> m_SpawnAreas = new();

	public Transform m_SpawnLocation;

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

	int iCurrentTargetAssignedZone;
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

	PatrolAgent2D InstantSpawn( bool bTarget = false )
	{
		GameObject xNewAgent = Instantiate( m_AgentPrefab, m_SpawnLocation.transform.position, Quaternion.identity, this.transform );
		PatrolAgent2D xAgentPatrol = xNewAgent.GetComponent<PatrolAgent2D>();

		int iPatrolAreaIndex = -1;
		int iMaskAttempt = -1;
		int iBodyType = -1;
		int iMaskColor = -1;
		int iMaskTrim = -1;

		bool bSpawn = false;
		while ( !bSpawn )
		{
			iPatrolAreaIndex = Random.Range( 0, m_PatrolAreas.Count );
			iMaskAttempt = Random.Range( 0, m_Masks.Count );
			iBodyType = Random.Range( 0, m_Bodies.Count );
			iMaskColor = Random.Range( 0, m_MaskColours.Count );
			iMaskTrim = Random.Range( 0, m_MaskColours.Count );

			bool bCheckActivity = m_PatrolActivities[ iPatrolAreaIndex ] != null;

			if ( ( bCheckActivity && iCurrentTargetAssignedZone == iPatrolAreaIndex ) &&
			iCurrentTargetMaskValue == iMaskAttempt &&
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

		xAgentPatrol.m_iAssignedZone = iPatrolAreaIndex;
		xAgentPatrol.m_PatrolArea = m_PatrolAreas[ iPatrolAreaIndex ];
		xAgentPatrol.m_ActivityObject = m_PatrolActivities[ iPatrolAreaIndex ];
		Sprite xMask = m_Masks[ iMaskAttempt ];
		Sprite xBody = m_Bodies[ iBodyType ];
		Sprite xHandLeft = m_HandsL[ iBodyType ];
		Sprite xHandRight = m_HandsR[ iBodyType ];
		UnityEngine.Color xMaskColor = m_MaskColours[ iMaskColor ];
		UnityEngine.Color xMaskTrim = m_MaskColours[ iMaskTrim ];

		xNewAgent.GetComponent<GuestDesignController>().SetGuestElements(
			xMask, xBody, xHandLeft, xHandRight, xMaskColor, xMaskTrim );


		if ( bTarget )
		{
			xAgentPatrol.m_IsTheTarget = true;
			SpawnTarget( iPatrolAreaIndex, iMaskAttempt, iBodyType, iMaskColor, iMaskTrim );
		}

		m_PatrolManager.AddAgent( xAgentPatrol );

		xAgentPatrol.m_DoorToEnterFrom = m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ];
		xAgentPatrol.m_DoorToLeaveFrom = m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ];

		return xAgentPatrol;
	}

	void DoorSpawn()
	{
		PatrolAgent2D xAgent = InstantSpawn();
	}

	void StartDoorDespawn( PatrolAgent2D xDespawningAgent )
	{
		xDespawningAgent.DoorDespawn();
	}

	public void OnJump()
	{
		InstantSpawn( true );
	}

	public void SpawnTarget( int iPatrolAreaIndex, int iMask, int iBody, int iMaskColor, int iTrim )
	{
		m_PatrolManager.DeleteSimilarToTargetFeatures( iPatrolAreaIndex, m_Masks[ iMask ], m_Bodies[ iBody ], m_MaskColours[ iMaskColor ], m_MaskColours[ iTrim ] );
		iCurrentTargetMaskValue = iMask;
		iCurrentTargetBodyValue = iBody;
		iMaskColorValue = iMaskColor;
		iTrimValue = iTrim;
	}
}
