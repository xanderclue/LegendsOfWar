using UnityEngine;

enum Move_State { LANING_STATE, COMBAT_STATE, COMMAND_STATE, IDLE_STATE, ENGAGE_STATE, DISENGAGE_STATE };

public enum Path { NORTH_PATH = 577, SOUTH_PATH = 1153, CENTER_PATH = 289, NULL = 1, ANY_PATH = 2023 };


public class MinionMovement : MovementScript
{
	NavMeshAgent agent;
	public Transform goal;
	bool followingNav = true;
	LineRenderer line;

	MinionInfo info;
	Interactive interactive;

	[SerializeField]
	Move_State m_state, m_prevState;
	Path m_path;

	void SetState( Move_State _state )
	{
		switch ( _state )
		{
			case Move_State.COMBAT_STATE:
			case Move_State.ENGAGE_STATE:
			case Move_State.DISENGAGE_STATE:
				m_state = _state;
				break;
			case Move_State.LANING_STATE:
			case Move_State.COMMAND_STATE:
			case Move_State.IDLE_STATE:
			default:
				m_prevState = m_state;
				m_state = _state;
				break;
		}
	}
	SkinnedMeshRenderer temp_smr;
	protected override void Start()
	{
		base.Start();
		Start2();
		agent.speed = info.MovementSpeed;
		if ( info.type == MinionClass.SIEGE_MINION )
			return;
		line = gameObject.AddComponent<LineRenderer>();
		temp_smr = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		if ( temp_smr )
			line.material = temp_smr.material;
	}
	void Start2()
	{
		agent = GetComponent<NavMeshAgent>();

		m_state = m_prevState = Move_State.LANING_STATE;
		info = GetComponent<MinionInfo>();
		interactive = GetComponent<Interactive>();

		if ( Team.RED_TEAM == info.team )
		{
			goal = GameManager.BluePortalTransform;
			if ( agent.enabled )
				agent.destination = goal.position;
		}
		else
		{
			goal = GameManager.RedPortalTransform;
		}
	}

	void Update()
	{

		if ( GameManager.GameRunning )
		{
			switch ( m_state )
			{
				case Move_State.LANING_STATE:
					agent.enabled = true;
					if ( agent.pathPending )
						break;
					if ( info.type != MinionClass.SIEGE_MINION )
						line.enabled = false;
					if ( inCombat )
					{
						SetState( Move_State.ENGAGE_STATE );
					}
					else if ( !followingNav )
					{
						followingNav = true;
						if ( transform.position.z <= GameManager.botSplitZ )
							ChangeLane( Path.SOUTH_PATH );
						else if ( transform.position.z >= GameManager.topSplitZ )
							ChangeLane( Path.NORTH_PATH );
						else
							ChangeLane( Path.CENTER_PATH );

						agent.destination = goal.position;
					}
					if ( !interactive.Selected )
						rayHit = false;
					CheckForInput();
					break;
				case Move_State.COMMAND_STATE:
					if ( agent.pathPending )
						break;
					if ( info.type != MinionClass.SIEGE_MINION )
					{
						line.enabled = true;
						var temp = new Vector3[ ] { transform.localPosition, agent.destination };
						line.SetPositions( temp );
					}
					if ( m_path != Path.ANY_PATH )
						ChangeLane();
					agent.SetDestination( hit.point );
					CheckForInput();
					if ( !agent.pathPending )
						if ( agent.remainingDistance <= 3 )
							SetState( Move_State.IDLE_STATE );
					break;
				case Move_State.COMBAT_STATE:
					if ( info.type != MinionClass.SIEGE_MINION )
						line.enabled = false;
					if ( inCombat && TargetPosition != null )
					{
						if ( Vector3.Distance( transform.position, TargetPosition.position ) > combatRange )
						{
							agent.Resume();
							withinRange = false;
						}
						else
						{
							agent.Stop();
							withinRange = true;
						}
					}
					else
						SetState( Move_State.DISENGAGE_STATE );
					break;
				case Move_State.IDLE_STATE:
					if ( info.type != MinionClass.SIEGE_MINION )
						line.enabled = false;
					if ( CheckForInput() )
						SetState( Move_State.COMMAND_STATE );
					else if ( !interactive.Selected )
						SetState( Move_State.LANING_STATE );
					else if ( InCombat )
						SetState( Move_State.ENGAGE_STATE );
					else
						agent.destination = hit.point;
					break;
				case Move_State.ENGAGE_STATE:
					if ( agent && TargetPosition )
						agent.destination = TargetPosition.position;
					followingNav = false;
					withinRange = false;
					SetState( Move_State.COMBAT_STATE );
					break;
				case Move_State.DISENGAGE_STATE:
					agent.Resume();
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


	public void ChangeLane( Path _newPath = Path.ANY_PATH )
	{
		if ( !followingNav )
		{
			m_path = Path.ANY_PATH;
		}
		else
		{
			m_path = _newPath;
		}
		if ( !agent )
			Start2();
		agent.areaMask = ( int )m_path;
		if ( agent.isPathStale )
		{
			agent.ResetPath();
		}
	}

	bool CheckForInput()
	{
		if ( HeroCamScript.onHero )
			return false;
		if ( interactive.Selected )
		{
			if ( rayHit )
			{
				rayHit = false;
				followingNav = false;
				agent.ResetPath();
				agent.SetDestination( hit.point );
				if ( info.type != MinionClass.SIEGE_MINION )
				{
					var temp = new Vector3[ ] { transform.localPosition, agent.destination };
					line.SetPositions( temp );
				}
				SetState( Move_State.COMMAND_STATE );
				return true;
			}
			else if ( Input.GetMouseButton( 1 ) )
			{
				if ( Physics.Raycast( CameraControl.Current.ScreenPointToRay( Input.mousePosition ), out hit, 1000, 5943 ) )
				{
					followingNav = false;
					agent.ResetPath();
					agent.SetDestination( hit.point );
					if ( info.type != MinionClass.SIEGE_MINION )
					{
						var temp = new Vector3[ ] { transform.localPosition, agent.destination };
						line.SetPositions( temp );
					}
					SetState( Move_State.COMMAND_STATE );
					return true;
				}
			}
		}
		return false;
	}
}