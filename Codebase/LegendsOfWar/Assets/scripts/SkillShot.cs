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
using UnityEngine;
using System.Collections;

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
        if (isFired)
        {
            if (target && target.gameObject && target.gameObject.activeInHierarchy && Vector3.Distance( transform.position , target.transform.position)>1.0)
            {
                //transform.LookAt(target);
                transform.Translate(transform.forward * speed * Time.fixedDeltaTime, Space.World);
            }
            else Destroy(gameObject);
        }
    }

    public void Fire()
    {
        isFired = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (Shooter == null || col.gameObject == null)
            return;
        //if (col.gameObject == target.gameObject)
        //{
        //    //col.gameObject.GetComponent<Info>().TakeDamage(damage);
        //    Destroy(gameObject);
        //}
        else if (col.gameObject.GetComponent<Info>()!=null && col.gameObject.GetComponent<Info>().team != Shooter.GetComponent<Info>().team)
        {
            Debug.Log("DAMAGE OVER TIME");
            //TODO:DAMAGE OVER TIME

            //TODO: MARKING ATTACK
            //if (MarkingAttack)
            //{
            //    if(Shooter.GetComponentInChildren<AssassinAbilityW>().Marked[0] == col.gameObject)
            //    {
            //        damage *= 2;
            //        if (Shooter.GetComponentInChildren<AssassinAbilityW>().Marked[1] == col.gameObject)
            //        {
            //            damage *= 2;
            //            if(Shooter.GetComponentInChildren<AssassinAbilityW>().Marked[2] == col.gameObject)
            //            {
            //                damage *= 2;
            //            }
            //        }
            //    }
            //    Shooter.GetComponentInChildren<AssassinAbilityW>().MarkHit(col.gameObject);

            //}

            StatusEffects.Inflict(col.gameObject,effect);
            Debug.Log(col.gameObject.GetInstanceID().ToString());

                var tmpStack = StatusEffectsManager.Instance.GetStacks(col.gameObject.GetInstanceID().ToString(), effect.m_name);

            if (tmpStack > 0)
            {
                col.gameObject.GetComponent<Info>().TakeDamage(damage * (1+2*tmpStack));
                Debug.Log("Stacked damage" + tmpStack);
            }
            else
                col.gameObject.GetComponent<Info>().TakeDamage(damage);


            if(HitMultiTarget == false)
                Destroy(gameObject);
        }
    }

	void Update() { if ( GameManager.GameEnded ) Destroy( gameObject );
	// <BUGFIX: Dev Team #21>
	else if ( projectileTimer <= 0.0f ) Destroy( gameObject );
	else if ( isFired ) projectileTimer -= Time.deltaTime; }
	float projectileTimer;
	public float projectileLifetime = 2.0f;
	void Start() { projectileTimer = projectileLifetime; }
	// </BUGFIX: Dev Team #21>
}

#endif
#endregion //OLD_CODE