using UnityEngine;
using System.Collections.Generic;

public class EnemyAIManager : MonoBehaviour {
	public enum HeroLocation {TOP_Lane, MID_Lane, BOT_Lane,Close, TOO_Close, Unknown};

	[Header("Needed stuff")]
	public PortalInfo redPortal = null;
	public PortalInfo bluePortal = null;
	public GameObject UpperSplit = null;
	public GameObject LowerSplit = null;
	public GameObject portalLazer = null;
	public BcWeapon lazer = null;
	public GameObject lastResortParticle = null;
	public Detector attackRange = null;

	[Header("Logic variables")]
	[Range(0.0f,100.0f)]
	public float dangerTreshold = 0.0f;
	[Range(0,6)]
	public int towersRemaining = 6;
	[Range(0.0f,5000.0f)]
	public float remainingHealth = 0;
	public HeroLocation heroPresence = HeroLocation.Unknown;


	[Header("Timers")]
	[Range(0.0f, 80.0f)]
	public float siegeTimer = 0.0f;
	[Range(0.0f, 80.0f)]
	public float reinforcementsTimer = 0.0f;
	[Range(0.0f, 30.0f)]
	public float lastResortTimer = 3.0f;

#if DEBUG
	[Header("Enabled Behaviors")]
	public bool m_spawnSiegeMinion = false;
	public bool m_reinforcements = false;
	public bool m_towerMovement = false;
	public bool m_selfRecover = false;
	public bool m_huntHero = false;
	public bool m_upgradeSiege = false;
	public bool m_extraSiegeMinion = false;
	public bool m_lastResort = false;
#endif

	public static bool spawnSiegeMinion = false;
	public static bool reinforcements = false;
	public static bool towerMovement = false;
	public static bool selfRecover = false;
	public static bool huntHero = false;
	public static bool upgradeSiege = false;
	public static bool extraSiegeMinion = false;
	public static bool lastResort = false;


	[Header("Minion Prefabs")]  
	public GameObject siegeMinion = null;
	public GameObject redStriker = null;
	public GameObject redTank = null;
	public GameObject redCaster = null;


	float reinforcementsTime = 40;
	float siegeTime= 60.0f;
	float lastResortTime = 20.0f;
	bool  LastResortActive = false; 
	float towerCost = 9f;
	float healthCost = 50f;
	float timeCost = 40.0f;
	float selfRecoveryBase= 20.0f;
	float selfRecoveryGrowth = 8.0f;
	float maxTime = 900.0f;
	GameObject min_go;
	NavMeshAgent nma;
	const float halfsqrt2 = 0.707106781f;
	static readonly Quaternion
		faceLeft = new Quaternion( 0.0f, -halfsqrt2, 0.0f, halfsqrt2 ),
		faceUp = new Quaternion( 0.0f, 0.0f, 0.0f, 1.0f ),
		faceDown = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
	List<Transform> targets = new List<Transform>();
	GameObject Hero = null;


	float CalcSelfRecovery(){
		return (selfRecoveryBase + selfRecoveryGrowth*((((redPortal.MAXHP- redPortal.HP)/redPortal.MAXHP) * selfRecoveryBase) + (0.0175f * (maxTime - GameManager.Instance.Timer))));
	}

	void Start(){
		remainingHealth = redPortal.HP;
		Hero = GameManager.Instance.Player;
		attackRange.triggerEnter += AttackRange_triggerEnter;
		attackRange.triggerExit += AttackRange_triggerExit;
		attackRange.CreateTrigger(210.0f);
	}

	void AttackRange_triggerExit (GameObject obj)
	{
		targets.Remove(obj.transform);
	}

	void AttackRange_triggerEnter (GameObject obj)
	{
		if (redPortal.Alive) {
			if (obj && obj.CompareTag("Minion"))
				if (obj.GetComponent<Info>().team == Team.BLUE_TEAM)
					targets.Add(obj.transform);
		}
	}

	public void Destroyed(){
		--towersRemaining;
	}

