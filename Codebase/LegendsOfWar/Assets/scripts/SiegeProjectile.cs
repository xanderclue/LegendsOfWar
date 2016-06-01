using UnityEngine;
public class SiegeProjectile : MonoBehaviour
{
	public float speed, damage;
	[SerializeField]
	private bool lazer = false;
	public float projectileLifetime = 2.0f;
	private Info colInfo;
	private float projectileTimer;
	private void Start()
	{
		projectileTimer = projectileLifetime;
	}
	private void Update()
	{
		if ( projectileTimer <= 0.0f || GameManager.GameEnded )
			Destroy( gameObject );
		else
			projectileTimer -= Time.deltaTime;
	}
	private void OnTriggerEnter( Collider col )
	{
		colInfo = col.gameObject.GetComponent<Info>();
		if ( colInfo )
			if ( Team.BLUE_TEAM == colInfo.team )
			{
				colInfo.TakeDamage( damage + 1.0f );
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
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class SiegeProjectile : MonoBehaviour {
    public float speed;
    //public Transform target = null;
    public float damage;
	[SerializeField]
	bool lazer = false;
    //public bool HitFirstCollision = false;
    //private bool isFired = false;

    //public void Fire()
    //{
    //    isFired = true;
    //}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<Info>())
        {
            if (col.gameObject.GetComponent<Info>().team == Team.BLUE_TEAM /*|| HitFirstCollision == true*/)
            {
                col.gameObject.GetComponent<Info>().TakeDamage(damage+1);
				if (lazer) {
					Effect effect = new Effect();
					effect.m_type = StatusEffectType.DOT;
					effect.m_duration = 3.0f;
					effect.m_name = "Lazer Burn";
					effect.m_tickRate = 0.5f;
					effect.m_damage = Mathf.Max(damage, 25);
					StatusEffects.Inflict(col.gameObject, effect.CreateEffect());
				}
                Destroy(this.gameObject);
            }
        }
    }


	// BUGFIX Code is redundant if used where intended, as in by the siege minion. 
	// The siege minion firing has it's own bullet script that does the below and more. 
	void Update() { if ( GameManager.GameEnded ) Destroy( gameObject );
	// <BUGFIX: Dev Team #21>
	else if ( projectileTimer <= 0.0f ) Destroy( gameObject );
	else projectileTimer -= Time.deltaTime; }
	float projectileTimer;
	public float projectileLifetime = 2.0f;
	void Start() { projectileTimer = projectileLifetime; }
	// </BUGFIX: Dev Team #21>
}

#endif
#endregion //OLD_CODE