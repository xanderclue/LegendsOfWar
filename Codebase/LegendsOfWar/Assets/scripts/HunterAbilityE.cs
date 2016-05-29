using UnityEngine;
public class HunterAbilityE : AbilityEBase
{
	[SerializeField]
	private float range = 0.0f, speed = 0.0f, abilityAdditionalDamage = 0.0f;
	[SerializeField]
	private GameObject projectile = null, arrowSpawn = null, visualTarget = null;
	private RaycastHit hit;
	private bool aiming = false;
	protected override void Update()
	{
		Vector3 pos = transform.position + transform.forward * 50.0f;
		visualTarget.transform.position = pos;
		skillTimer -= Time.deltaTime;
		if ( abilityOn && skillTimer <= 0.0f )
			AbilityDeactivate();
		if ( !GameManager.GameRunning || !abilityEnabled )
			return;
		if ( EnoughMana )
			if ( !aiming && cooldownTimer <= 0.0f )
			{
				if ( ( Input.GetKeyDown( KeyCode.E ) && !HeroCamScript.onHero ) || Input.GetKeyDown(
					KeyCode.Alpha3 ) || Input.GetKeyDown( KeyCode.Keypad3 ) )
				{
					aiming = true;
					GetComponent<LineRenderer>().enabled = true;
				}
			}
		if ( aiming && cooldownTimer <= 0.0f )
		{
			Vector3[ ] vecRange = new Vector3[ ] { transform.parent.position, visualTarget.transform
				.position };
			GetComponent<LineRenderer>().SetPositions( vecRange );
			if ( Input.GetMouseButtonDown( 0 ) )
			{
				Fire();
				aiming = false;
				GetComponent<LineRenderer>().enabled = false;
			}
			else if ( Input.GetMouseButtonDown( 1 ) )
			{
				aiming = false;
				GetComponent<LineRenderer>().enabled = false;
			}
		}
	}
	private void Fire()
	{
		if ( Physics.SphereCast( new Ray( transform.position, transform.forward ), 5.0f, out hit,
			range, 9, QueryTriggerInteraction.Collide ) )
			if ( hit.collider.GetComponent<Info>() && hit.collider.GetComponent<Info>().team !=
				heroInfo.team )
			{
				ProjectileBehaviour p = ( Instantiate( projectile, arrowSpawn.transform.position,
					arrowSpawn.transform.rotation ) as GameObject ).GetComponent<ProjectileBehaviour
					>();
				p.speed = speed;
				p.damage = GetComponentInParent<HeroInfo>().Damage + abilityAdditionalDamage;
				p.target = hit.transform;
				p.Fire();
				TryCast();
			}
	}
}