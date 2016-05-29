using UnityEngine;
using System.Collections.Generic;
public class MinionAttack : AttackScript
{
	[SerializeField]
	private List<Transform> targets;
	[SerializeField]
	private ParticleSystem attackParticles = null;
	private MinionMovement movement;
	private ProximityCompare poo = new ProximityCompare();
	private float second = 1.0f;
	private float effectTime = 0.5f;
	private bool psEnabled = false;
	private void Start()
	{
		Minioninfo = GetComponent<MinionInfo>();
		attackTrigger.CreateTrigger( Minioninfo.AgroRange );
		attackTrigger.triggerEnter += AttackTriggerEnter;
		attackTrigger.triggerExit += AttackTriggerExit;
		targets = new List<Transform>();
		movement = GetComponent<MinionMovement>();
	}
	private void Update()
	{
		second -= Time.deltaTime * Minioninfo.AttackSpeed;
		if ( targets.Count == 0 || !targets[ 0 ] || !targets[ 0 ].gameObject.GetComponent<Info>().
			Alive )
		{
			movement.Disengage();
			Nil();
			if ( targets.Count >= 1 && !targets[ 0 ].gameObject.GetComponent<Info>().Alive )
				AttackTriggerExit( targets[ 0 ].gameObject );
		}
		else if ( movement.InCombat && movement.WithinRange && second <= 0 )
		{
			if ( attackParticles )
			{
				psEnabled = true;
				attackParticles.Play();
			}
			FireAtTarget( targets[ 0 ], 120.0f, Minioninfo.Damage );
			AudioManager.PlaySoundEffect( AudioManager.sfxMinionAttack, transform.position );
			second = 1.0f;
		}
		else if ( !movement.InCombat )
		{
			if ( targets[ 0 ].gameObject.GetComponent<PortalInfo>() )
				movement.SetTarget( targets[ 0 ], Minioninfo.Range + 30.0f );
			else
				movement.SetTarget( targets[ 0 ], Minioninfo.Range );
		}
		if ( psEnabled && effectTime <= 0.0f )
		{
			psEnabled = false;
			attackParticles.Stop();
			effectTime = 0.25f;
		}
	}
	private void AttackTriggerEnter( GameObject obj )
	{
		if ( this.isActiveAndEnabled )
			if ( obj && obj.activeInHierarchy )
			{
				Info targ = obj.GetComponent<Info>();
				if ( targ )
					if ( targ.team != Minioninfo.team )
						targets.Add( obj.transform );
			}
	}
	private void AttackTriggerExit( GameObject obj )
	{
		targets.Remove( obj.transform );
		if ( targets.Count > 2 )
		{
			targets.Sort( 1, targets.Count - 1, poo );
			targets.Reverse( 1, targets.Count - 1 );
		}
	}
	private void Nil()
	{
		for ( int i = 0; i < targets.Count; ++i )
			if ( !( targets[ i ] && targets[ i ].gameObject.activeInHierarchy ) )
				targets.RemoveAt( i-- );
	}
}