using UnityEngine;
public class PortalInfo : Info
{
	[SerializeField]
	private Transform[ ] minionSpawnPointLeft = null, minionSpawnPointMid = null,
		minionSpawnPointRight = null;
	public Transform[ ] LeftSpawn
	{ get { return minionSpawnPointLeft; } }
	public Transform[ ] MidSpawn
	{ get { return minionSpawnPointMid; } }
	public Transform[ ] RightSpawn
	{ get { return minionSpawnPointRight; } }
	public float Damage
	{ get { return damage; } }
	protected override void Start()
	{
		base.Start();
		Attacked += OnPortalAttacked;
		Destroyed += OnPortalDestroy;
	}
	private void OnPortalAttacked()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxPortalAttacked, transform.position );
	}
	private void OnPortalDestroy()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxPortalDestroyed );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE