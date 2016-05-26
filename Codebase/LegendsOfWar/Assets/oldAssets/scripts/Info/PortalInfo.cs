using UnityEngine;
using System.Collections.Generic;

public class PortalInfo : Info
{
	[SerializeField]
	Transform[] minionSpawnPointLeft=null;
	[SerializeField]
    Transform[] minionSpawnPointMid =null;
	[SerializeField]
    Transform[] minionSpawnPointRight =null;


	public Transform[] LeftSpawn { get { return minionSpawnPointLeft; } }
	public Transform[] MidSpawn { get { return minionSpawnPointMid; } }
	public Transform[] RightSpawn { get { return minionSpawnPointRight; } }

	public float Damage{ get { return damage; } }

	protected override void Start()
	{
		base.Start();
		Attacked += OnPortalAttacked;
		Destroyed += OnPortalDestroy;
	}

	void OnPortalDestroy()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxPortalDestroyed );
	}

	void OnPortalAttacked()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxPortalAttacked, transform.position );
	}
}