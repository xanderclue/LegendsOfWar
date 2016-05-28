using UnityEngine;
public class ExplosiveProjectileBehavior : MonoBehaviour
{
	public Transform target = null;
	public float projectileLifetime = 3.0f;

	private ExplosiveProjectileInfo info;
	private Collider[ ] victims;
	private Team team;
	private bool fired, aoeActive = false;
	private float effectTime = 3.0f;
	private void Awake()
	{
		info = TowerManager.Instance.explosiveInfo;
	}
	private void FixedUpdate()
	{
		if ( fired && target && target.gameObject )
		{
			if ( !target.gameObject.activeInHierarchy && Vector3.Distance( target.position,
				transform.position ) < 1.0f )
			{
				DamageTargets();
				return;
			}
			transform.LookAt( target );
			transform.Translate( transform.forward * info.ProjectileSpeed * Time.fixedDeltaTime,
				Space.World );
		}
		if ( aoeActive )
			PlayEffect();
		if ( !target || !target.gameObject.activeInHierarchy )
			Destroy( gameObject );
	}
	public void Fire( Team theTeam )
	{
		team = theTeam;
		fired = true;
		if ( GetComponentInChildren<ParticleSystem>().isPlaying )
			GetComponentInChildren<ParticleSystem>().Stop();
	}
	private void OnTriggerEnter( Collider col )
	{
		if ( target && col.gameObject == target.gameObject )
			DamageTargets();
	}
	private void DamageTargets()
	{
		victims = Physics.OverlapSphere( transform.position, info.aoeRadius, 9,
			QueryTriggerInteraction.Collide );
		foreach ( Collider victim in victims )
		{
			Info targ = victim.gameObject.GetComponent<Info>();
			if ( targ && targ.team != team )
				targ.TakeDamage( info.aoeDamage );
		}
		AudioManager.PlayClipRaw( GetComponent<AudioSource>().clip, transform );
		GetComponentInChildren<ParticleSystem>().Play();
		GetComponent<MeshRenderer>().enabled = false;
		aoeActive = true;
		fired = false;
	}
	private void PlayEffect()
	{
		if ( effectTime <= 0.0f )
			Destroy( gameObject );
		else
			effectTime -= Time.deltaTime;
	}
	private void Update()
	{
		if ( GameManager.GameEnded || ( projectileTimer <= 0.0f && !aoeActive ) )
			Destroy( gameObject );
		else if ( fired )
			projectileTimer -= Time.deltaTime;
	}
	private float projectileTimer;
	private void Start()
	{
		projectileTimer = projectileLifetime;
	}
}