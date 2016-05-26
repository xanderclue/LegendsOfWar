using UnityEngine;
using System.Collections.Generic;

public class ExplosiveProjectileBehavior : MonoBehaviour
{
	ExplosiveProjectileInfo info;
	Collider[ ] victims;

	Team team;
	public Transform target = null;
	bool fired, aoeActive = false;
	float effectTime = 3.0f;

	void Awake()
	{
		info = TowerManager.Instance.explosiveInfo;
	}

	void FixedUpdate()
	{
		if ( fired && target && target.gameObject )
		{
			if ( !target.gameObject.activeInHierarchy && Vector3.Distance( target.position, transform.position ) < 1.0f )
			{
				DamageTargets();
				return;
			}
			transform.LookAt( target );
			transform.Translate( transform.forward * info.ProjectileSpeed * Time.fixedDeltaTime, Space.World );
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

	void OnTriggerEnter( Collider col )
	{
		if ( target != null && col.gameObject == target.gameObject )
			DamageTargets();
	}

	void DamageTargets()
	{
		victims = Physics.OverlapSphere( transform.position, info.aoeRadius, 9, QueryTriggerInteraction.Collide );

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

	void PlayEffect()
	{
		if ( effectTime <= 0 )
			Destroy( gameObject );
		else
			effectTime -= Time.deltaTime;
	}

	void Update()
	{
		if ( GameManager.GameEnded )
			Destroy( gameObject );
		// <BUGFIX: Dev Team #21>
		else if ( projectileTimer <= 0.0f && !aoeActive )
			Destroy( gameObject );
		else if ( fired )
			projectileTimer -= Time.deltaTime;
	}
	float projectileTimer;
	public float projectileLifetime = 3.0f;
	void Start() { projectileTimer = projectileLifetime; }
	// </BUGFIX: Dev Team #21>
}
