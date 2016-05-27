using UnityEngine;
public class SkillShot : MonoBehaviour
{
	public float speed;
	public Transform target = null;
	public float damage;
	public bool HitMultiTarget = false;
	public bool MarkingAttack = false;
	public GameObject Shooter = null;
	public Effect effect = new Effect();
	private bool isFired = false;
	void FixedUpdate()
	{
		if ( isFired )
		{
			if ( target && target.gameObject && target.gameObject.activeInHierarchy && Vector3.Distance( transform.position, target.transform.position ) > 1.0 )
				transform.Translate( transform.forward * speed * Time.fixedDeltaTime, Space.World );
			else
				Destroy( gameObject );
		}
	}
	public void Fire()
	{
		isFired = true;
	}
	void OnTriggerEnter( Collider col )
	{
		if ( Shooter && col )
			if ( col.gameObject.GetComponent<Info>() && col.gameObject.GetComponent<Info>().team != Shooter.GetComponent<Info>().team )
			{
				StatusEffects.Inflict( col.gameObject, effect );
				int tmpStack = StatusEffectsManager.Instance.GetStacks( col.gameObject.GetInstanceID().ToString(), effect.m_name );
				if ( tmpStack > 0 )
					col.gameObject.GetComponent<Info>().TakeDamage( damage * ( 1 + 2 * tmpStack ) );
				else
					col.gameObject.GetComponent<Info>().TakeDamage( damage );
				if ( !HitMultiTarget )
					Destroy( gameObject );
			}
	}
	void Update()
	{
		if ( GameManager.GameEnded || projectileTimer <= 0.0f )
			Destroy( gameObject );
		else if ( isFired )
			projectileTimer -= Time.deltaTime;
	}
	float projectileTimer;
	public float projectileLifetime = 2.0f;
	void Start()
	{
		projectileTimer = projectileLifetime;
	}
}