using UnityEngine;
public enum MOVE_State { COMBAT_STATE, MOVE_STATE, IDLE_STATE, ENGAGE_STATE, DISENGAGE_STATE }
public class HeroMovement : MovementScript
{
	[SerializeField]
	private bool m_AttMvKey = false;
	[SerializeField]
	private MOVE_State m_state, m_prevState;
	private UnityEngine.AI.NavMeshAgent agent;
	private HeroInfo info;
	private UnityEngine.AI.NavMeshHit nmhit;
	private Vector3 rot, newVel;
	private float prevMousePos, currMousePos, currentRot, tValue = 0.0f;
	private bool shiftKeyPressed, moving = false;
	public bool SprintingAbility
	{ private get; set; }
	public void ResetToSpawn()
	{
		UnityEngine.AI.NavMesh.SamplePosition( GameManager.blueHeroSpawnPosition, out nmhit, 5.0f, UnityEngine.AI.NavMesh.AllAreas
			);
		transform.position = new Vector3( nmhit.position.x, agent.baseOffset * transform.localScale.
			y, nmhit.position.z );
		transform.rotation = new Quaternion( 0.0f, 0.707106781f, 0.0f, 0.707106781f );
		currentRot = transform.rotation.eulerAngles.y;
	}
	protected override void Start()
	{
		base.Start();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		m_state = m_prevState = MOVE_State.IDLE_STATE;
		info = GetComponent<HeroInfo>();
		prevMousePos = currMousePos = Input.mousePosition.x;
		currentRot = transform.rotation.eulerAngles.y;
		SprintingAbility = false;
		agent.angularSpeed = 0.0f;
	}
	private void Update()
	{
		TPUpdate();
		if ( !HeroCamScript.onHero && GameManager.GameRunning )
		{
			switch ( m_state )
			{
				case MOVE_State.MOVE_STATE:
					if ( agent.enabled )
						agent.Resume();
					if ( CheckInput() || m_attackMOve )
						if ( inCombat && m_attackMOve )
							SetState( MOVE_State.ENGAGE_STATE );
					if ( agent.enabled && agent.remainingDistance <= 0.0f )
						SetState( MOVE_State.IDLE_STATE );
					break;
				case MOVE_State.COMBAT_STATE:
					if ( !CheckInput() || m_attackMOve )
					{
						if ( inCombat && TargetPosition )
						{
							withinRange = Vector3.Distance( transform.position, TargetPosition.
								position ) <= combatRange;
							if ( agent.enabled )
							{
								if ( withinRange )
									agent.Stop();
								else
									agent.Resume();
							}
						}
						else
							SetState( MOVE_State.DISENGAGE_STATE );
					}
					break;
				case MOVE_State.IDLE_STATE:
					m_attackMOve = true;
					if ( !CheckInput() )
					{
						if ( CameraControl.instance.CameraFollowsPlayer && inCombat )
							SetState( MOVE_State.ENGAGE_STATE );
						else if ( inCombat && TargetPosition )
							withinRange = Vector3.Distance( transform.position, TargetPosition.
								position ) <= combatRange;
					}
					break;
				case MOVE_State.ENGAGE_STATE:
					if ( agent.enabled )
						agent.destination = TargetPosition.position;
					withinRange = false;
					SetState( MOVE_State.COMBAT_STATE );
					break;
				case MOVE_State.DISENGAGE_STATE:
					if ( agent.enabled )
						agent.destination = hit.point;
					inCombat = false;
					withinRange = false;
					SetState( m_prevState );
					m_prevState = m_state;
					break;
				default:
					break;
			}
		}
	}
	private void TPUpdate()
	{
		shiftKeyPressed = Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift );
		agent.speed = ( ( SprintingAbility ) ? ( shiftKeyPressed ? 25000.0f : 21500.0f ) : (
			shiftKeyPressed ? 16000.0f : 10500.0f ) );
		currMousePos = Input.mousePosition.x;
		bool shouldRepositionToCenter = HeroCamScript.onHero && GameManager.GameRunning && !
			heroCamDisabler.disabledCameraMovement && StateID.STATE_SHOP != ApplicationManager.
			Instance.GetAppState();
		if ( Mathf.Abs( Screen.width * 0.5f - currMousePos ) >= ( Screen.width * 0.2f ) || Mathf.Abs
			( Screen.height * 0.5f - HeroCamScript.MouseVertical ) >= ( Screen.height * 0.2f ) )
		{
			Cursor.lockState = shouldRepositionToCenter ? CursorLockMode.Locked : CursorLockMode.
				None;
			if ( shouldRepositionToCenter && CursorLockMode.None == Cursor.lockState )
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		if ( GameManager.GameRunning )
			if ( !HeroCamScript.onVantage && HeroCamScript.onHero && StateID.STATE_SHOP !=
				ApplicationManager.Instance.GetAppState() )
			{
				if ( !GameManager.Tutorial || !heroCamDisabler.disabledCameraMovement )
				{
					transform.Rotate( transform.up, ( currMousePos - prevMousePos ) * 0.5f );
					currentRot += ( currMousePos - prevMousePos ) * 0.5f;
				}
				tValue += Time.deltaTime * 2.5f;
				if ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) || Input.
					GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow ) || Input.
					GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow ) || Input.
					GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow ) )
					if ( !moving )
					{
						tValue = 0.0f;
						moving = true;
					}
				if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) || Input.GetKey(
					KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) || Input.GetKey( KeyCode.S ) ||
					Input.GetKey( KeyCode.DownArrow ) || Input.GetKey( KeyCode.D ) || Input.GetKey(
						KeyCode.RightArrow ) )
					info.Deidle();
				else
					moving = false;
				newVel = Vector3.zero;
				if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) )
					newVel += transform.forward;
				if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) )
					newVel -= transform.forward;
				if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) )
					newVel += transform.right;
				if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) )
					newVel -= transform.right;
				newVel = newVel.normalized * Time.deltaTime * agent.speed;
				agent.velocity = Vector3.Lerp( Vector3.zero, newVel, tValue );
			}
		if ( CursorLockMode.Locked == Cursor.lockState )
		{
			prevMousePos = Screen.width * 0.5f;
			HeroCamScript.MouseVertical = Screen.height * 0.5f;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			prevMousePos = Input.mousePosition.x;
			HeroCamScript.MouseVertical = Input.mousePosition.y;
		}
		rot = transform.rotation.eulerAngles;
		rot.y = currentRot;
		transform.rotation = Quaternion.Euler( rot );
	}
	private void SetState( MOVE_State _state )
	{
		switch ( _state )
		{
			case MOVE_State.COMBAT_STATE:
			case MOVE_State.ENGAGE_STATE:
			case MOVE_State.DISENGAGE_STATE:
				m_state = _state;
				break;
			default:
				m_prevState = m_state;
				m_state = _state;
				break;
		}
	}
	private bool CheckInput()
	{
		m_AttMvKey = Input.GetKey( KeyCode.A );
		if ( Input.GetMouseButtonDown( 1 ) )
		{
			if ( m_AttMvKey )
			{
				if ( CameraControl.instance.CameraFollowsPlayer )
				{
					if ( rayHit )
					{
						rayHit = false;
						if ( agent.enabled )
							agent.destination = hit.point;
						m_attackMOve = true;
						SetState( MOVE_State.MOVE_STATE );
					}
					else if ( Physics.Raycast( CameraControl.Current.ScreenPointToRay( Input.
						mousePosition ), out hit ) )
					{
						if ( agent.enabled )
							agent.destination = hit.point;
						m_attackMOve = true;
						SetState( MOVE_State.MOVE_STATE );
					}
					return true;
				}
			}
			else
			{
				if ( CameraControl.instance.CameraFollowsPlayer )
				{
					if ( rayHit )
					{
						rayHit = false;
						m_attackMOve = false;
						if ( agent.enabled )
							agent.destination = hit.point;
						SetState( MOVE_State.MOVE_STATE );
					}
					else if ( Physics.Raycast( CameraControl.Current.ScreenPointToRay( Input.
						mousePosition ), out hit ) )
					{
						if ( agent.enabled )
							agent.destination = hit.point;
						m_attackMOve = false;
						SetState( MOVE_State.MOVE_STATE );
					}
					return true;
				}
			}
		}
		return false;
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public class HeroMovement : MovementScript
{
	NavMeshAgent agent;
	[SerializeField]
	bool m_AttMvKey = false;


	enum MOVE_State { COMBAT_STATE, MOVE_STATE, IDLE_STATE, ENGAGE_STATE, DISENGAGE_STATE };

	[SerializeField]
	MOVE_State m_state, m_prevState;

	HeroInfo info;

	protected override void Start()
	{
		base.Start();
		agent = GetComponent<NavMeshAgent>();
		m_state = m_prevState = MOVE_State.IDLE_STATE;
		info = GetComponent<HeroInfo>();
		TPStart();
	}

	void SetState( MOVE_State _state )
	{
		switch ( _state )
		{
			case MOVE_State.COMBAT_STATE:
			case MOVE_State.ENGAGE_STATE:
			case MOVE_State.DISENGAGE_STATE:
				m_state = _state;
				break;
			case MOVE_State.MOVE_STATE:
			case MOVE_State.IDLE_STATE:
			default:
				m_prevState = m_state;
				m_state = _state;
				break;
		}
	}

	void Update()
	{
		
		TPUpdate();
		if ( HeroCamScript.onHero )
			return;
		if ( GameManager.GameRunning )
		{

			switch ( m_state )
			{
				case MOVE_State.MOVE_STATE:
					if ( agent.enabled )
						agent.Resume();
					if ( CheckInput() || m_attackMOve )
					{
						//agent.destination = hit.point;
						if ( inCombat && m_attackMOve )
							SetState( MOVE_State.ENGAGE_STATE );
					}

					if ( agent.enabled && agent.remainingDistance <= 0.0f )
						SetState( MOVE_State.IDLE_STATE );
					break;

				case MOVE_State.COMBAT_STATE:
					if ( !CheckInput() || m_attackMOve )
					{
						if ( inCombat && TargetPosition != null )
						{
							if ( Vector3.Distance( transform.position, TargetPosition.position ) > combatRange )
							{
								//agent.destination = TargetPosition.position;
								if ( agent.enabled )
									agent.Resume();
								withinRange = false;

								//Debug.Log("Chasing");
							}
							else
							{
								if ( agent.enabled )
									agent.Stop();
								withinRange = true;
								//Debug.Log("not chasing");
							}
						}
						else
							SetState( MOVE_State.DISENGAGE_STATE );
					}
					break;
				case MOVE_State.IDLE_STATE:
					m_attackMOve = true;
					if ( !CheckInput() )
					{

						if ( CameraControl.instance.CameraFollowsPlayer && inCombat )
						{
							SetState( MOVE_State.ENGAGE_STATE );
						}
						else
						{
							if ( inCombat && TargetPosition != null )
							{
								if ( Vector3.Distance( transform.position, TargetPosition.position ) > combatRange )
								{
									withinRange = false;
								}
								else
								{
									withinRange = true;
								}
							}
						}
					}
					break;
				case MOVE_State.ENGAGE_STATE:
					if ( agent.enabled )
						agent.destination = TargetPosition.position;
					withinRange = false;
					SetState( MOVE_State.COMBAT_STATE );
					break;
				case MOVE_State.DISENGAGE_STATE:
					if ( agent.enabled )
						agent.destination = hit.point;
					inCombat = false;
					withinRange = false;
					SetState( m_prevState );
					m_prevState = m_state;
					break;
				default:
					break;
			}
		}
#region OLD
		//if ( Input.GetMouseButtonDown( 1 ) )
		//{
		//    if (CameraControl.instance.CameraFollowsPlayer)
		//    {
		//        if (rayHit)
		//        {
		//            //Debug.Log(rayHit);
		//            rayHit = false;
		//            agent.destination = hit.point;
		//        }
		//        else if (Physics.Raycast(CameraControl.Current.ScreenPointToRay(Input.mousePosition), out hit))
		//        {
		//            agent.destination = hit.point;
		//            SetState(MOVE_State.COMMAND_STATE);
		//        }
		//    }
		//    else
		//        agent.destination = hit.point;
		//
		//}
#endregion
	}


	bool CheckInput()
	{
		m_AttMvKey = Input.GetKey( KeyCode.A );
		if ( Input.GetMouseButtonDown( 1 ) )
		{
			if ( m_AttMvKey )
			{
				if ( CameraControl.instance.CameraFollowsPlayer )
				{
					if ( rayHit )
					{
						//Debug.Log(rayHit);
						rayHit = false;
						if ( agent.enabled )
							agent.destination = hit.point;
						m_attackMOve = true;
						SetState( MOVE_State.MOVE_STATE );
					}
					else if ( Physics.Raycast( CameraControl.Current.ScreenPointToRay( Input.mousePosition ), out hit ) )
					{
						if ( agent.enabled )
							agent.destination = hit.point;
						m_attackMOve = true;
						SetState( MOVE_State.MOVE_STATE );
					}
					return true;
				}
				//else
				//    agent.destination = hit.point;

			}
			else
			{
				if ( CameraControl.instance.CameraFollowsPlayer )
				{
					if ( rayHit )
					{
						//Debug.Log(rayHit);
						rayHit = false;
						m_attackMOve = false;
						if ( agent.enabled )
							agent.destination = hit.point;
						SetState( MOVE_State.MOVE_STATE );
					}
					else if ( Physics.Raycast( CameraControl.Current.ScreenPointToRay( Input.mousePosition ), out hit ) )
					{
						if ( agent.enabled )
							agent.destination = hit.point;
						m_attackMOve = false;
						SetState( MOVE_State.MOVE_STATE );
					}
					return true;
				}
				//else
				//    agent.destination = hit.point;
			}
		}
		return false;
	}

	// TP Move
	float prevMousePos, currMousePos, currentRot;
	bool shiftKeyPressed;
	Vector3 rot;

	public bool SprintingAbility { private get; set; }

	void TPStart()
	{
		prevMousePos = currMousePos = Input.mousePosition.x;
		currentRot = transform.rotation.eulerAngles.y;
		SprintingAbility = false;
		agent.angularSpeed = 0.0f;
	}

	Vector3 newVel;
	float tValue = 0.0f;
	bool moving = false;
	void TPUpdate()
	{
		shiftKeyPressed = Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift );
		agent.speed = ( ( SprintingAbility ) ?
			( shiftKeyPressed ? 25000.0f : 21500.0f ) :
			( shiftKeyPressed ? 16000.0f : 10500.0f ) );
		currMousePos = Input.mousePosition.x;
		bool shouldRepositionToCenter = HeroCamScript.onHero && GameManager.GameRunning
			&& !heroCamDisabler.disabledCameraMovement
			&& StateID.STATE_SHOP != ApplicationManager.Instance.GetAppState();
		// <BUGFIX: Test Team #2>
		//if ( Mathf.Abs( Screen.width * 0.5f - currMousePos ) >= ( Screen.width * 0.499f ) )
		if ( Mathf.Abs( Screen.width * 0.5f - currMousePos ) >= ( Screen.width * 0.2f )
		// </BUGFIX: Test Team #2>
		|| Mathf.Abs( Screen.height * 0.5f - HeroCamScript.MouseVertical ) >= ( Screen.height * 0.2f ) )
		{
			Cursor.lockState = shouldRepositionToCenter ? CursorLockMode.Locked : CursorLockMode.None;
			if ( shouldRepositionToCenter && Cursor.lockState == CursorLockMode.None )
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		// <BUGFIX: Test Team #5>
		//Cursor.visible = true;//( !HeroCamScript.onHero || !GameManager.GameRunning );
		// </BUGFIX: Test Team #5>
		if ( GameManager.GameRunning )
		{
			// <Bugfix: Test Team #29>
			//if ( HeroCamScript.onHero && ApplicationManager.Instance.GetAppState() != StateID.STATE_SHOP )
			if (!HeroCamScript.onVantage && HeroCamScript.onHero && ApplicationManager.Instance.GetAppState() != StateID.STATE_SHOP )
			// </Bugfix: Test Team #29>
			{
				// <BUGFIX: Dev Team #16>
				if ( !GameManager.Tutorial || !heroCamDisabler.disabledCameraMovement ) {
				// </BUGFIX: Dev Team #16>
					transform.Rotate( transform.up, ( currMousePos - prevMousePos ) * 0.5f );
					currentRot += ( currMousePos - prevMousePos ) * 0.5f; }
				tValue += Time.deltaTime * 2.5f;
				if ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) ||
					Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow ) ||
					Input.GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow ) ||
					Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow ) )
					if ( !moving )
					{
						tValue = 0.0f;
						moving = true;
					}
				if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) ||
					Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) ||
					Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) ||
					Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) )
					info.Deidle();
				else
					moving = false;
				newVel = Vector3.zero;
				if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) )
					newVel += transform.forward;
				if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) )
					newVel -= transform.forward;
				if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) )
					newVel += transform.right;
				if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) )
					newVel -= transform.right;
				newVel = newVel.normalized * Time.deltaTime * agent.speed;
				agent.velocity = Vector3.Lerp( Vector3.zero, newVel, tValue );
			}
		}

		if ( CursorLockMode.Locked == Cursor.lockState )
		{
			prevMousePos = Screen.width * 0.5f;
			HeroCamScript.MouseVertical = Screen.height * 0.5f;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			prevMousePos = Input.mousePosition.x;
			HeroCamScript.MouseVertical = Input.mousePosition.y;
		}

		rot = transform.rotation.eulerAngles;
		rot.y = currentRot;
		transform.rotation = Quaternion.Euler( rot );
	}
	NavMeshHit nmhit;
	public void ResetToSpawn()
	{
		NavMesh.SamplePosition( GameManager.blueHeroSpawnPosition, out nmhit, 5.0f, NavMesh.AllAreas );
		transform.position = new Vector3(
			nmhit.position.x,
			agent.baseOffset * transform.localScale.y,
			nmhit.position.z
			);
		// <BUGFIX: Test Team #22>
		transform.rotation = new Quaternion( 0.0f, 0.707106781f, 0.0f, 0.707106781f );
		currentRot = transform.rotation.eulerAngles.y;
		// </BUGFIX: Test Team #22>
	}
}
#endif
#endregion //OLD_CODE