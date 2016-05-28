using UnityEngine;
public class SupportAbilityE : AbilityEBase
{
	private SupportRange supprang;
	private ParticleSystem ps;
	protected override void Start()
	{
		base.Start();
		ps = GetComponentInChildren<ParticleSystem>();
		supprang = heroInfo.GetComponentInChildren<SupportRange>();
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		supprang.ApplyToEnemiesInRange( Cyclone );
		ps.Play();
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		ps.Stop();
		ps.Clear();
	}
	private void Cyclone( Info entity )
	{
		if ( entity )
			if ( entity.team != heroInfo.team )
				if ( entity is MinionInfo )
				{
					entity.TakeDamage( 10.0f );
					( entity as MinionInfo ).Cyclone();
				}
	}
}