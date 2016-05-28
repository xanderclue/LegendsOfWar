using UnityEngine;
public class ProjectileBehaviour : MonoBehaviour
{
	public float speed;
	public Transform target = null;
	public float damage;
	public float projectileLifetime = 3.0f;
	private bool isFired = false;
	private float projectileTimer;

	private void FixedUpdate()
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
	private void OnTriggerEnter( Collider col )
	{
		if ( col.gameObject == target.gameObject )
		{
			col.gameObject.GetComponent<Info>().TakeDamage( damage );
			Destroy( gameObject );
		}
	}
	private void Update()
	{
		if ( GameManager.GameEnded || projectileTimer <= 0.0f )
			Destroy( gameObject );
		else if ( isFired )
			projectileTimer -= Time.deltaTime;
	}
	private void Start()
	{
		projectileTimer = projectileLifetime;
	}
}