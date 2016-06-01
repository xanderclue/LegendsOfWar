using UnityEngine;
public class TankAbilityW : AbilityWBase
{
	public bool skillON = false;
	public float Wdamage;
	public CollisionDetector coll;
	private ParticleSystem AbilityWParticle;
	private HeroMovement movement;
	protected override void Start()
	{
		base.Start();
		AbilityWParticle = GameObject.FindGameObjectWithTag( "PW" ).GetComponent<ParticleSystem>();
		AbilityWParticle.Stop();
		movement = heroInfo.GetComponent<HeroMovement>();
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		skillON = true;
		AbilityWParticle.transform.position = new Vector3( heroInfo.transform.position.x, 1.0f,
			heroInfo.transform.position.z );
		AbilityWParticle.Play();
		movement.SprintingAbility = true;
		coll.DealDamage( CCharge );
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		movement.SprintingAbility = false;
		skillON = false;
		AbilityWParticle.transform.localPosition += new Vector3( 0.0f, -10.0f );
		AbilityWParticle.Stop();
		AbilityWParticle.Clear();
	}
	private void CCharge( Info entity )
	{
		if ( entity )
			if ( entity is MinionInfo )
				entity.TakeDamage( Wdamage );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE