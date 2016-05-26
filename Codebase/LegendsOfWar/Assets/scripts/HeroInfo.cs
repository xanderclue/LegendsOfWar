using UnityEngine;

public class HeroInfo : Info
{
	float mana;
	float respawnTimer;
	int deathCount;
	public Sprite heroIcon;
    public float Damage { get { return damage; } set { damage = value; } }
    public float Range { get { return attackRange; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AgroRange { get { return agroRange; } }

    [SerializeField]
	float maxMana = 100.0f;

	public float manaRegen = 7.5f;

    [SerializeField]
    public string Lore = "", roaa = "";

	public Transform thirdPerson = null;
	public Transform heroCenter = null;
	HeroMovement movement;

	//[SerializeField]
	//float damage;

	public float respawnTime = 9.0f, respawnIncrement = 3.0f;
	[HideInInspector]
	public HeroAudio heroAudio;

	void Update()
	{
		mana = Mathf.Min( mana + Time.deltaTime * manaRegen, maxMana );
		tauntTimer -= Time.deltaTime;
		if ( Input.GetKeyDown( KeyCode.T ) )
			PlayTaunt();
		idleTimer -= Time.deltaTime;
		if ( idleTimer <= 0.0f )
		{
			PlayIdle();
			Deidle();
		}
	}

	float tauntTimer = 0.0f;
	void PlayTaunt()
	{
		if ( tauntTimer >= 0.0f )
			return;
		if ( heroAudio.CHeroTaunt1 && heroAudio.CHeroTaunt2 )
			tauntTimer = heroAudio.PlayClip( "HeroTaunt" + Random.Range( 1, 3 ) );
		else if ( heroAudio.CHeroTaunt1 )
			tauntTimer = heroAudio.PlayClip( "HeroTaunt1" );
		else if ( heroAudio.CHeroTaunt2 )
			tauntTimer = heroAudio.PlayClip( "HeroTaunt2" );
	}

	protected override void Start()
	{
		base.Start();
		movement = GetComponent<HeroMovement>();
		mana = maxMana;
		dontDestroy = true;
		Attacked += HeroAttacked;
		Destroyed += HeroDeath;
		if(GameManager.Avail)
			GameManager.Instance.AddHero( this );
		heroAudio = GetComponent<HeroAudio>();
		idleTimer = 8.0f;
    }
	float idleTimer;
    void PlayIdle()
    {
		if ( heroAudio.CHeroIdle1 && heroAudio.CHeroIdle2 )
			heroAudio.PlayClip( ( Random.Range( 0, 2 ) == 0 ) ? "HeroIdle1" : "HeroIdle2" );
		else if ( heroAudio.CHeroIdle1 )
			heroAudio.PlayClip( "HeroIdle1" );
		else if ( heroAudio.CHeroIdle2 )
			heroAudio.PlayClip( "HeroIdle2" );
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

	void HeroAttacked()
	{
		overlay.Flash( HP, MAXHP );
		AudioManager.PlaySoundEffect( AudioManager.sfxHeroAttacked, transform.position );
		HeroUIScript.HeroBeingAttacked = true;
	}

	void HeroDeath()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxHeroDeath, transform.position );
        respawnTimer = respawnTime;
		respawnTime += respawnIncrement;
        ++deathCount;
		// <BUGFIX: Dev Team #19>
		mana = 0.0f;
        // </BUGFIX: Dev Team #19>
	}

	public float Mana { get { return mana; } }
	public float MaxMana { get { return maxMana; } }
	public bool waitingRespawn { get { return ( !Alive && respawnTimer <= 0.0f ); } }
	public float RespawnTimer { get { return respawnTimer; }set { respawnTimer = value; } }
	public void Respawn()
	{
		movement.ResetToSpawn();
		// <BUGFIX: Dev Team #19>
		mana = maxMana;
		// </BUGFIX: Dev Team #19>
		Alive = true;
	}

	public Difficulty difficulty = Difficulty.Easy;

	public string heroNameEn = "Player";
	public string heroNameJp = "プレイヤー";
}
public enum Difficulty { Easy, Hard }