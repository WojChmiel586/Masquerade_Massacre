using UnityEngine;

public class PatrolAgent2D : MonoBehaviour
{
	public enum State { Idle, Move }

	[Header("Patrol Area")]
	public Collider2D m_PatrolArea;
	public float m_MinMoveDistance = 0.75f;
	public int m_MaxSampleAttempts = 12;

	[Header("Behaviour")]
	public float m_MoveSpeed = 2.0f;
	public Vector2 m_IdleTimeRange = new Vector2( 0.8f, 2.5f) ;
	public float m_ChanceStandStill = 0.35f; // probability to just idle again instead of moving

	[Header("Thinking")]
	public Vector2 m_ThinkIntervalRange = new Vector2( 0.25f, 0.6f ); // decision cadence (not movement)
	[HideInInspector] public float m_NextThinkTime;

	public State m_CurrentState { get; private set; } = State.Idle;

	Rigidbody2D m_RigidBody;
	Vector2 m_Target;
	float m_IdleUntil;

	void Awake()
	{
		m_RigidBody = GetComponent<Rigidbody2D>();
		// Stagger start times to avoid everyone thinking at frame 0
		m_NextThinkTime = Time.time + Random.Range( 0f, m_ThinkIntervalRange.y );
		EnterIdle();

		if ( m_PatrolArea != null && TryPickRandomPoint( out var xPoint ) )
		{
			m_RigidBody.position = xPoint;
		}
	}

	void FixedUpdate()
	{
		if ( m_CurrentState != State.Move ) return;

		Vector2 xPos = m_RigidBody.position;
		Vector2 xDir = ( m_Target - xPos );
		float fDist = xDir.magnitude;

		if (fDist < 0.05f)
		{
			EnterIdle();
			return;
		}

		Vector2 xStep = xDir * m_MoveSpeed * Time.fixedDeltaTime;
		m_RigidBody.MovePosition( xPos + xStep );
	}

	// Called by the manager when it's time to "think"
	public void Think( float fCurrentTime )
	{
		if ( m_CurrentState == State.Idle )
		{
			// Still idling? do nothing.
			if ( fCurrentTime < m_IdleUntil ) return;

			// Decide: stand still again or move somewhere else
			if ( Random.value < m_ChanceStandStill )
			{
				EnterIdle();
				return;
			}

			if ( TryPickRandomPoint( out var xPoint ) )
			{
				m_Target = xPoint;
				m_CurrentState = State.Move;
			}
			else
			{
				// If sampling failed, just idle a bit
				EnterIdle();
			}
		}
		else if ( m_CurrentState == State.Move )
		{
			// Functionality for if they need to do something when thinking while moving
		}
	}

	void EnterIdle()
	{
		m_CurrentState = State.Idle;
		float fTime = Random.Range( m_IdleTimeRange.x, m_IdleTimeRange.y );
		m_IdleUntil = Time.time + fTime;
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
}
