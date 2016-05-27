using UnityEngine;
public class TankAbilityW : AbilityWBase
{
	private GameObject AbilityWParticle;
	public bool skillON = false;
	public float Wdamage;
	private HeroMovement movement;
	public CollisionDetector coll;
	protected override void Start()
	{
		base.Start();
		AbilityWParticle = GameObject.FindGameObjectWithTag( "PW" );
		AbilityWParticle.GetComponent<ParticleSystem>().Stop();
		movement = GetComponentInParent<HeroMovement>();
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		skillON = true;
		AbilityWParticle.transform.position = new Vector3( heroInfo.transform.position.x, 1.0f,
			heroInfo.transform.position.z );
		AbilityWParticle.GetComponent<ParticleSystem>().Play();
		movement.SprintingAbility = true;
		coll.DealDamage( CCharge );
	}
	protected override void AbilityDeactivate()
	{
		movement.SprintingAbility = false;
		base.AbilityDeactivate();
		skillON = false;
		AbilityWParticle.transform.localPosition -= new Vector3( 0.0f, 10.0f );
		AbilityWParticle.GetComponent<ParticleSystem>().Stop();
		AbilityWParticle.GetComponent<ParticleSystem>().Clear();
	}
	private void CCharge( Info entity )
	{
		if ( entity )
			if ( entity is MinionInfo )
				entity.TakeDamage( Wdamage );
	}
}