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