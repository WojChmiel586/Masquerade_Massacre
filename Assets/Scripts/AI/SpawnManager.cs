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

	PatrolAgent2D InstantSpawn( Vector2 xSpawnPosition )
	{
		GameObject xNewAgent = Instantiate( m_AgentPrefab, xSpawnPosition, Quaternion.identity, this.transform );
		PatrolAgent2D xAgentPatrol = xNewAgent.GetComponent<PatrolAgent2D>();
		int iPatrolAreaIndex = Random.Range( 0, m_PatrolAreas.Count );
		xAgentPatrol.m_PatrolArea = m_PatrolAreas[ iPatrolAreaIndex ];

		// Double check that the agent is assigned a patrol area
		if ( xAgentPatrol.m_PatrolArea == null )
		{
			xAgentPatrol.m_PatrolArea = m_PatrolAreas[ 0 ];
		}

		m_PatrolManager.AddAgent( xAgentPatrol );

		return xAgentPatrol;
	}

	void DoorSpawn()
	{
		PatrolAgent2D xAgent = InstantSpawn( m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ].transform.position );
		xAgent.m_DoorToLeaveFrom = m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ].transform.position;
	}

	void StartDoorDespawn( PatrolAgent2D xDespawningAgent )
	{
		xDespawningAgent.DoorDespawn( m_SpawnAreas[ Random.Range( 0, m_SpawnAreas.Count ) ].transform.position );
	}

	public void OnJump()
	{
		StartDoorDespawn( m_PatrolManager.m_Agents[ Random.Range( 0, m_PatrolManager.m_Agents.Count ) ] );
	}
}
