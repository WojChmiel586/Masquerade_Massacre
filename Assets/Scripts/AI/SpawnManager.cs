using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.LowLevelPhysics2D.PhysicsBody;

public class GuestIdentifiers : IEquatable<GuestIdentifiers>
{
	public int m_iActivity;
	public int m_iMaskDesign;
	public int m_iBodyType;
	public int m_iMaskColor;
	public int m_iTrimColor;

	public static bool operator ==( GuestIdentifiers xGuest1, GuestIdentifiers xGuest2 )
	{
		if ( ReferenceEquals( xGuest1, xGuest2 ) )
			return true;
		if ( ReferenceEquals( xGuest1, null ) )
			return false;
		if ( ReferenceEquals( xGuest2, null ) )
			return false;
		return xGuest1.Equals( xGuest2 );
	}
	public static bool operator !=( GuestIdentifiers xGuest1, GuestIdentifiers xGuest2 ) => !( xGuest1 == xGuest2 );

	public bool Equals( GuestIdentifiers xOther )
	{
		if ( ReferenceEquals( xOther, null ) )
			return false;
		if ( ReferenceEquals( this, xOther ) )
			return true;
		return m_iActivity.Equals( xOther.m_iActivity )
			   && m_iMaskDesign.Equals( xOther.m_iMaskDesign )
			   && m_iBodyType.Equals( xOther.m_iBodyType )
			   && m_iMaskColor.Equals( xOther.m_iMaskColor )
			   && m_iTrimColor.Equals( xOther.m_iTrimColor );
	}
	public override bool Equals( object xObj ) => Equals( xObj as GuestIdentifiers );

	public override int GetHashCode()
	{
		unchecked
		{
			int iHashCode = m_iMaskDesign.GetHashCode();
			iHashCode = ( iHashCode * 397 ) ^ m_iBodyType.GetHashCode();
			iHashCode = ( iHashCode * 397 ) ^ m_iMaskColor.GetHashCode();
			iHashCode = ( iHashCode * 397 ) ^ m_iTrimColor.GetHashCode();
			return iHashCode;
		}
	}

}

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

	GuestIdentifiers m_CurrentTargetIdentifiers = new();

	void Awake()
	{
		m_PatrolManager = GetComponent<PatrolManager2D>();

		for ( int i = 0; i < m_MaxAgents; i++ )
		{
			InstantSpawn();
		}
	}

	void LateUpdate()
	{
		if ( m_PatrolManager.m_Agents.Count < m_MaxAgents )
		{
			InstantSpawn();
		}
	}

	PatrolAgent2D InstantSpawn()
	{
		GameObject xNewAgent = Instantiate( m_AgentPrefab, m_SpawnLocation.transform.position, Quaternion.identity, this.transform );
		PatrolAgent2D xAgentPatrol = xNewAgent.GetComponent<PatrolAgent2D>();

		bool bSpawn = false;
		GuestIdentifiers xNewGuestIdentifiers = new();
		int iAttempts = 0;
		int iPotentialPatrol = 0;
		while ( !bSpawn )
		{
			iPotentialPatrol = UnityEngine.Random.Range( 0, m_PatrolAreas.Count );
			xNewGuestIdentifiers.m_iActivity = 
				( iPotentialPatrol >= m_PatrolActivities.Count || m_PatrolActivities[ iPotentialPatrol ] == null ) ? -1 : iPotentialPatrol;
			xNewGuestIdentifiers.m_iMaskDesign = UnityEngine.Random.Range( 0, m_Masks.Count );
			xNewGuestIdentifiers.m_iBodyType = UnityEngine.Random.Range( 0, m_Bodies.Count );
			xNewGuestIdentifiers.m_iMaskColor = UnityEngine.Random.Range( 0, m_MaskColours.Count );
			xNewGuestIdentifiers.m_iTrimColor = UnityEngine.Random.Range( 0, m_MaskColours.Count );

			if ( xNewGuestIdentifiers == m_CurrentTargetIdentifiers )
			{
				bSpawn = false;
				iAttempts++;
				if( iAttempts == 100 )
				{
					return null;
				}
			}
			else
			{
				bSpawn = true;
			}
		}

		xAgentPatrol.m_PatrolArea = m_PatrolAreas[ iPotentialPatrol ];
		if ( xNewGuestIdentifiers.m_iActivity != -1 )
		{
			xAgentPatrol.m_ActivityObject = m_PatrolActivities[ xNewGuestIdentifiers.m_iActivity ];
		}

		xAgentPatrol.m_GuestIdentifiers = xNewGuestIdentifiers;

		xNewAgent.GetComponent<GuestDesignController>().SetGuestElements(
			m_Masks[ xNewGuestIdentifiers.m_iMaskDesign ],
			m_Bodies[ xNewGuestIdentifiers.m_iBodyType ],
			m_HandsL[ xNewGuestIdentifiers.m_iBodyType ],
			m_HandsR[ xNewGuestIdentifiers.m_iBodyType ],
			m_MaskColours[ xNewGuestIdentifiers.m_iMaskColor ],
			m_MaskColours[ xNewGuestIdentifiers.m_iTrimColor ] );



		m_PatrolManager.AddAgent( xAgentPatrol );

		xAgentPatrol.m_DoorToEnterFrom = m_SpawnAreas[ UnityEngine.Random.Range( 0, m_SpawnAreas.Count ) ];
		xAgentPatrol.m_DoorToLeaveFrom = m_SpawnAreas[ UnityEngine.Random.Range( 0, m_SpawnAreas.Count ) ];

		return xAgentPatrol;
	}

	public void SpawnTarget()
	{
		PatrolAgent2D xAgent = InstantSpawn();
		xAgent.m_IsTheTarget = true;
		SpawnTarget( xAgent.m_GuestIdentifiers );
	}

	void StartDoorDespawn( PatrolAgent2D xDespawningAgent )
	{
		xDespawningAgent.DoorDespawn();
	}

	public void OnJump()
	{
		SpawnTarget();
	}

	public void SpawnTarget( GuestIdentifiers xGuestIdentifiers )
	{
		m_PatrolManager.DeleteSimilarToTargetFeatures( xGuestIdentifiers );
		m_CurrentTargetIdentifiers = xGuestIdentifiers;
	}
}
