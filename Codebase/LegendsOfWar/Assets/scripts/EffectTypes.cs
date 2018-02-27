public enum StatusEffectType { NONE, DOT, STUN, SLOW, SNARE, DMG_Amp, DMG_Damp }
[System.Serializable]
public class Effect
{
    public string m_name;
    public StatusEffectType m_type;
    public float m_damage, m_duration;
    public int m_percentage;
    public float m_tickRate = 1.0f;
    public bool m_stackable;
    public int m_stacks;
    private float m_elapsedTime = 0.0f, m_tickTimer = 0.0f;
    public Effect CreateEffect()
        => new Effect
        {
            m_damage = m_damage,
            m_duration = m_duration,
            m_elapsedTime = 0.0f,
            m_name = m_name,
            m_percentage = m_percentage,
            m_stackable = m_stackable,
            m_stacks = m_stacks,
            m_tickRate = m_tickRate,
            m_type = m_type
        };
    public bool Ticked(float _time)
    {
        m_tickTimer += _time;
        if (m_tickTimer < m_tickRate)
            return false;
        m_tickTimer = 0.0f;
        return true;
    }
    public bool Expired(float _time)
    {
        m_elapsedTime += _time;
        if (m_elapsedTime >= m_duration)
            return true;
        else
            return false;
    }
    public void Refresh()
    {
        m_elapsedTime = 0;
    }
}