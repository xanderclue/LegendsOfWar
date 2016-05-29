using UnityEngine;
public enum Difficulty { Easy, Hard }
public class HeroInfo : Info
{
	public Sprite heroIcon;
	[SerializeField]
	private float maxMana = 100.0f;
	public float manaRegen = 7.5f;
	public string Lore = "", roaa = "";
	public Transform thirdPerson = null;
	public Transform heroCenter = null;
	public float respawnTime = 9.0f, respawnIncrement = 3.0f;
	public Difficulty difficulty = Difficulty.Easy;
	public string heroNameEn = "Player";
	public string heroNameJp = "プレイヤー";
	private float mana;
	private float respawnTimer;
	private HeroMovement movement;
	private float tauntTimer = 0.0f;
	private float idleTimer;
	public HeroAudio heroAudio
	{ get; private set; }
	public float Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public float Range
	{ get { return attackRange; } }
	public float AttackSpeed
	{ get { return attackSpeed; } }
	public float Mana
	{ get { return mana; } }
	public float MaxMana
	{ get { return maxMana; } }
	public bool waitingRespawn
	{ get { return ( !Alive && respawnTimer <= 0.0f ); } }
	public float RespawnTimer
	{
		get { return respawnTimer; }
		set { respawnTimer = value; }
	}
	public void Deidle()
	{
		idleTimer = 25.0f;
	}
	public bool UseMana( float manaCost )
	{
		if ( mana - manaCost >= 0.0f )
			mana -= manaCost;
		else
			return false;
		HeroUIScript.Mana( manaCost, transform );
		return true;
	}
	public void Respawn()
	{
		movement.ResetToSpawn();
		mana = maxMana;
		Alive = true;
	}
	protected override void Start()
	{
		base.Start();
		movement = GetComponent<HeroMovement>();
		mana = maxMana;
		dontDestroy = true;
		Attacked += HeroAttacked;
		Destroyed += HeroDeath;
		if ( GameManager.Avail )
			GameManager.Instance.AddHero( this );
		heroAudio = GetComponent<HeroAudio>();
		idleTimer = 8.0f;
	}
	private void Update()
	{
		mana = Mathf.Min( mana + Time.deltaTime * manaRegen, maxMana );
		tauntTimer -= Time.deltaTime;
		if ( Input.GetKeyDown( KeyCode.T ) && tauntTimer < 0.0f )
		{
			if ( heroAudio.CHeroTaunt1 && heroAudio.CHeroTaunt2 )
				tauntTimer = heroAudio.PlayClip( "HeroTaunt" + Random.Range( 1, 3 ) );
			else if ( heroAudio.CHeroTaunt1 )
				tauntTimer = heroAudio.PlayClip( "HeroTaunt1" );
			else if ( heroAudio.CHeroTaunt2 )
				tauntTimer = heroAudio.PlayClip( "HeroTaunt2" );
		}
		idleTimer -= Time.deltaTime;
		if ( idleTimer <= 0.0f )
		{
			if ( heroAudio.CHeroIdle1 && heroAudio.CHeroIdle2 )
				heroAudio.PlayClip( "HeroIdle" + Random.Range( 1, 3 ) );
			else if ( heroAudio.CHeroIdle1 )
				heroAudio.PlayClip( "HeroIdle1" );
			else if ( heroAudio.CHeroIdle2 )
				heroAudio.PlayClip( "HeroIdle2" );
			Deidle();
		}
	}
	private void HeroAttacked()
	{
		overlay.Flash( HP, MAXHP );
		AudioManager.PlaySoundEffect( AudioManager.sfxHeroAttacked, transform.position );
		HeroUIScript.HeroBeingAttacked = true;
	}
	private void HeroDeath()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxHeroDeath, transform.position );
		respawnTimer = respawnTime;
		respawnTime += respawnIncrement;
		mana = 0.0f;
	}
}