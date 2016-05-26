using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour
{
	[SerializeField]
	protected bool inCombat = false;
	protected bool withinRange = false;
	protected float combatRange = 0;
	Transform targetPosition;
	protected RaycastHit hit;
	protected bool rayHit = false;
	[SerializeField]
	protected bool m_attackMOve = false;

	public bool CanEngage
	{
		get { return m_attackMOve; }
	}

	protected virtual void Start()
	{
		GameManager.Hud.GrabHit += MovementScript_GrabHit;
	}

	void OnDestoy() { GameManager.Hud.GrabHit -= MovementScript_GrabHit; }

	void MovementScript_GrabHit( RaycastHit _hit )
	{
		hit = _hit;
		rayHit = true;
	}

	protected Transform TargetPosition
	{
		get { return targetPosition; }
	}

	public void SetTarget( Transform target, float distance = 0 )
	{
		targetPosition = target;
		combatRange = distance;
		inCombat = true;
	}

	public void Disengage()
	{
		inCombat = false;
	}

	public bool WithinRange { get { return withinRange; } }
	public bool InCombat { get { return inCombat; } }


}