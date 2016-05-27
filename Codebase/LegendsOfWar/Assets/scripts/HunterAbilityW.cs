using UnityEngine;
public class HunterAbilityW : AbilityWBase
{
	[SerializeField]
	private float range = 0.0f, speed = 0.0f, damage = 0.0f;
	[SerializeField]
	private GameObject projectile = null, arrowSpawn = null;
	[SerializeField]
	private GameObject Icon = null;
	private GameObject activeIcon = null;
	private Info target = null;
	private float originalSpeed = 0.0f;
	private RaycastHit hit;
	protected override void Update()
	{
		skillTimer -= Time.deltaTime;
		if ( abilityOn && skillTimer <= 0.0f )
			AbilityDeactivate();
		if ( EnoughMana )
			if ( ( ( Input.GetKeyDown( KeyCode.W ) && !HeroCamScript.onHero ) || Input.GetKeyDown(
				KeyCode.Alpha2 ) || Input.GetKeyDown( KeyCode.Keypad2 ) ) && cooldownTimer <= 0.0f )
				if ( TargetSelected() )
					TryCast();
		if ( !target && activeIcon && activeIcon.GetComponent<SpriteRenderer>().enabled )
			activeIcon.GetComponent<SpriteRenderer>().enabled = false;
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		StopTarget();
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		if ( target )
			target.GetComponent<NavMeshAgent>().speed = originalSpeed;
		if ( activeIcon && activeIcon.GetComponent<SpriteRenderer>() )
			activeIcon.GetComponent<SpriteRenderer>().enabled = false;
	}
	private bool TargetSelected()
	{
		Ray ray = new Ray( transform.position, transform.forward );
		if ( Physics.SphereCast( ray, 5.0f, out hit, range, 9, QueryTriggerInteraction.Collide ) )
			if ( hit.collider.GetComponent<NavMeshAgent>() && hit.collider.GetComponent<Info>() &&
				hit.collider.GetComponent<Info>().team != GetComponentInParent<Info>().team )
			{
				target = hit.collider.gameObject.GetComponent<Info>();
				return true;
			}
		return false;
	}
	private void StopTarget()
	{
		originalSpeed = target.GetComponent<NavMeshAgent>().speed;
		target.GetComponent<NavMeshAgent>().speed = 0.0f;
		activeIcon = ( Instantiate( Icon, target.transform.position, target.transform.rotation ) as
			GameObject );
		activeIcon.transform.parent = target.transform;
		activeIcon.GetComponent<SpriteRenderer>().enabled = true;
		ProjectileBehaviour p = ( Instantiate( projectile, arrowSpawn.transform.position, arrowSpawn
			.transform.rotation ) as GameObject ).GetComponent<ProjectileBehaviour>();
		p.speed = speed;
		p.damage = damage;
		p.target = hit.transform;
		p.Fire();
	}
}