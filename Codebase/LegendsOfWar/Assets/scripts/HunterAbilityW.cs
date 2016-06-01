using UnityEngine;
public class HunterAbilityW : AbilityWBase
{
	[SerializeField]
	private float range = 0.0f, speed = 0.0f, damage = 0.0f;
	[SerializeField]
	private GameObject projectile = null, arrowSpawn = null, Icon = null;
	private GameObject activeIcon = null;
	private SpriteRenderer activeIconSpriteRenderer = null;
	private NavMeshAgent targNm = null, colNm;
	private Info target = null, colInfo;
	private RaycastHit hit;
	private float originalSpeed = 0.0f;
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
		if ( !target && activeIcon && activeIconSpriteRenderer.enabled )
			activeIconSpriteRenderer.enabled = false;
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		originalSpeed = targNm.speed;
		targNm.speed = 0.0f;
		activeIcon = Instantiate( Icon, target.transform.position, target.transform.rotation ) as
			GameObject;
		activeIconSpriteRenderer = activeIcon.GetComponent<SpriteRenderer>();
		activeIcon.transform.parent = target.transform;
		activeIconSpriteRenderer.enabled = true;
		ProjectileBehaviour p = ( Instantiate( projectile, arrowSpawn.transform.position, arrowSpawn
			.transform.rotation ) as GameObject ).GetComponent<ProjectileBehaviour>();
		p.speed = speed;
		p.damage = damage;
		p.target = hit.transform;
		p.Fire();
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		if ( target )
			targNm.speed = originalSpeed;
		if ( activeIconSpriteRenderer )
			activeIconSpriteRenderer.enabled = false;
	}
	private bool TargetSelected()
	{
		Ray ray = new Ray( transform.position, transform.forward );
		if ( Physics.SphereCast( ray, 5.0f, out hit, range, 9, QueryTriggerInteraction.Collide ) )
		{
			colInfo = hit.collider.GetComponent<Info>();
			colNm = hit.collider.GetComponent<NavMeshAgent>();
			if ( colNm && colInfo && colInfo.team != heroInfo.team )
			{
				target = colInfo;
				targNm = colNm;
				return true;
			}
		}
		return false;
	}
}