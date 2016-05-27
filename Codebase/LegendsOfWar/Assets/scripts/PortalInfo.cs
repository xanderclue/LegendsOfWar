using UnityEngine;
public class PortalInfo : Info
{
	[SerializeField]
	private Transform[ ] minionSpawnPointLeft = null;
	[SerializeField]
	private Transform[ ] minionSpawnPointMid = null;
	[SerializeField]
	private Transform[ ] minionSpawnPointRight = null;
	public Transform[ ] LeftSpawn { get { return minionSpawnPointLeft; } }
	public Transform[ ] MidSpawn { get { return minionSpawnPointMid; } }
	public Transform[ ] RightSpawn { get { return minionSpawnPointRight; } }
	public float Damage { get { return damage; } }
	protected override void Start()
	{
		base.Start();
		Attacked += OnPortalAttacked;
		Destroyed += OnPortalDestroy;
	}
	private void OnPortalDestroy()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxPortalDestroyed );
	}
	private void OnPortalAttacked()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxPortalAttacked, transform.position );
	}
}