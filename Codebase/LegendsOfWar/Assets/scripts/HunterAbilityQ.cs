using UnityEngine;

public class HunterAbilityQ : AbilityQBase
{
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		GetComponentInParent<HeroInfo>().Damage += 20;
		GetComponent<ParticleSystem>().Play();
	}

	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		GetComponentInParent<HeroInfo>().Damage -= 20;
		GetComponent<ParticleSystem>().Stop();
		GetComponent<ParticleSystem>().Clear();

	}
}
