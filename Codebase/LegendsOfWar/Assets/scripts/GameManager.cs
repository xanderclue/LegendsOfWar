using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	bool tutorial = false;
	public static bool Tutorial { get { if ( instance ) return instance.tutorial; return false; } }
	[SerializeField]
	public static bool WebPlayerMode = false;

	[SerializeField]
	PortalInfo redPortal = null;
	[SerializeField]
	PortalInfo bluePortal = null;
	[SerializeField]
	GameObject redTankMinion = null, redCasterMinion = null, redStrikerMinion = null;
	[SerializeField]
	GameObject blueTankMinion = null, blueCasterMinion = null, blueStrikerMinion = null;
	[SerializeField]
	EventSystem uiEventSystem = null;
	bool gameRunning = false;
	[SerializeField]
	float maxTime = 900.0f;
	[SerializeField]
	float waveTime = 60.0f;
	[SerializeField]
	HudScript hudScript = null;
	public static HudScript Hud { get { return instance.hudScript; } }
	public static float topSplitZ, botSplitZ;

	[SerializeField]
	GameObject cursorObject = null;
	public static GameObject cursor { get { return instance.cursorObject; } }

	float waveTimer;
	float timer;
	int wave = 0;
	public static List<HeroAbilities> abilities;
	[SerializeField]
	Transform HeroSpawnPoint = null;
	public GameObject Player;

	public static bool Avail { get { return instance != null; } }
	public static bool GameRunning
	{
		get
		{
			if ( instance )
				return Instance.gameRunning;
			else
				return false;
		}
	}
	public static Transform RedPortalTransform { get { return Instance.redPortal.transform; } }
	public static Transform BluePortalTransform { get { return Instance.bluePortal.transform; } }

	public ParticleSystem BoomRed;
	public ParticleSystem BoomBlue;
	public ParticleSystem ExitRed;
	public ParticleSystem ExitBlue;
	MinionInfo[ ] endminions;

	GameObject min_go;
	NavMeshAgent nma;
	void SetupMinion( Object _minion, Path lane, Team team )
	{
		min_go = _minion as GameObject;
		min_go.GetComponent<MinionMovement>().ChangeLane( lane );
		nma = min_go.GetComponent<NavMeshAgent>();
		nma.enabled = true;
		if ( team == Team.BLUE_TEAM )
			nma.destination = RedPortalTransform.position;
		else
			nma.destination = BluePortalTransform.position;
	}

	void SpawnMinionWaves()
	{
		SetupMinion( Instantiate( redCasterMinion, redPortal.LeftSpawn[ 0 ].position, faceDown ), Path.SOUTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redStrikerMinion, redPortal.LeftSpawn[ 3 ].position, faceDown ), Path.SOUTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redStrikerMinion, redPortal.LeftSpawn[ 4 ].position, faceDown ), Path.SOUTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redTankMinion, redPortal.LeftSpawn[ 1 ].position, faceDown ), Path.SOUTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redTankMinion, redPortal.LeftSpawn[ 2 ].position, faceDown ), Path.SOUTH_PATH, Team.RED_TEAM );

		SetupMinion( Instantiate( redCasterMinion, redPortal.MidSpawn[ 0 ].position, faceLeft ), Path.CENTER_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redStrikerMinion, redPortal.MidSpawn[ 3 ].position, faceLeft ), Path.CENTER_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redStrikerMinion, redPortal.MidSpawn[ 4 ].position, faceLeft ), Path.CENTER_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redTankMinion, redPortal.MidSpawn[ 1 ].position, faceLeft ), Path.CENTER_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redTankMinion, redPortal.MidSpawn[ 2 ].position, faceLeft ), Path.CENTER_PATH, Team.RED_TEAM );

		SetupMinion( Instantiate( redCasterMinion, redPortal.RightSpawn[ 0 ].position, faceUp ), Path.NORTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redStrikerMinion, redPortal.RightSpawn[ 3 ].position, faceUp ), Path.NORTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redStrikerMinion, redPortal.RightSpawn[ 4 ].position, faceUp ), Path.NORTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redTankMinion, redPortal.RightSpawn[ 1 ].position, faceUp ), Path.NORTH_PATH, Team.RED_TEAM );
		SetupMinion( Instantiate( redTankMinion, redPortal.RightSpawn[ 2 ].position, faceUp ), Path.NORTH_PATH, Team.RED_TEAM );

		SetupMinion( Instantiate( blueCasterMinion, bluePortal.LeftSpawn[ 0 ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueStrikerMinion, bluePortal.LeftSpawn[ 3 ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueStrikerMinion, bluePortal.LeftSpawn[ 4 ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueTankMinion, bluePortal.LeftSpawn[ 1 ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueTankMinion, bluePortal.LeftSpawn[ 2 ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );

		SetupMinion( Instantiate( blueCasterMinion, bluePortal.MidSpawn[ 0 ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueStrikerMinion, bluePortal.MidSpawn[ 3 ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueStrikerMinion, bluePortal.MidSpawn[ 4 ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueTankMinion, bluePortal.MidSpawn[ 1 ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueTankMinion, bluePortal.MidSpawn[ 2 ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );

		SetupMinion( Instantiate( blueCasterMinion, bluePortal.RightSpawn[ 0 ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueStrikerMinion, bluePortal.RightSpawn[ 3 ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueStrikerMinion, bluePortal.RightSpawn[ 4 ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueTankMinion, bluePortal.RightSpawn[ 1 ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
		SetupMinion( Instantiate( blueTankMinion, bluePortal.RightSpawn[ 2 ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );

		EconomyManager.Instance.NewWave();

		AudioManager.PlaySoundEffect( AudioManager.sfxWaveSpawn );
	}

	const float halfsqrt2 = 0.707106781f;
	static readonly Quaternion
		faceRight = new Quaternion( 0.0f, halfsqrt2, 0.0f, halfsqrt2 ),
		faceLeft = new Quaternion( 0.0f, -halfsqrt2, 0.0f, halfsqrt2 ),
		faceUp = new Quaternion( 0.0f, 0.0f, 0.0f, 1.0f ),
		faceDown = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
	public void InitiateGame()
	{
		gameRunning = true;
		gameEnded = false;
		timer = maxTime;
		waveTimer = waveTime;
		EconomyManager.Instance.StartingGame();
		ResetUpgrades();
	}
	bool gameEnded = false;
	public static bool GameEnded { get { return instance.gameEnded; } }
	public void EndGame()
	{
		gameEnded = true;
		gameRunning = false;
		if ( bluePortal.HP < redPortal.HP )
			ApplicationManager.Instance.ChangeAppState( StateID.STATE_GAME_LOST );
		else if ( bluePortal.HP > redPortal.HP )
			ApplicationManager.Instance.ChangeAppState( StateID.STATE_GAME_WON );
		else
			ApplicationManager.Instance.ChangeAppState( StateID.STATE_GAME_DRAW );
	}

	public void Pause()
	{
		ApplicationManager.Instance.ChangeAppState( StateID.STATE_PAUSED );
		gameRunning = false;
	}

	public void Unpause()
	{
		ApplicationManager.Instance.ChangeAppState( StateID.STATE_INGAME );
		gameRunning = true;
	}

	public void EnterShop()
	{
		ApplicationManager.Instance.ChangeAppState( StateID.STATE_SHOP );
	}

	public void ExitShop()
	{
		ApplicationManager.Instance.ChangeAppState( StateID.STATE_INGAME );
	}

	public static bool eventSystem
	{
		set
		{
			Instance.uiEventSystem.enabled = value;
			foreach ( Button button in Instance.buttons )
				button.enabled = value;
		}
	}

	[SerializeField]
	GameObject defaultHeroPrefab = null;

	static GameManager instance = null;
	public static GameManager Instance
	{
		get
		{
			if ( !instance )
			{
				instance = FindObjectOfType<GameManager>();
				if ( !instance )
					instance = new GameObject( "GameManager" )
						.AddComponent<GameManager>();
			}
			return instance;
		}
	}
	[SerializeField]
	Transform topSplit = null, botSplit = null;
	void Awake()
	{
		if ( instance )
			Destroy( this );
		else
			instance = this;
		abilities = new List<HeroAbilities>();
		heros = new List<HeroInfo>();
		topSplitZ = topSplit.position.z;
		botSplitZ = botSplit.position.z;
		SpawnHero();
	}
	void OnDestroy() { if ( this == instance ) instance = null; }

	void Start()
	{


		InitiateGame();
		BoomBlue.gameObject.SetActive( false );
		BoomRed.gameObject.SetActive( false );
		ExitBlue.gameObject.SetActive( false );
		ExitRed.gameObject.SetActive( false );

	}

	public delegate void GameEndEvent();
	public static event GameEndEvent OnBlueWin, OnRedWin;
	IEnumerator GameEnding()
	{
		gameEnded = true;
		if ( ApplicationManager.Instance.GetAppState() == StateID.STATE_SHOP )
			ExitShop();
		if ( redPortal.HP <= 0.0f )
		{
			if ( OnBlueWin != null )
				OnBlueWin();
			if ( !BoomRed.isPlaying )
				BoomRed.gameObject.SetActive( true );
		}
		else if ( bluePortal.HP <= 0.0f )
		{
			if ( OnRedWin != null )
				OnRedWin();
			if ( !BoomBlue.isPlaying )
				BoomBlue.gameObject.SetActive( true );
		}
		endminions = FindObjectsOfType<MinionInfo>();
		foreach ( MinionInfo endminion in endminions )
			Destroy( endminion.gameObject );

		yield return new WaitForSeconds( 4.0f );
		redPortal.gameObject.SetActive( false );
		bluePortal.gameObject.SetActive( false );


		if ( redPortal.HP <= 0.0f )
		{
			ExitRed.gameObject.SetActive( true );
		}
		else if ( bluePortal.HP <= 0.0f )
		{
			ExitBlue.gameObject.SetActive( true );
		}
		yield return new WaitForSeconds( 4.0f );

		EndGame();

	}

	bool shopActive = false;
	void Update()
	{
		if ( !gameRunning || gameEnded )
			return;
		if ( Input.GetKeyDown( KeyCode.Escape ) && ApplicationManager.Instance.GetAppState() != StateID.STATE_SHOP )
			Pause();
		else if ( Input.GetKeyDown( KeyCode.Tab ) )
		{
			if ( !shopActive )
			{
				shopActive = true;
				EnterShop();
			}
			else
			{
				shopActive = false;
				ExitShop();
			}

		}
		else if ( Input.GetKeyDown( KeyCode.Escape ) && ApplicationManager.Instance.GetAppState() == StateID.STATE_SHOP )
			ExitShop();
		else if ( timer <= 0.0f || redPortal.HP <= 0.0f || bluePortal.HP <= 0.0f )
		{ StartCoroutine( GameEnding() ); return; }
		timer -= Time.deltaTime;
		waveTimer -= Time.deltaTime;
		if ( waveTimer <= 0.0f )
		{
			waveTimer = waveTime;
			++wave;
			SpawnMinionWaves();
		}
		DoAbilityCooldowns();
		RespawnTimers();
	}

	public float Timer { get { return timer; } }
	public float WaveTimer { get { return waveTimer; } }
	public int Wave { get { return wave; } }


	void DoAbilityCooldowns()
	{
		foreach ( HeroAbilities abs in abilities )
		{
			abs.abilityQ.Timer -= Time.deltaTime;
			abs.abilityW.Timer -= Time.deltaTime;
			abs.abilityE.Timer -= Time.deltaTime;
			abs.abilityR.Timer -= Time.deltaTime;
		}
	}

	List<HeroInfo> heros = null;

	public void AddHero( HeroInfo info )
	{
		heros.Add( info );
	}
	void RespawnTimers()
	{
		foreach ( HeroInfo hero in heros )
		{
			hero.RespawnTimer -= Time.deltaTime;
			if ( hero.waitingRespawn )
				hero.Respawn();
		}
	}

	public static Vector3 blueHeroSpawnPosition { get { return Instance.HeroSpawnPoint.position; } }
	[SerializeField]
	List<Button> buttons = null;

	void ResetUpgrades()
	{
		MinionInfo info;

		info = blueCasterMinion.GetComponent<MinionInfo>();
		info.MAXHP = 290.0f;
		info.AttackSpeed = 0.7f;
		info.Range = 50.0f;
		info.Damage = 30.0f;

		info = blueTankMinion.GetComponent<MinionInfo>();
		info.MAXHP = 515.0f;
		info.AttackSpeed = 0.8f;
		info.Damage = 60.0f;

		info = blueStrikerMinion.GetComponent<MinionInfo>();
		info.MAXHP = 380.0f;
		info.AttackSpeed = 0.8f;
		info.Damage = 50.0f;
	}

	public List<HeroInfo> Heros { get { return heros; } }

	public void InstaRespawn( Team team, HeroInfo hero )
	{
		if ( hero.team == team && !hero.Alive )
			hero.RespawnTimer = 0.0f;
	}

	[SerializeField]
	ResourceBarScript heroHealthPanel = null, heroManaPanel = null;
	public void SpawnHero()
	{
		if ( CharacterSelectionManager.Instance )
			Player = Instantiate( CharacterSelectionManager.LegendChoice, HeroSpawnPoint.position, faceRight ) as GameObject;
		else
			Player = Instantiate( defaultHeroPrefab, HeroSpawnPoint.position, faceRight ) as GameObject;
		heroHealthPanel.Host = heroManaPanel.Host = Player;
		if ( tutorial )
		{
			HeroAbilities abilities = Player.GetComponent<HeroAbilities>();
			abilities.abilityW.abilityEnabled = false;
			abilities.abilityE.abilityEnabled = false;
			abilities.abilityR.abilityEnabled = false;
		}
	}

	public void SpawnStrikerMinion( Team team, int lane )
	{
		if ( team == Team.BLUE_TEAM )
		{
			switch ( lane )
			{
				case 1:
					SetupMinion( Instantiate( blueStrikerMinion, bluePortal.LeftSpawn[ Random.Range( 0, 5 ) ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
					break;
				case 2:
					SetupMinion( Instantiate( blueStrikerMinion, bluePortal.MidSpawn[ Random.Range( 0, 5 ) ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
					break;
				case 3:
					SetupMinion( Instantiate( blueStrikerMinion, bluePortal.RightSpawn[ Random.Range( 0, 5 ) ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
					break;
				default:
					return;
			}
		}
	}

	public void SpawnTankMinion( Team team, int lane )
	{
		if ( team == Team.BLUE_TEAM )
		{
			switch ( lane )
			{
				case 1:
					SetupMinion( Instantiate( blueTankMinion, bluePortal.LeftSpawn[ Random.Range( 0, 5 ) ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
					break;
				case 2:
					SetupMinion( Instantiate( blueTankMinion, bluePortal.MidSpawn[ Random.Range( 0, 5 ) ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
					break;
				case 3:
					SetupMinion( Instantiate( blueTankMinion, bluePortal.RightSpawn[ Random.Range( 0, 5 ) ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
					break;
				default:
					return;
			}
		}
	}

	public void SpawnCasterMinion( Team team, int lane )
	{
		if ( team == Team.BLUE_TEAM )
		{
			switch ( lane )
			{
				case 1:
					SetupMinion( Instantiate( blueCasterMinion, bluePortal.LeftSpawn[ Random.Range( 0, 5 ) ].position, faceUp ), Path.NORTH_PATH, Team.BLUE_TEAM );
					break;
				case 2:
					SetupMinion( Instantiate( blueCasterMinion, bluePortal.MidSpawn[ Random.Range( 0, 5 ) ].position, faceRight ), Path.CENTER_PATH, Team.BLUE_TEAM );
					break;
				case 3:
					SetupMinion( Instantiate( blueCasterMinion, bluePortal.RightSpawn[ Random.Range( 0, 5 ) ].position, faceDown ), Path.SOUTH_PATH, Team.BLUE_TEAM );
					break;
				default:
					return;
			}
		}
	}

	public void UpgradeStrikerMinions( Team team )
	{
		if ( team == Team.BLUE_TEAM )
		{
			MinionInfo info = blueStrikerMinion.GetComponent<MinionInfo>();
			info.Damage += ShopManager.Instance.strikerDamageUpgrade;
			info.MAXHP += ShopManager.Instance.strikerHpUpgrade;
			info.AttackSpeed += ShopManager.Instance.strikerAttackspeedUpgrade;
		}
		else
		{
			MinionInfo info = redStrikerMinion.GetComponent<MinionInfo>();
			info.Damage += ShopManager.Instance.strikerDamageUpgrade;
			info.MAXHP += ShopManager.Instance.strikerHpUpgrade;
			info.AttackSpeed += ShopManager.Instance.strikerAttackspeedUpgrade;
		}
	}

	public void UpgradeTankMinions( Team team )
	{
		if ( team == Team.BLUE_TEAM )
		{
			MinionInfo info = blueTankMinion.GetComponent<MinionInfo>();
			info.Damage += ShopManager.Instance.tankDamageUpgrade;
			info.MAXHP += ShopManager.Instance.tankHpUpgrade;
			info.AttackSpeed += ShopManager.Instance.tankAttackspeedUpgrade;
		}
		else
		{
			MinionInfo info = redTankMinion.GetComponent<MinionInfo>();
			info.Damage += ShopManager.Instance.tankDamageUpgrade;
			info.MAXHP += ShopManager.Instance.tankHpUpgrade;
			info.AttackSpeed += ShopManager.Instance.tankAttackspeedUpgrade;
		}
	}

	public void UpgradeCasterMinions( Team team )
	{
		if ( team == Team.BLUE_TEAM )
		{
			MinionInfo info = blueCasterMinion.GetComponent<MinionInfo>();
			info.Damage += ShopManager.Instance.casterDamageUpgrade;
			info.MAXHP += ShopManager.Instance.casterHpUpgrade;
			info.AttackSpeed += ShopManager.Instance.casterAttackspeedUpgrade;
			info.Range += ShopManager.Instance.casterRangeUpgrade;
		}
		else
		{
			MinionInfo info = redCasterMinion.GetComponent<MinionInfo>();
			info.Damage += ShopManager.Instance.casterDamageUpgrade;
			info.MAXHP += ShopManager.Instance.casterHpUpgrade;
			info.AttackSpeed += ShopManager.Instance.casterAttackspeedUpgrade;
			info.Range += ShopManager.Instance.casterRangeUpgrade;
		}
	}
}