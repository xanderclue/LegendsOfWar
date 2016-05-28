using UnityEngine;
public class MovementScript : MonoBehaviour
{
	[SerializeField]
	protected bool inCombat = false;
	[SerializeField]
	protected bool m_attackMOve = false;
	protected bool withinRange = false;
	protected float combatRange = 0;
	protected RaycastHit hit;
	protected bool rayHit = false;
	private Transform targetPosition;
	public bool CanEngage
	{ get { return m_attackMOve; } }
	public bool WithinRange
	{ get { return withinRange; } }
	public bool InCombat
	{ get { return inCombat; } }
	protected Transform TargetPosition
	{ get { return targetPosition; } }

	protected virtual void Start()
	{
		GameManager.Hud.GrabHit += MovementScript_GrabHit;
	}
	private void OnDestoy()
	{
		GameManager.Hud.GrabHit -= MovementScript_GrabHit;
	}
	private void MovementScript_GrabHit( RaycastHit _hit )
	{
		hit = _hit;
		rayHit = true;
	}
	public void SetTarget( Transform target, float distance )
	{
		targetPosition = target;
		combatRange = distance;
		inCombat = true;
	}
	public void Disengage()
	{
		inCombat = false;
	}
}