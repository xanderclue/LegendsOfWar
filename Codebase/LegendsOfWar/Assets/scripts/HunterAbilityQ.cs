using UnityEngine;
public class HunterAbilityQ : AbilityQBase
{
	private ParticleSystem ps;
	protected override void Start()
	{
		base.Start();
		ps = GetComponent<ParticleSystem>();
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		heroInfo.Damage += 20.0f;
		ps.Play();
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		heroInfo.Damage -= 20.0f;
		ps.Stop();
		ps.Clear();
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public class HunterAbilityQ : AbilityQBase
{
    //Hunter's Mark
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

#endif
#endregion //OLD_CODE