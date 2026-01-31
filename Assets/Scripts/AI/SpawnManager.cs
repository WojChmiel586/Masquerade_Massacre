using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public List<Collider2D> m_PatrolAreas = new();
	public GameObject m_AgentPrefab;
	public int m_MaxAgents;

	PatrolManager2D m_PatrolManager;

	void Start()
	{
		m_PatrolManager = GetComponent<PatrolManager2D>();

		for ( int i = 0; i <= m_MaxAgents; i++ )
		{
			InstantSpawn();
		}
	}

	void Update()
	{
		
	}

	void InstantSpawn()
	{
		GameObject xNewAgent = Instantiate( m_AgentPrefab );
		PatrolAgent2D xAgentPatrol = m_AgentPrefab.GetComponent<PatrolAgent2D>();
		xAgentPatrol.m_PatrolArea = m_PatrolAreas[ Random.Range( 0, m_PatrolAreas.Count ) ];

		m_PatrolManager.AddAgent( xNewAgent );
	}

	void NaturalSpawn( int iNumberToSpawn )
	{

	}
}
