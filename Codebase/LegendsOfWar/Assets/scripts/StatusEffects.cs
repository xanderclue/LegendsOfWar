using UnityEngine;
using System.Collections.Generic;
public class StatusEffects : MonoBehaviour
{
	private IList<Effect> m_stats;
	private Info infoTemp;
	private MovementScript movementTemp;
	private AttackScript attackTemp;
	private NavMeshAgent agentTemp;
	private string instanceString;
	private bool isMinion = false, isHero = false;
	public static void Inflict( GameObject _target, Effect _effect )
	{
		StatusEffectsManager.Instance.AddStatus( _target.GetInstanceID().ToString(), _effect );
	}
	public void Apply( Effect _effect )
	{
		if ( _effect.Expired( Time.deltaTime ) )
			RemoveExpired( _effect );
		else
			switch ( _effect.m_type )
			{
				case StatusEffectType.DOT:
					if ( _effect.Ticked( Time.deltaTime ) )
						if ( _effect.m_stackable )
							infoTemp.TakeDamage( _effect.m_damage * ( 1 + _effect.m_stacks / 3 ) *
								_effect.m_tickRate );
						else
							infoTemp.TakeDamage( _effect.m_damage * _effect.m_tickRate );
					break;
				case StatusEffectType.STUN:
					if ( isMinion )
						agentTemp.enabled = movementTemp.enabled = attackTemp.enabled = false;
					break;
				case StatusEffectType.SLOW:
					if ( isMinion )
						agentTemp.speed = ( infoTemp as MinionInfo ).MovementSpeed * ( 1.0f -
							_effect.m_percentage * 0.01f );
					else if ( isHero )
						agentTemp.speed *= 0.5f;
					break;
				case StatusEffectType.SNARE:
					if ( isMinion )
					{
						agentTemp.Stop();
						agentTemp.speed = 0.0f;
					}
					break;
				case StatusEffectType.DMG_Damp:
					infoTemp.DmgDamp = _effect.m_percentage * 0.01f;
					break;
				default:
					break;
			}
	}
	private void Awake()
	{
		instanceString = gameObject.GetInstanceID().ToString();
	}
	private void Start()
	{
		infoTemp = GetComponent<Info>();
		movementTemp = GetComponent<MovementScript>();
		attackTemp = GetComponent<AttackScript>();
		agentTemp = GetComponent<NavMeshAgent>();
		if ( infoTemp )
		{
			isMinion = infoTemp is MinionInfo;
			isHero = infoTemp is HeroInfo;
		}
	}
	private void Update()
	{
		m_stats = StatusEffectsManager.Instance.GetMyStatus( instanceString );
		if ( null != m_stats )
			for ( int i = 0; i < m_stats.Count; ++i )
				Apply( m_stats[ i ] );
	}
	private void RemoveExpired( Effect _effect )
	{
		switch ( _effect.m_type )
		{
			case StatusEffectType.STUN:
				if ( isMinion )
				{
					agentTemp.enabled = movementTemp.enabled = attackTemp.enabled = true;
					agentTemp.Resume();
				}
				break;
			case StatusEffectType.SLOW:
				if ( isMinion )
					agentTemp.speed = ( infoTemp as MinionInfo ).MovementSpeed;
				else if ( isHero )
					agentTemp.speed *= 2.0f;
				break;
			case StatusEffectType.SNARE:
				if ( gameObject.GetComponent<MinionInfo>() )
				{
					agentTemp.speed = ( infoTemp as MinionInfo ).MovementSpeed;
					agentTemp.Resume();
				}
				break;
			case StatusEffectType.DMG_Damp:
				infoTemp.DmgDamp = 0.0f;
				break;
			default:
				break;
		}
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE