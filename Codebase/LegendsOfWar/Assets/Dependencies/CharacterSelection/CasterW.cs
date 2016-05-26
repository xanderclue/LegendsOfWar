using UnityEngine;
using System.Collections;

public class CasterW : AbilityWBase {
    [SerializeField]
    GameObject m_zone = null;


    protected override void Start()
    {
        base.Start();
//        m_effect.m_name = "TheZone";
//        m_effect.m_type = StatusEffectType.DOT;
//        m_effect.m_damage = 20;
//        m_effect.m_duration = 1;
        m_zone.GetComponent<ParticleSystem>().Stop();
		m_zone.GetComponent<ParticleSystem>().Clear();

    }

    protected override void AbilityActivate()
    {
		CasterWZoneBehavior tmp = (Instantiate(m_zone, transform.position, transform.rotation)
			as GameObject).GetComponent<CasterWZoneBehavior>();
        tmp.Activate = true;
        tmp.zoneDuration = m_effect.m_duration;
		//tmp.m_effect = m_effect.CreateEffect();
		base.AbilityActivate();
    }
}
