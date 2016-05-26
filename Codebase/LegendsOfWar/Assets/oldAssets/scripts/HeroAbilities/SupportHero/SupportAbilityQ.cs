using UnityEngine;
public class SupportAbilityQ : AbilityQBase
{
	ParticleSystem ps;
	SupportRange supprang;
	protected override void Start()
	{
		base.Start();
		ps = GetComponentInChildren<ParticleSystem>();
		supprang = heroInfo.GetComponentInChildren<SupportRange>();
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		supprang.ApplyToAlliesInRange( SoothingAura );
		ps.Play();
	}
	void SoothingAura( Info entity )
	{
		if ( entity is MinionInfo )
			entity.HP += 10.0f;
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		ps.Stop();
		ps.Clear();
	}
}