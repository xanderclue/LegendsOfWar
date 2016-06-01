using UnityEngine;
public class MovementScript : MonoBehaviour
{
	[SerializeField]
	protected bool inCombat = false, m_attackMOve = false;
	protected RaycastHit hit;
	protected float combatRange = 0.0f;
	protected bool withinRange = false, rayHit = false;
	private Transform targetPosition;
	public bool WithinRange
	{ get { return withinRange; } }
	public bool InCombat
	{ get { return inCombat; } }
	protected Transform TargetPosition
	{ get { return targetPosition; } }
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
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE