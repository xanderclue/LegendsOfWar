using UnityEngine;

public class TankAbilityW : AbilityWBase
{
	GameObject AbilityWParticle;

	public bool skillON = false;
	public float Wdamage;

	HeroMovement movement;

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
		AbilityWParticle.GetComponent<Transform>().localPosition = new Vector3( GameObject.FindGameObjectWithTag( "Hero" ).GetComponent<Transform>().localPosition.x, 1, GameObject.FindGameObjectWithTag( "Hero" ).GetComponent<Transform>().localPosition.z );
		AbilityWParticle.GetComponent<ParticleSystem>().Play();

		movement.SprintingAbility = true;
		//if (coll.metEnemy == true)
		//{
		//    coll.metEnemy = false;
		//    AbilityDeactivate();
		//}
		coll.DealDamage( CCharge );

	}


	protected override void AbilityDeactivate()
	{
		movement.SprintingAbility = false;
		base.AbilityDeactivate();
		skillON = false;
		AbilityWParticle.GetComponent<Transform>().localPosition -= new Vector3( 0, 10 );
		AbilityWParticle.GetComponent<ParticleSystem>().Stop();
		AbilityWParticle.GetComponent<ParticleSystem>().Clear();

	}

	void CCharge( Info entity )
	{
		if ( entity )
			if ( entity is MinionInfo )
			{
				entity.TakeDamage( Wdamage );
			}
	}
}