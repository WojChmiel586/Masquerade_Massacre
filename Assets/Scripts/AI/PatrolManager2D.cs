using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PatrolManager2D : MonoBehaviour
{
	public List<PatrolAgent2D> m_Agents = new();
	public int m_MaxAgentsToThinkPerFrame = 50; // budget cap (prevents spikes)

	int m_Cursor = 0;

	void Update()
	{
		float fCurrentTime = Time.time;

		int iProcessed = 0;
		int iCount = m_Agents.Count;
		if ( iCount == 0 ) return;
		List<PatrolAgent2D> xAgentsToRemove = new();

		// Round-robin so we don't scan the list from 0 every frame
		while ( iProcessed < m_MaxAgentsToThinkPerFrame )
		{
			m_Cursor = ( m_Cursor + 1 ) % iCount;
			var xAgent = m_Agents[ m_Cursor ];

			if ( xAgent.m_FlagForDeletion == true )
			{
				m_Agents.Remove( xAgent );
				Destroy( xAgent.gameObject );
				iCount = m_Agents.Count;
			}

			else if ( xAgent && fCurrentTime >= xAgent.m_NextThinkTime )
			{
				xAgent.Think( fCurrentTime );
				xAgent.ScheduleNextThink( fCurrentTime );
			}

			iProcessed++;
		}

		while ( xAgentsToRemove.Count > 0 )
		{
 			m_Agents.Remove( xAgentsToRemove[ xAgentsToRemove.Count - 1 ] );
			Destroy( xAgentsToRemove[ xAgentsToRemove.Count - 1 ].gameObject );
			xAgentsToRemove.Remove( xAgentsToRemove[ xAgentsToRemove.Count - 1 ] );
		}
	}

	public void AddAgent( PatrolAgent2D xAgentObject )
	{
		m_Agents.Add( xAgentObject );
	}

	public void DeleteSimilarToTargetFeatures( GuestIdentifiers xGuestIdentifiers )
	{
		for ( int i = m_Agents.Count - 1; i >= 0; i-- )
		{
			if ( !m_Agents[ i ].m_IsTheTarget && xGuestIdentifiers == m_Agents[ i ].m_GuestIdentifiers )
			{
				Destroy( m_Agents[ i ].gameObject );
				Destroy( m_Agents[ i ] );
				m_Agents.Remove( m_Agents[ i ] );
			}
		}
	}
}