	void Update(){
#if DEBUG
	 m_spawnSiegeMinion = spawnSiegeMinion;
     m_reinforcements =     reinforcements ;
     m_towerMovement =      towerMovement ;
     m_selfRecover =       selfRecover ;
     m_huntHero =           huntHero ;
     m_upgradeSiege =      upgradeSiege ;
     m_extraSiegeMinion =   extraSiegeMinion ;
	m_lastResort = lastResort;
	#endif
		if (!Hero.activeSelf)
			lazer.autofire = false;
		remainingHealth = redPortal.HP;
		dangerTreshold = ((timeCost - ((GameManager.Instance.Timer/ maxTime)*timeCost)) + (healthCost - ((redPortal.HP/redPortal.MAXHP)* healthCost)) + (5 - towersRemaining) * towerCost);

		lazer.bulletPrefab.GetComponent<SiegeProjectile>().damage = GetComponentInParent<PortalInfo>().Damage * towersRemaining;
		if (LastResortActive == false)
			switch (GetTriggered(dangerTreshold)) {
				case DangerLevel.EXTREME:
					selfRecover = true;
					spawnSiegeMinion = true;
					reinforcements = true;
					towerMovement = true;
					huntHero = true;
					upgradeSiege = true;
					extraSiegeMinion = true;
					lastResort = true;
					reinforcementsTime = 10;
					siegeTime = 10;
					selfRecoveryBase = 30;
					break;
				case DangerLevel.CRITICAL:
					selfRecover = true;
					spawnSiegeMinion = true;
					reinforcements = true;
					towerMovement = true;
					huntHero = true;
					upgradeSiege = true;
					extraSiegeMinion = true;
					lastResort = false;
					reinforcementsTime = 20;
					siegeTime = 15;
					selfRecoveryBase = 20;
					break;
				case DangerLevel.HIGH:
					selfRecover = true;
					spawnSiegeMinion = true;
					reinforcements = true;
					towerMovement = true;
					huntHero = false;
					upgradeSiege = true;
					extraSiegeMinion = true;
					lastResort = false;
					reinforcementsTime = 40;
					siegeTime = 25;
					selfRecoveryBase = 15;
					break;
				case DangerLevel.MODERATE:
					selfRecover = true;
					spawnSiegeMinion = true;
					reinforcements = true;
					towerMovement = true;
					huntHero = false;
					upgradeSiege = true;
					extraSiegeMinion = false;
					lastResort = false;
					reinforcementsTime = 60;
					siegeTime = 40;
					selfRecoveryBase = 10;
					break;
				case DangerLevel.MEDIUM:
					selfRecover = false;
					spawnSiegeMinion = true;
					reinforcements = true;
					towerMovement = true;
					huntHero = false;
					upgradeSiege = false;
					extraSiegeMinion = false;
					lastResort = false;
					reinforcementsTime = 80;
					siegeTime = 60;
					break;
				case DangerLevel.LOW:
					selfRecover = false;
					spawnSiegeMinion = true;
					reinforcements = false;
					towerMovement = false;
					huntHero = false;
					upgradeSiege = false;
					extraSiegeMinion = false;
					lastResort = false;
					reinforcementsTime = 80;
					siegeTime = 80;
					break;
				case DangerLevel.MINIMAL:
					selfRecover = false;
					spawnSiegeMinion = false;
					reinforcements = false;
					towerMovement = false;
					huntHero = false;
					upgradeSiege = false;
					extraSiegeMinion = false;
					lastResort = false;
					reinforcementsTime = 80;
					siegeTime = 80;
					break;
				default:
					break;
			}
	
		if(spawnSiegeMinion) 
			siegeTimer -= Time.deltaTime;
		
		if (reinforcements)
			reinforcementsTimer -= Time.deltaTime;
		
		if (lastResort && lastResortTimer >=15.0f)
			LastResortActive = true;
		
		if (redPortal.Alive && selfRecover) {
			redPortal.HP = remainingHealth + (CalcSelfRecovery() * Time.deltaTime);
			if (LastResortActive) {
				lastResortParticle.GetComponent<ParticleSystem>().Play();
				lastResortTimer -= Time.deltaTime;
				selfRecoveryBase *= 3;
				redPortal.HP = remainingHealth + (CalcSelfRecovery() * Time.deltaTime);
				if (lastResortTimer <= 0.0f)
					LastResortActive = false;
			}
			else {
				lastResortParticle.GetComponent<ParticleSystem>().Stop();
				lastResortParticle.GetComponent<ParticleSystem>().Clear();
				lastResortTimer = System.Math.Min(lastResortTime, (lastResortTimer + Time.deltaTime*0.6f));
			}
		}
		if (spawnSiegeMinion && siegeTimer <= 0) {
			
			SpawnSiegeMinion(Team.RED_TEAM, heroPresence);
			if(extraSiegeMinion)
				SpawnSiegeMinion(Team.RED_TEAM, heroPresence);
			siegeTimer = siegeTime;
		}
		
		if (reinforcements && reinforcementsTimer <= 0) {
			SetupMinion(Instantiate(redCaster, redPortal.LeftSpawn[0].position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
			SetupMinion(Instantiate(redStriker, redPortal.LeftSpawn[3].position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
			SetupMinion(Instantiate(redTank, redPortal.LeftSpawn[1].position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);

			SetupMinion(Instantiate(redCaster, redPortal.MidSpawn[0].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
			SetupMinion(Instantiate(redStriker, redPortal.MidSpawn[4].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
			SetupMinion(Instantiate(redTank, redPortal.MidSpawn[2].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);

			SetupMinion(Instantiate(redCaster, redPortal.RightSpawn[0].position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
			SetupMinion(Instantiate(redStriker, redPortal.RightSpawn[4].position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
			SetupMinion(Instantiate(redTank, redPortal.RightSpawn[2].position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
			reinforcementsTimer = reinforcementsTime;
		}


		// Locate the hero
		if (Vector3.Distance(Hero.transform.position, redPortal.gameObject.transform.position) <= 130.0f)
			heroPresence = HeroLocation.TOO_Close;
		if (Vector3.Distance(Hero.transform.position, redPortal.gameObject.transform.position) <= 210.0f)
			heroPresence = HeroLocation.Close;
		else if (Hero.transform.position.z > UpperSplit.transform.position.z)
			heroPresence = HeroLocation.TOP_Lane;
		else if (Hero.transform.position.z < LowerSplit.transform.position.z)
				heroPresence = HeroLocation.BOT_Lane;
			else
				heroPresence = HeroLocation.MID_Lane;
		// if the hero is close, increase defence
		if (heroPresence == HeroLocation.TOO_Close) {
			redPortal.DmgDamp = 16.555f * towersRemaining;
		}
		else
			redPortal.DmgDamp = 10 * towersRemaining;


	}
	enum DangerLevel{ MINIMAL, LOW, MEDIUM, MODERATE, HIGH, CRITICAL, EXTREME}; 

	DangerLevel GetTriggered(float _num){
		if (dangerTreshold >= 90)
			return DangerLevel.EXTREME;
		else if (dangerTreshold >= 75)
				return DangerLevel.CRITICAL;
			else if (_num >= 60)
					return DangerLevel.HIGH;
				else if (_num >= 45)
						return DangerLevel.MODERATE;
					else if (_num >= 30)
							return DangerLevel.MEDIUM;
						else if (_num >= 12)
								return DangerLevel.LOW;
							else
								return DangerLevel.MINIMAL;
	}
#if DEBUG		
	public float temp;
	float second = 1.0f;
	public float temp2 = 0.0f;
#endif
	void FixedUpdate(){
#if DEBUG
		second -= Time.fixedDeltaTime;
		temp += ((CalcSelfRecovery()) * Time.fixedDeltaTime);
		if (second <= 0.0f) {
			temp2 = temp;
			second = 1.0f;
			temp = 0.0f;
		}
#endif
		if (lazer == null)
			return;
		if (targets.Count == 0 || targets[0] == null || !targets[0].gameObject.GetComponent<Info>().Alive) {
			Nil();
			if (targets.Count >= 1 && !targets[0].gameObject.GetComponent<Info>().Alive)
				AttackRange_triggerExit(targets[0].gameObject);
		}
		if (heroPresence == HeroLocation.TOO_Close && Hero.GetComponent<Info>().Alive) {
			portalLazer.transform.LookAt(Hero.transform);
			lazer.autofire = true;
		}
		else if (huntHero && heroPresence == HeroLocation.Close && Hero.GetComponent<Info>().Alive) {
				portalLazer.transform.LookAt(Hero.transform);
				lazer.autofire = true;
			}
			else if (targets.Count > 0) {
					portalLazer.transform.LookAt(targets[0]);
					lazer.autofire = true;
				}
				else if (heroPresence == HeroLocation.Close && Hero.GetComponent<Info>().Alive) {
						portalLazer.transform.LookAt(Hero.transform);
						lazer.autofire = true;
					}
					else
						lazer.autofire = false;
	}

	void Nil(){
		for (int i = 0; i < targets.Count; ++i)
			if (targets[i] && targets[i].gameObject.activeInHierarchy)
				continue;
			else {
				targets.RemoveAt(i--);
			}
	}

	void SetupMinion(Object _minion, Path lane, Team team)
	{
		min_go = _minion as GameObject;
		min_go.GetComponent<MinionMovement>().ChangeLane(lane);
		nma=min_go.GetComponent<NavMeshAgent>();
		nma.enabled = true;
		nma.destination = bluePortal.gameObject.transform.position;
	}

	public void SpawnTankMinion(Team team, int lane)
	{
		switch (lane) {
			case 1:
				SetupMinion(Instantiate(redTank, redPortal.LeftSpawn[Random.Range(0, 5)].position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
				break;
			case 2:
				SetupMinion(Instantiate(redTank, redPortal.MidSpawn[Random.Range(0, 5)].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
				break;
			case 3:
				SetupMinion(Instantiate(redTank, redPortal.RightSpawn[Random.Range(0, 5)].position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
				break;
			default:
				return;
		}
	}

	public void SpawnCasterMinion(Team team, int lane)
	{
		switch (lane) {
			case 1:
				SetupMinion(Instantiate(redCaster, redPortal.LeftSpawn[Random.Range(0, 5)].position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
				break;
			case 2:
				SetupMinion(Instantiate(redCaster, redPortal.MidSpawn[Random.Range(0, 5)].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
				break;
			case 3:
				SetupMinion(Instantiate(redCaster, redPortal.RightSpawn[Random.Range(0, 5)].position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
				break;
			default:
				return;
		}
	}
	public void SpawnStrikerMinion(Team team, int lane)
	{
		switch (lane) {
			case 1:
				SetupMinion(Instantiate(redStriker, redPortal.LeftSpawn[Random.Range(0, 5)].position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
				break;
			case 2:
				SetupMinion(Instantiate(redStriker, redPortal.MidSpawn[Random.Range(0, 5)].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
				break;
			case 3:
				SetupMinion(Instantiate(redStriker, redPortal.RightSpawn[Random.Range(0, 5)].position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
				break;
			default:
				return;
		}
	}
	public void SpawnSiegeMinion(Team team, HeroLocation lane)
	{
		switch (lane) {
			case HeroLocation.BOT_Lane:
				SetupMinion(Instantiate(siegeMinion, redPortal.LeftSpawn[Random.Range(0, 5)].position, faceUp), Path.SOUTH_PATH, Team.RED_TEAM);
				break;
			case HeroLocation.MID_Lane:
				SetupMinion(Instantiate(siegeMinion, redPortal.MidSpawn[Random.Range(0, 5)].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
				break;
			case HeroLocation.TOP_Lane:
				SetupMinion(Instantiate(siegeMinion, redPortal.RightSpawn[Random.Range(0, 5)].position, faceDown), Path.NORTH_PATH, Team.RED_TEAM);
				break;
			default:
				SetupMinion(Instantiate(siegeMinion, redPortal.MidSpawn[Random.Range(0, 5)].position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
				return;
		}
	}
}
