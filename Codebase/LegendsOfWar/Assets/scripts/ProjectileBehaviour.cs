using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
	public float speed;
	public Transform target = null;
	public float damage;
	private bool isFired = false;

	void FixedUpdate()
	{
		if ( isFired )
		{
			if ( target && target.gameObject.activeInHierarchy )
			{
				transform.LookAt( target );
				transform.Translate( transform.forward * speed * Time.fixedDeltaTime, Space.World );
			}
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
		if ( col.gameObject == target.gameObject )
		{
			col.gameObject.GetComponent<Info>().TakeDamage( damage );
			Destroy( gameObject );
		}
	}

	void Update()
	{
		if ( GameManager.GameEnded )
			Destroy( gameObject );
		else if ( projectileTimer <= 0.0f )
			Destroy( gameObject );
		else if ( isFired )
			projectileTimer -= Time.deltaTime;
	}
	float projectileTimer;
	public float projectileLifetime = 3.0f;
	void Start() { projectileTimer = projectileLifetime; }
}