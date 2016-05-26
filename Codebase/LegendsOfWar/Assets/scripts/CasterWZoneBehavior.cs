using UnityEngine;
using System.Collections;

public class CasterWZoneBehavior : MonoBehaviour
{
	public float zoneDuration;
	[SerializeField]
	Effect m_effect;
	public bool Activate = false;

	void OnParticleCollision( GameObject _other )
	{
		if ( _other.GetComponentInParent<StatusEffects>() )
		{
			if ( _other.GetComponent<Info>().team == Team.RED_TEAM )
			{
				StatusEffects.Inflict( _other.gameObject, m_effect.CreateEffect() );

			}
		}
	}

	void Update()
	{
		if ( Activate )
		{
			if ( GetComponent<ParticleSystem>().isStopped )
				GetComponent<ParticleSystem>().Play();
			zoneDuration -= Time.deltaTime;
			if ( zoneDuration <= 0.0f )
				Destroy( this.gameObject );
		}
	}
}
