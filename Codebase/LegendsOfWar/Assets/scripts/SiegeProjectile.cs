using UnityEngine;
public class SiegeProjectile : MonoBehaviour
{
	public float speed;
	public float damage;
	[SerializeField]
	private bool lazer = false;
	public float projectileLifetime = 2.0f;

	private void OnTriggerEnter( Collider col )
	{
		if ( col.gameObject.GetComponent<Info>() )
			if ( col.gameObject.GetComponent<Info>().team == Team.BLUE_TEAM )
			{
				col.gameObject.GetComponent<Info>().TakeDamage( damage + 1.0f );
				if ( lazer )
				{
					Effect effect = new Effect();
					effect.m_type = StatusEffectType.DOT;
					effect.m_duration = 3.0f;
					effect.m_name = "Lazer Burn";
					effect.m_tickRate = 0.5f;
					effect.m_damage = Mathf.Max( damage, 25.0f );
					StatusEffects.Inflict( col.gameObject, effect.CreateEffect() );
				}
				Destroy( this.gameObject );
			}
	}
	private void Update()
	{
		if ( GameManager.GameEnded || projectileTimer <= 0.0f )
			Destroy( gameObject );
		else
			projectileTimer -= Time.deltaTime;
	}
	private float projectileTimer;
	private void Start()
	{
		projectileTimer = projectileLifetime;
	}
}