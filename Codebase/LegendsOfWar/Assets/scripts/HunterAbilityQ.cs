using UnityEngine;
public class HunterAbilityQ : AbilityQBase
{
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		heroInfo.Damage += 20;
		GetComponent<ParticleSystem>().Play();
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		heroInfo.Damage -= 20;
		GetComponent<ParticleSystem>().Stop();
		GetComponent<ParticleSystem>().Clear();
	}
}