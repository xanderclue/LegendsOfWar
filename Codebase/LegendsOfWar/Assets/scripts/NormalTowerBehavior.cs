using UnityEngine;
using System.Collections.Generic;
public class NormalTowerBehavior : MonoBehaviour
{
	[SerializeField]
	Team team = Team.BLUE_TEAM;
	[SerializeField]
	Transform projectileSpawnPoint = null;
	[SerializeField]
	Detector detector = null;
	List<Transform> targets;
	NormalProjectileInfo info;
	float fireTimer;
	void Awake()
	{
		targets = new List<Transform>();
		info = TowerManager.Instance.normalInfo;
		detector.CreateTrigger( info.AgroRange );
		detector.triggerEnter += AddTarget;
		detector.triggerExit += RemoveTarget;
	}
	void AddTarget( GameObject obj )
	{
		if ( obj )
		{
			Info targ = obj.GetComponent<Info>();
			if ( targ && targ.team != team )
				targets.Add( obj.transform );
		}
	}
	void RemoveTarget( GameObject obj )
	{
		targets.Remove( obj.transform );
	}
	void Update()
	{
		targets.RemoveAll( item => item == null );
		if ( TowerManager.Instance.CheckIfShotActive( team, Items.NormalShot ) )
		{
			if ( fireTimer <= 0.0f )
			{
				if ( targets != null && targets.Count > 0 && targets[ 0 ] && targets[ 0 ].gameObject )
				{
					if ( !targets[ 0 ].gameObject.activeInHierarchy )
						RemoveTarget( targets[ 0 ].gameObject );
					else
					{
						FireAtTarget();
						fireTimer = info.AttackSpeed;
					}
				}
			}
			else
				fireTimer -= Time.deltaTime;
		}
	}
	void FireAtTarget()
	{
		if ( GameManager.GameEnded )
			return;
		projectileSpawnPoint.LookAt( targets[ 0 ] );
		NormalProjectileBehavior p = ( Instantiate( TowerManager.Instance.normalShotPrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation ) as GameObject ).GetComponent<NormalProjectileBehavior>();
		p.target = targets[ 0 ];
		p.Fire();
	}
}