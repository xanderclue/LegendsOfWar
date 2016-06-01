using UnityEngine;
public class ProjectileBehaviour : MonoBehaviour
{
	public float speed;
	public Transform target = null;
	public float damage, projectileLifetime = 3.0f;
	private float projectileTimer;
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
			if ( target && target.gameObject.activeInHierarchy )
			{
				transform.LookAt( target );
				transform.Translate( speed * Time.fixedDeltaTime * transform.forward, Space.World );
			}
			else
				Destroy( gameObject );
	}
	private void OnTriggerEnter( Collider col )
	{
		if ( col.gameObject == target.gameObject )
		{
			col.gameObject.GetComponent<Info>().TakeDamage( damage );
			Destroy( gameObject );
		}
	}
}