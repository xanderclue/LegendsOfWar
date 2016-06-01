using UnityEngine;
public class SkillShot : MonoBehaviour
{
	public float speed;
	public Transform target = null;
	public float damage;
	public bool HitMultiTarget = false, MarkingAttack = false;
	public GameObject Shooter = null;
	public Effect effect = new Effect();
	public float projectileLifetime = 2.0f;
	private Info colInfo;
	private float projectileTimer;
	private int tmpStack;
	private bool isFired = false;
	public void Fire()
	{
		isFired = true;
	}
	private void Start()
	{
		projectileTimer = projectileLifetime;
	}
	private void Update()
	{
		if ( GameManager.GameEnded || projectileTimer <= 0.0f )
			Destroy( gameObject );
		else if ( isFired )
			projectileTimer -= Time.deltaTime;
	}
	private void FixedUpdate()
	{
		if ( isFired )
		{
			if ( target && target.gameObject && target.gameObject.activeInHierarchy && Vector3.
				Distance( transform.position, target.transform.position ) > 1.0 )
				transform.Translate( transform.forward * speed * Time.fixedDeltaTime, Space.World );
			else
				Destroy( gameObject );
		}
	}
	private void OnTriggerEnter( Collider col )
	{
		if ( Shooter && col )
		{
			colInfo = col.GetComponent<Info>();
			if ( colInfo && colInfo.team != Shooter.GetComponent<Info>().team )
			{
				StatusEffects.Inflict( col.gameObject, effect );
				tmpStack = StatusEffectsManager.Instance.GetStacks( col.gameObject.GetInstanceID().
					ToString(), effect.m_name );
				if ( 0 < tmpStack )
					colInfo.TakeDamage( damage * ( 2 * tmpStack + 1 ) );
				else
					colInfo.TakeDamage( damage );
				if ( !HitMultiTarget )
					Destroy( gameObject );
			}
		}
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE