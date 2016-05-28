public enum StatusEffectType { NONE, DOT, STUN, SLOW, SNARE, DMG_Amp, DMG_Damp }
[System.Serializable]
public class Effect
{
	public string m_name;
	public StatusEffectType m_type;
	public float m_damage;
	public float m_duration;
	public int m_percentage;
	public float m_tickRate = 1.0f;
	public bool m_stackable;
	public int m_stacks;

	private float m_elapsedTime = 0.0f;
	private float m_tickTimer = 0.0f;
	public bool Ticked( float _time = 0.0f )
	{
		m_tickTimer += _time;
		if ( m_tickTimer < m_tickRate )
			return false;
		m_tickTimer = 0.0f;
		return true;
	}
	public bool Expired( float _time = 0.0f )
	{
		m_elapsedTime += _time;
		if ( m_elapsedTime >= m_duration )
			return true;
		else
			return false;
	}
	public void Refresh()
	{
		m_elapsedTime = 0;
	}
	public Effect CreateEffect()
	{
		Effect tmp = new Effect();
		tmp.m_damage = m_damage;
		tmp.m_duration = m_duration;
		tmp.m_elapsedTime = 0.0f;
		tmp.m_name = m_name;
		tmp.m_percentage = m_percentage;
		tmp.m_stackable = m_stackable;
		tmp.m_stacks = m_stacks;
		tmp.m_tickRate = m_tickRate;
		tmp.m_type = m_type;
		return tmp;
	}
}