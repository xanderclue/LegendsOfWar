using UnityEngine;
public class MovementScript : MonoBehaviour
{
	[SerializeField]
	protected bool inCombat = false;
	[SerializeField]
	protected bool m_attackMOve = false;

	protected bool withinRange = false;
	protected float combatRange = 0;
	private Transform targetPosition;
	protected RaycastHit hit;
	protected bool rayHit = false;
	public bool CanEngage { get { return m_attackMOve; } }
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
	protected Transform TargetPosition { get { return targetPosition; } }
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
	public bool WithinRange { get { return withinRange; } }
	public bool InCombat { get { return inCombat; } }
}