using UnityEngine;
using System.Collections.Generic;
public class HeroAttack : AttackScript
{
	private List<Transform> targets;
	private ProximityCompare comparer = new ProximityCompare();
	private Info targ;
	private HeroInfo info;
	private float attackDelay, attackTimer = 0.0f, AsoundTimer = 1.0f;
	private void Start()
	{
		info = GetComponent<HeroInfo>();
		attackTrigger.CreateTrigger( info.Range );
		attackTrigger.triggerEnter += AttackTriggerEnter;
		attackTrigger.triggerExit += AttackTriggerExit;
		targets = new List<Transform>();
		attackDelay = 1.0f / info.AttackSpeed;
		info.Destroyed += targets.Clear;
	}
	private void Update()
	{
		if ( GameManager.GameEnded )
			return;
		targets.RemoveAll( item => !( item && item.gameObject.activeInHierarchy ) );
		AsoundTimer -= Time.deltaTime;
		if ( 0 < targets.Count && attackTimer <= 0.0f )
		{
			FireAtTarget( targets[ 0 ], info.Damage );
			attackTimer = attackDelay;
			if ( AsoundTimer <= 0.0f )
			{
				info.heroAudio.PlayClip( "HeroAttack" );
				AsoundTimer = 1.0f;
			}
		}
	}
	private void FixedUpdate()
	{
		attackTimer -= Time.fixedDeltaTime;
	}
	private void AttackTriggerEnter( GameObject obj )
	{
		if ( this.isActiveAndEnabled )
			if ( obj && obj.activeInHierarchy )
			{
				targ = obj.GetComponent<Info>();
				if ( targ )
					if ( targ.team != info.team )
						targets.Add( obj.transform );
			}
	}
	private void AttackTriggerExit( GameObject obj )
	{
		targets.Remove( obj.transform );
		if ( targets.Count > 2 )
		{
			targets.Sort( 1, targets.Count - 1, comparer );
			targets.Reverse( 1, targets.Count - 1 );
		}
	}
}
#region OLD_CODE
#if false
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
		//attackTimer -= Time.deltaTime;
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

	void FixedUpdate(){
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
				targets.RemoveAt( i-- );
			}
	}
}
#endif
#endregion //OLD_CODE