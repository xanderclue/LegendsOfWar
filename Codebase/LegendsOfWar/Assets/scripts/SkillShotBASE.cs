using UnityEngine;
using System.Collections;

public abstract class SkillShotBASE : MonoBehaviour {
    public float speed;
    public Vector3 RangeLimit = Vector3.zero;
    public float damage;
    public bool MultiTarget = false;
    public GameObject Shooter = null;
    public Effect effect = new Effect();
    private bool isFired = false;

    protected virtual void FixedUpdate()
    {
        if (isFired)
        {
            if (Vector3.Distance(transform.position, RangeLimit) > 1.0)
            {
                transform.LookAt(RangeLimit);
                transform.Translate(transform.forward * speed * Time.fixedDeltaTime, Space.World);
            }
            else Destroy(gameObject);
        }
    }

    public virtual void Fire()
    {
        isFired = true;
    }
    
    //Deals Damage by default. destroys itself if it's not multi hit.
    protected virtual void OnTriggerEnter(Collider col)
    {
        if (Shooter == null || col.gameObject == null)
            return;
        else if (col.gameObject.GetComponent<Info>() != null && col.gameObject.GetComponent<Info>().team != Shooter.GetComponent<Info>().team)
        {
            col.gameObject.GetComponent<Info>().TakeDamage(damage);
            if (MultiTarget == false)
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
