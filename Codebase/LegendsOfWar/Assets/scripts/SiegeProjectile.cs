using UnityEngine;

public class SiegeProjectile : MonoBehaviour
{
	public float speed;
	public float damage;
	[SerializeField]
	bool lazer = false;

	void OnTriggerEnter( Collider col )
	{
		if ( col.gameObject.GetComponent<Info>() )
		{
			if ( col.gameObject.GetComponent<Info>().team == Team.BLUE_TEAM )
			{
				col.gameObject.GetComponent<Info>().TakeDamage( damage + 1 );
				if ( lazer )
				{
					Effect effect = new Effect();
					effect.m_type = StatusEffectType.DOT;
					effect.m_duration = 3.0f;
					effect.m_name = "Lazer Burn";
					effect.m_tickRate = 0.5f;
					effect.m_damage = Mathf.Max( damage, 25 );
					StatusEffects.Inflict( col.gameObject, effect.CreateEffect() );
				}
				Destroy( this.gameObject );
			}
		}
	}


	void Update()
	{
		if ( GameManager.GameEnded )
			Destroy( gameObject );
		else if ( projectileTimer <= 0.0f )
			Destroy( gameObject );
		else
			projectileTimer -= Time.deltaTime;
	}
	float projectileTimer;
	public float projectileLifetime = 2.0f;
	void Start() { projectileTimer = projectileLifetime; }
}
