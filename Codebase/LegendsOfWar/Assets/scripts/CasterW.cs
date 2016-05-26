using UnityEngine;

public class CasterW : AbilityWBase
{
	[SerializeField]
	GameObject m_zone = null;


	protected override void Start()
	{
		base.Start();
		m_zone.GetComponent<ParticleSystem>().Stop();
		m_zone.GetComponent<ParticleSystem>().Clear();

	}

	protected override void AbilityActivate()
	{
		CasterWZoneBehavior tmp = ( Instantiate( m_zone, transform.position, transform.rotation )
			as GameObject ).GetComponent<CasterWZoneBehavior>();
		tmp.Activate = true;
		tmp.zoneDuration = m_effect.m_duration;
		base.AbilityActivate();
	}
}
