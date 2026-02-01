using DefaultNamespace;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAgent2D : MonoBehaviour
{
	public enum State { IDLE, MOVE, LEAVE, DEAD }

	[Header("Guest Identifiers")]
	public GuestIdentifiers m_GuestIdentifiers;
	public bool m_IsTheTarget = false;
	public bool m_IsVIP = false;

	[ Header( "Patrol Area" )]
	public Collider2D m_PatrolArea;
	public float m_MinMoveDistance = 0.75f;
	public int m_MaxSampleAttempts = 12;

	[Header("Behaviour")]
	public float m_MoveSpeed = 2.0f;
	public Vector2 m_IdleTimeRange = new Vector2( 0.8f, 2.5f) ;
	public float m_ChanceStandStill = 0.35f; // probability to just idle again instead of moving
	public float m_ChanceToLeave = 0.05f; // probability to leave the event
	public float m_ChanceToChooseADifferentActivity = 0.1f; // probability to change target at the event
	public DoorController m_DoorToEnterFrom;
	public DoorController m_DoorToLeaveFrom;
	[SerializeField] List<Color> m_TestColors = new();
	public Sprite m_ActivityObject;
	[SerializeField] SpriteRenderer m_HoldingObjectSpriteRenderer;

	[Header("Thinking")]
	public Vector2 m_ThinkIntervalRange = new Vector2( 0.25f, 0.6f ); // decision cadence (not movement)
	[HideInInspector] public float m_NextThinkTime;

	[Header("Debug")]
	public bool m_Debug;

	public State m_CurrentState { get; private set; } = State.MOVE;

	public bool m_FlagForDeletion = false;
	public bool m_ForceDelete = false;

	Rigidbody2D m_RigidBody;
	Vector2 m_Target;
	float m_IdleUntil;
	SpriteRenderer m_SpriteRenderer;
	Animator m_Animator;

	bool m_bFirstMove;

	[SerializeField]
	float m_TimeToDie;

	void Start()
	{
		m_RigidBody = GetComponent<Rigidbody2D>();
		m_Animator = GetComponent<Animator>();
		m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

		m_bFirstMove = true;

		// Stagger start times to avoid everyone thinking at frame 0
		m_NextThinkTime = Time.time + Random.Range( 0f, m_ThinkIntervalRange.y );
		EnterIdle();
	}

	void FixedUpdate()
	{
		if ( m_CurrentState == State.IDLE ) return;

		if( m_CurrentState == State.DEAD )
		{
			m_TimeToDie -= Time.deltaTime;
			if( m_TimeToDie <= 0.0f )
			{
				m_FlagForDeletion = true;
			}
			return;
		}

		Vector2 xPos = m_RigidBody.position;
		Vector2 xDir = ( m_Target - xPos );
		float fDist = xDir.magnitude;

		if ( m_CurrentState == State.LEAVE && fDist < 1.0f )
		{
			m_DoorToLeaveFrom.OpenDoor();
		}

		if ( fDist < 0.05f )
		{
			if( m_CurrentState == State.LEAVE ) 
			{
				m_FlagForDeletion = true;
			}

			if ( m_ActivityObject != null )
			{
				m_HoldingObjectSpriteRenderer.sprite = m_ActivityObject;
			}

			EnterIdle();
			return;
		}

		Vector2 xStep = xDir / Mathf.Max( fDist, 0.0001f ) * m_MoveSpeed * Time.fixedDeltaTime;
		m_RigidBody.MovePosition( xPos + xStep );
	}

	// Called by the manager when it's time to "think"
	public void Think( float fCurrentTime )
	{
		if ( m_CurrentState == State.IDLE )
		{
			if ( m_FlagForDeletion ) return;

			// Still idling? do nothing.
			if ( fCurrentTime < m_IdleUntil ) return;

			// Decide: stand still again or move somewhere else
			if ( Random.value < m_ChanceStandStill )
			{
				EnterIdle();
				return;
			}

			if ( !m_bFirstMove && !m_IsTheTarget && !m_IsVIP && Random.value < m_ChanceToLeave )
			{
				EnterLeave();
				return;
			}

			if ( TryPickRandomPoint( out var xPoint ) )
			{
				if ( m_bFirstMove )
				{
					transform.position = m_DoorToEnterFrom.transform.position;
					m_DoorToEnterFrom.OpenDoor();
					m_bFirstMove = false;
				}
				m_Target = xPoint;
				m_CurrentState = State.MOVE;
				m_Animator.SetBool( "Walking", true );

				if ( m_Debug && m_IsTheTarget )
				{
					m_SpriteRenderer.color = m_TestColors[ 1 ];
				}
			}
			else
			{
				// If sampling failed, just idle a bit
				EnterIdle();
			}
		}
		else if ( m_CurrentState == State.MOVE )
		{
			// Functionality for if they need to do something when thinking while moving
		}
	}

	void EnterIdle()
	{
		m_CurrentState = State.IDLE;
		float fTime = Random.Range( m_IdleTimeRange.x, m_IdleTimeRange.y );
		m_IdleUntil = Time.time + fTime;
		m_Animator.SetBool( "Walking", false );

		if ( m_Debug && m_IsTheTarget )
		{
			m_SpriteRenderer.color = m_TestColors[ 0 ];
		}
	}

	void EnterLeave()
	{
		m_CurrentState = State.LEAVE;
		m_Target = m_DoorToLeaveFrom.transform.position;
		m_Animator.SetBool( "Walking", true );
		if ( m_Debug && m_IsTheTarget )
		{
			m_SpriteRenderer.color = m_TestColors[ 2 ];
		}
	}

	bool TryPickRandomPoint( out Vector2 xPoint )
	{
		xPoint = m_RigidBody.position;
		if ( !m_PatrolArea )
			return false;
	
		Bounds xBounds = m_PatrolArea.bounds;
		Vector2 xOrigin = m_RigidBody.position;

		for ( int i = 0; i < m_MaxSampleAttempts; i++ )
		{
			Vector2 xCandidate = new Vector2(
				Random.Range( xBounds.min.x, xBounds.max.x ),
				Random.Range( xBounds.min.y, xBounds.max.y )
			);
	
			if ( !m_PatrolArea.OverlapPoint( xCandidate ) )
				continue;

			if ( Vector2.Distance( xOrigin, xCandidate ) < m_MinMoveDistance )
				continue;
	
			xPoint = xCandidate;
			return true;
		}
	
		return false;
	}

	public void ScheduleNextThink( float fCurrentTime )
	{
		m_NextThinkTime = fCurrentTime + Random.Range( m_ThinkIntervalRange.x, m_ThinkIntervalRange.y );
	}

	public void DoorDespawn()
	{
		m_PatrolArea = null;
		m_Target = m_DoorToLeaveFrom.transform.position;
		m_CurrentState = State.LEAVE;
	}

	public void Shot()
	{
		if (m_CurrentState != State.DEAD )
		{
			m_CurrentState = State.DEAD;
			m_Animator.SetBool( "Dead", true );
			if( m_IsTheTarget)
			{
				GameController.Instance.OnTargetKilled();
			}
			if( m_IsVIP )
			{
				GameController.Instance.m_VIPDead = true;
			}
		}
	}
}
