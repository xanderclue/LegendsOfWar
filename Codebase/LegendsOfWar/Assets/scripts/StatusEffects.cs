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
using UnityEngine;
using System.Collections.Generic;

public class StatusEffects : MonoBehaviour {
    IList<Effect> m_stats;

    public IList<Effect> Stats
    {
        get { return m_stats; }
    }

    public void Apply(Effect _effect)
    {
        if (!_effect.Expired(Time.deltaTime))
        {
            switch (_effect.m_type)
            {
                case StatusEffectType.DOT:
                    {
						if (_effect.Ticked(Time.deltaTime)) {
							if (_effect.m_stackable)
								GetComponent<Info>().TakeDamage((_effect.m_damage * (1 + _effect.m_stacks / 3)) * _effect.m_tickRate);
							else
								GetComponent<Info>().TakeDamage(_effect.m_damage * _effect.m_tickRate);

						}
                        Debug.Log(_effect.m_type);
                    }
                    break;
                case StatusEffectType.STUN:
                    {
						if (!gameObject.GetComponent<MinionInfo>())
							break;
                        gameObject.GetComponent<MinionAttack>().enabled = false;
                        gameObject.GetComponent<MinionMovement>().enabled = false;
						gameObject.GetComponent<NavMeshAgent>().enabled = false;
						Debug.Log(_effect.m_type);
                    }
                    break;
                case StatusEffectType.SLOW:
                    {
						if (gameObject.GetComponent<MinionInfo>()) {
							gameObject.GetComponent<NavMeshAgent>().speed =
                            (gameObject.GetComponent<MinionInfo>().MovementSpeed * (1 - _effect.m_percentage * 0.01f));
						}
						else if (gameObject.GetComponent<HeroInfo>()) {
								gameObject.GetComponent<NavMeshAgent>().speed =
								(gameObject.GetComponent<NavMeshAgent>().speed * 0.5f);
							}
                        Debug.Log(_effect.m_type);

                    }
                    break;
                case StatusEffectType.SNARE:
                    {
						if (!gameObject.GetComponent<MinionInfo>())
							break;
						Debug.Log(_effect.m_type);
						gameObject.GetComponent<NavMeshAgent>().Stop();
						gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                    }
                    break;

                case StatusEffectType.NONE:
                    Debug.Log(_effect.m_type + _effect.m_stacks);
                    break;
                case StatusEffectType.DMG_Amp:
                    {
                        gameObject.GetComponent<HeroInfo>().DmgAmp = _effect.m_percentage * 0.01f;
                    }
                    break;
                case StatusEffectType.DMG_Damp:
                    {
                        gameObject.GetComponent<Info>().DmgDamp = _effect.m_percentage * 0.01f;
                    }
                    break;
                default:
                    Debug.LogWarning("StatusEffects.Apply(): Defualt case");
                    break;
            }
        }
        else
        {
            Debug.Log("removing"+_effect.m_name);
            RemoveExpired(_effect);
        }
    }

    void Awake()
    {
        m_stats = StatusEffectsManager.Instance.GetMyStatus(gameObject.GetInstanceID().ToString());
    }


    public static void Inflict(GameObject _target, Effect _effect)
    {
        StatusEffectsManager.Instance.AddStatus(_target.GetInstanceID().ToString(), _effect);
    }

    void Update()
    {
       m_stats = StatusEffectsManager.Instance.GetMyStatus(gameObject.GetInstanceID().ToString());
       if (m_stats != null)
           for (int i = 0; i < m_stats.Count; ++i)
           {
               Apply(m_stats[i]);
           }
    }

    void RemoveExpired(Effect _effect)
    {
        switch (_effect.m_type)
        {
            case StatusEffectType.DOT:
                {
                    
                }
                break;
            case StatusEffectType.STUN:
                {
					if (!gameObject.GetComponent<MinionInfo>())
						break;
                    gameObject.GetComponent<MinionAttack>().enabled = true;
                    gameObject.GetComponent<MinionMovement>().enabled = true;
					gameObject.GetComponent<NavMeshAgent>().enabled = true;
					gameObject.GetComponent<NavMeshAgent>().Resume();

                }
                break;
            case StatusEffectType.SLOW:
                {
                   
					if (gameObject.GetComponent<MinionInfo>()) { 
						gameObject.GetComponent<NavMeshAgent>().speed =
						gameObject.GetComponent<MinionInfo>().MovementSpeed;

					}
					else if (gameObject.GetComponent<HeroInfo>()) {
						gameObject.GetComponent<NavMeshAgent>().speed =
							(gameObject.GetComponent<NavMeshAgent>().speed * 2.0f);
					}

                }
                break;
            case StatusEffectType.SNARE:
                {
					if (!gameObject.GetComponent<MinionInfo>())
						break;
					gameObject.GetComponent<NavMeshAgent>().speed =
						gameObject.GetComponent<MinionInfo>().MovementSpeed;
					gameObject.GetComponent<NavMeshAgent>().Resume();
                }
                break;
            case StatusEffectType.DMG_Amp:
                {
                    gameObject.GetComponent<Info>().DmgAmp = 1.0f;
                }
                break;
            case StatusEffectType.DMG_Damp:
                {
                    gameObject.GetComponent<Info>().DmgDamp = 0.0f;
                }
                break;
            default:
                Debug.LogWarning("StatusEffects.Expire(): Default case");
                break;
        }
        if(StatusEffectsManager.Instance.Expired(gameObject.GetInstanceID().ToString(), _effect))
            Debug.LogWarning("StatusEffects.Expire(): failed to remove");
    }

}

#endif
#endregion //OLD_CODE