public class TowerInfo : Info
{
	protected override void Start()
	{
		base.Start();
		Attacked += TowerAttacked;
		Destroyed += TowerDestroyed;
	}
	void TowerAttacked()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxTowerAttacked, transform.position );
	}
	void TowerDestroyed()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxTowerDestroyed, transform.position );
	}
}