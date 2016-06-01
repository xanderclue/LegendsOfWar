using UnityEngine;
using System.Collections.Generic;
public class MinionAttack : AttackScript
{
	[SerializeField]
	private List<Transform> targets;
	[SerializeField]
	private ParticleSystem attackParticles = null;
	private ProximityCompare poo = new ProximityCompare();
	private Info targ;
	private MinionInfo Minioninfo;
	private MinionMovement movement;
	private float second = 1.0f;
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
		if ( 0 == targets.Count || !targets[ 0 ] || !targets[ 0 ].GetComponent<Info>().Alive )
		{
			movement.Disengage();
			targets.RemoveAll( item => !item || !item.gameObject.activeInHierarchy );
			if ( 0 < targets.Count && !targets[ 0 ].GetComponent<Info>().Alive )
				AttackTriggerExit( targets[ 0 ].gameObject );
		}
		else if ( movement.InCombat && movement.WithinRange && second <= 0.0f )
		{
			if ( attackParticles )
				attackParticles.Play();
			FireAtTarget( targets[ 0 ], Minioninfo.Damage );
			AudioManager.PlaySoundEffect( AudioManager.sfxMinionAttack, transform.position );
			second = 1.0f;
		}
		else if ( !movement.InCombat )
			if ( targets[ 0 ].GetComponent<PortalInfo>() )
				movement.SetTarget( targets[ 0 ], Minioninfo.Range + 30.0f );
			else
				movement.SetTarget( targets[ 0 ], Minioninfo.Range );
	}
	private void AttackTriggerEnter( GameObject obj )
	{
		if ( isActiveAndEnabled )
			if ( obj && obj.activeInHierarchy )
			{
				targ = obj.GetComponent<Info>();
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
}