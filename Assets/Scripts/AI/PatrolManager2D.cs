using System.Collections.Generic;
using UnityEngine;

public class PatrolManager2D : MonoBehaviour
{
	public List<PatrolAgent2D> m_Agents = new();
	public int m_MaxAgentsToThinkPerFrame = 12; // budget cap (prevents spikes)

	int m_Cursor = 0;

	void Update()
	{
		float fCurrentTime = Time.time;

		int iProcessed = 0;
		int iCount = m_Agents.Count;
		if ( iCount == 0 ) return;

		// Round-robin so we don't scan the list from 0 every frame
		while ( iProcessed < m_MaxAgentsToThinkPerFrame )
		{
			var xAgent = m_Agents[ m_Cursor ];
			m_Cursor = ( m_Cursor + 1 ) % iCount;

			if ( xAgent && fCurrentTime >= xAgent.m_NextThinkTime )
			{
				xAgent.Think( fCurrentTime );
				xAgent.ScheduleNextThink( fCurrentTime );
			}

			iProcessed++;
		}
	}

	public void AddAgent( GameObject xAgentObject )
	{
		m_Agents.Add( xAgentObject.GetComponent<PatrolAgent2D>() );
	}
}
