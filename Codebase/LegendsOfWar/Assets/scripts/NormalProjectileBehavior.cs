using UnityEngine;
public class NormalProjectileBehavior : MonoBehaviour
{
	public Transform target = null;
	[SerializeField]
	private float projectileLifetime = 2.0f;
	private float projectileTimer;
	private bool fired = false;
	public void Fire()
	{
		AudioManager.PlayClipRaw( GetComponent<AudioSource>().clip, transform );
		fired = true;
	}
	private void Start()
	{
		projectileTimer = projectileLifetime;
	}
	private void Update()
	{
		if ( GameManager.GameEnded || projectileTimer <= 0.0f )
			Destroy( gameObject );
		else if ( fired )
			projectileTimer -= Time.deltaTime;
	}
	private void FixedUpdate()
	{
		if ( !target || !target.gameObject.activeInHierarchy )
			Destroy( gameObject );
		else if ( fired && target )
		{
			transform.LookAt( target );
			transform.Translate( transform.forward * TowerManager.Instance.normalInfo.
				ProjectileSpeed * Time.fixedDeltaTime, Space.World );
		}
	}
	private void OnTriggerEnter( Collider col )
	{
		if ( target && col.transform == target )
		{
			col.GetComponent<Info>().TakeDamage( TowerManager.Instance.normalInfo.Damage );
			Destroy( gameObject );
		}
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class NormalProjectileBehavior : MonoBehaviour
{
    public Transform target = null;
    bool fired = false;

    void FixedUpdate()
    {
		if ( !target || !target.gameObject.activeInHierarchy ) Destroy( gameObject );
		else if (fired && target)
        {
            transform.LookAt(target);
            transform.Translate(transform.forward * TowerManager.Instance.normalInfo.ProjectileSpeed * Time.fixedDeltaTime, Space.World);
        }
    }

    public void Fire()
    {
        AudioManager.PlayClipRaw(GetComponent<AudioSource>().clip, transform);
        fired = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (target != null && col.gameObject == target.gameObject)
        {
            col.gameObject.GetComponent<Info>().TakeDamage(TowerManager.Instance.normalInfo.Damage);
            Destroy(gameObject);
        }
    }

	void Update() { if ( GameManager.GameEnded ) Destroy( gameObject );
	// <BUGFIX: Dev Team #21>
	else if ( projectileTimer <= 0.0f ) Destroy( gameObject );
	else if ( fired ) projectileTimer -= Time.deltaTime; }
	float projectileTimer;
	public float projectileLifetime = 2.0f;
	void Start() { projectileTimer = projectileLifetime; }
	// </BUGFIX: Dev Team #21>
}

#endif
#endregion //OLD_CODE