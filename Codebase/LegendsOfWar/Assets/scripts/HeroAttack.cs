using UnityEngine;
using System.Collections.Generic;

public class HeroAttack : AttackScript
{
	private HeroInfo info;
	List<Transform> targets;

	ProximityCompare comparer = new ProximityCompare();
	float attackDelay;
	float attackTimer = 0.0f;
	void Start()
	{
		info = GetComponent<HeroInfo>();
		attackTrigger.CreateTrigger( info.Range );
		attackTrigger.triggerEnter += AttackTriggerEnter;
		attackTrigger.triggerExit += AttackTriggerExit;
		targets = new List<Transform>();
		attackDelay = 1.0f / info.AttackSpeed;
		info.Destroyed += targets.Clear;
	}

	void AttackTriggerEnter( GameObject obj )
	{
		if ( this.isActiveAndEnabled )
			if ( obj && obj.activeInHierarchy )
			{
				Info targ = obj.GetComponent<Info>();
				if ( targ )
					if ( targ.team != info.team )
						targets.Add( obj.transform );
			}
	}

	float AsoundTimer = 1.0f;
	void Update()
	{
		if ( GameManager.GameEnded )
			return;
		Nil();
		AsoundTimer -= Time.deltaTime;
		if ( targets.Count > 0 && attackTimer <= 0.0f )
		{
			FireAtTarget( targets[ 0 ], 120.0f, info.Damage );
			attackTimer = attackDelay;
			if ( AsoundTimer <= 0.0f )
			{
				info.heroAudio.PlayClip( "HeroAttack" );
				AsoundTimer = 1.0f;
			}
		}
	}

	void FixedUpdate()
	{
		attackTimer -= Time.fixedDeltaTime;
	}

	void AttackTriggerExit( GameObject obj )
	{
		targets.Remove( obj.transform );
		if ( targets.Count > 2 )
		{
			targets.Sort( 1, targets.Count - 1, comparer );
			targets.Reverse( 1, targets.Count - 1 );
		}
	}

	void Nil()
	{
		for ( int i = 0; i < targets.Count; ++i )
			if ( targets[ i ] && targets[ i ].gameObject.activeInHierarchy )
				continue;
			else
			{
				targets.RemoveAt( i ); --i;
			}
	}
}