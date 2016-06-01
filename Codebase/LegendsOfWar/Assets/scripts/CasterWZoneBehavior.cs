using UnityEngine;
public class CasterWZoneBehavior : MonoBehaviour
{
	public float zoneDuration;
	[SerializeField]
	private Effect m_effect = null;
	public bool Activate = false;
	private ParticleSystem ps;
	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}
	private void Update()
	{
		if ( Activate )
		{
			if ( ps.isStopped )
				ps.Play();
			zoneDuration -= Time.deltaTime;
			if ( zoneDuration <= 0.0f )
				Destroy( gameObject );
		}
	}
	private void OnParticleCollision( GameObject _other )
	{
		if ( _other.GetComponentInParent<StatusEffects>() )
			if ( Team.RED_TEAM == _other.GetComponent<Info>().team )
				StatusEffects.Inflict( _other.gameObject, m_effect.CreateEffect() );
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class CasterWZoneBehavior : MonoBehaviour {
    public float zoneDuration;
	[SerializeField]
    Effect m_effect;
    public bool Activate = false;

    void OnParticleCollision(GameObject _other)
    {
        if (_other.GetComponentInParent<StatusEffects>())
        {
            if (_other.GetComponent<Info>().team == Team.RED_TEAM)
			{
					StatusEffects.Inflict(_other.gameObject, m_effect.CreateEffect());

            }
        }
    } 

    void Update()
    {
        if (Activate)
        {
            if (GetComponent<ParticleSystem>().isStopped)
                GetComponent<ParticleSystem>().Play();
            zoneDuration -= Time.deltaTime;
            if (zoneDuration <=0.0f)
                Destroy(this.gameObject);
        }
    }
}

#endif
#endregion //OLD_CODE