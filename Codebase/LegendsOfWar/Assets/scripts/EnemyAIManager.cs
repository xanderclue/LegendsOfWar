using UnityEngine;
using System.Collections.Generic;
public enum HeroLocation { TOP_Lane, MID_Lane, BOT_Lane, Close, TOO_Close, Unknown }
public enum DangerLevel { MINIMAL, LOW, MEDIUM, MODERATE, HIGH, CRITICAL, EXTREME }
public class EnemyAIManager : MonoBehaviour
{
    [Header("Needed stuff")]
    public PortalInfo redPortal = null, bluePortal = null;
    public GameObject UpperSplit = null, LowerSplit = null, portalLazer = null;
    public BcWeapon lazer = null;
    public GameObject lastResortParticle = null;
    public Detector attackRange = null;
    [Header("Logic variables")]
    [Range(0.0f, 100.0f)]
    public float dangerTreshold = 0.0f;
    [Range(0, 6)]
    public int towersRemaining = 6;
    [Range(0.0f, 5000.0f)]
    public float remainingHealth = 0.0f;
    public HeroLocation heroPresence = HeroLocation.Unknown;
    [Header("Timers")]
    [Range(0.0f, 80.0f)]
    public float siegeTimer = 0.0f, reinforcementsTimer = 0.0f;
    [Range(0.0f, 30.0f)]
    public float lastResortTimer = 3.0f;
#if DEBUG
    [Header("Enabled Behaviors")]
    public bool m_spawnSiegeMinion = false, m_reinforcements = false, m_towerMovement = false,
        m_selfRecover = false, m_huntHero = false, m_upgradeSiege = false, m_extraSiegeMinion =
        false, m_lastResort = false;
#endif
    [Header("Minion Prefabs")]
    public GameObject siegeMinion = null, redStriker = null, redTank = null, redCaster = null;
#if DEBUG
    public float temp, temp2 = 0.0f;
#endif
    private static readonly Quaternion faceLeft = new Quaternion(0.0f, -0.707106781f, 0.0f,
        0.707106781f), faceUp = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f), faceDown = new Quaternion
        (0.0f, 1.0f, 0.0f, 0.0f);
    public static bool spawnSiegeMinion = false, reinforcements = false, towerMovement = false,
        selfRecover = false, huntHero = false, upgradeSiege = false, extraSiegeMinion = false,
        lastResort = false;
    private List<Transform> targets = new List<Transform>();
    private GameObject min_go, Hero = null;
    private ParticleSystem lrpPs;
    private UnityEngine.AI.NavMeshAgent nma;
    private PortalInfo portalInfo;
    private HeroInfo heroInfo;
    private float reinforcementsTime = 40.0f, siegeTime = 60.0f, selfRecoveryBase = 20.0f
#if DEBUG
        , second = 1.0f
#endif
        ;
    private bool LastResortActive = false;
    private float CalcSelfRecovery
    {
        get
        {
            return selfRecoveryBase + 8.0f * ((redPortal.MAXHP - redPortal.HP) * redPortal.
                InvMAXHP * selfRecoveryBase - 0.0175f * GameManager.Instance.Timer + 15.75f);
        }
    }
    private DangerLevel GetTriggered
    {
        get
        {
            if (dangerTreshold >= 90.0f)
                return DangerLevel.EXTREME;
            else if (dangerTreshold >= 75.0f)
                return DangerLevel.CRITICAL;
            else if (dangerTreshold >= 60.0f)
                return DangerLevel.HIGH;
            else if (dangerTreshold >= 45.0f)
                return DangerLevel.MODERATE;
            else if (dangerTreshold >= 30.0f)
                return DangerLevel.MEDIUM;
            else if (dangerTreshold >= 12.0f)
                return DangerLevel.LOW;
            return DangerLevel.MINIMAL;
        }
    }
    public void SpawnTankMinion(Team team, int lane)
    {
        switch (lane)
        {
            case 1:
                SetupMinion(Instantiate(redTank, redPortal.LeftSpawn[Random.Range(0, 5)].
                    position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
                break;
            case 2:
                SetupMinion(Instantiate(redTank, redPortal.MidSpawn[Random.Range(0, 5)].
                    position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
                break;
            case 3:
                SetupMinion(Instantiate(redTank, redPortal.RightSpawn[Random.Range(0, 5)].
                    position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
                break;
            default:
                return;
        }
    }
    public void SpawnCasterMinion(Team team, int lane)
    {
        switch (lane)
        {
            case 1:
                SetupMinion(Instantiate(redCaster, redPortal.LeftSpawn[Random.Range(0, 5)].
                    position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
                break;
            case 2:
                SetupMinion(Instantiate(redCaster, redPortal.MidSpawn[Random.Range(0, 5)].
                    position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
                break;
            case 3:
                SetupMinion(Instantiate(redCaster, redPortal.RightSpawn[Random.Range(0, 5)].
                    position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
                break;
            default:
                return;
        }
    }
    public void SpawnStrikerMinion(Team team, int lane)
    {
        switch (lane)
        {
            case 1:
                SetupMinion(Instantiate(redStriker, redPortal.LeftSpawn[Random.Range(0, 5)].
                    position, faceUp), Path.NORTH_PATH, Team.RED_TEAM);
                break;
            case 2:
                SetupMinion(Instantiate(redStriker, redPortal.MidSpawn[Random.Range(0, 5)].
                    position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
                break;
            case 3:
                SetupMinion(Instantiate(redStriker, redPortal.RightSpawn[Random.Range(0, 5)].
                    position, faceDown), Path.SOUTH_PATH, Team.RED_TEAM);
                break;
            default:
                return;
        }
    }
    public void SpawnSiegeMinion(Team team, HeroLocation lane)
    {
        switch (lane)
        {
            case HeroLocation.BOT_Lane:
                SetupMinion(Instantiate(siegeMinion, redPortal.LeftSpawn[Random.Range(0, 5)].
                    position, faceUp), Path.SOUTH_PATH, Team.RED_TEAM);
                break;
            case HeroLocation.TOP_Lane:
                SetupMinion(Instantiate(siegeMinion, redPortal.RightSpawn[Random.Range(0, 5)].
                    position, faceDown), Path.NORTH_PATH, Team.RED_TEAM);
                break;
            default:
                SetupMinion(Instantiate(siegeMinion, redPortal.MidSpawn[Random.Range(0, 5)].
                    position, faceLeft), Path.CENTER_PATH, Team.RED_TEAM);
                return;
        }
    }
    public void Destroyed()
    {
        --towersRemaining;
    }
    private void Start()
    {
        remainingHealth = redPortal.HP;
        Hero = GameManager.Instance.Player;
        heroInfo = Hero.GetComponent<HeroInfo>();
        attackRange.triggerEnter += AttackRange_triggerEnter;
        attackRange.triggerExit += AttackRange_triggerExit;
        attackRange.CreateTrigger(210.0f);
        lrpPs = lastResortParticle.GetComponent<ParticleSystem>();
        portalInfo = GetComponentInParent<PortalInfo>();
    }
    private void Update()
    {
#if DEBUG
        m_spawnSiegeMinion = spawnSiegeMinion;
        m_reinforcements = reinforcements;
        m_towerMovement = towerMovement;
        m_selfRecover = selfRecover;
        m_huntHero = huntHero;
        m_upgradeSiege = upgradeSiege;
        m_extraSiegeMinion = extraSiegeMinion;
        m_lastResort = lastResort;
#endif
        if (!Hero.activeSelf)
            lazer.autofire = false;
        remainingHealth = redPortal.HP;
        dangerTreshold = 90.0f - GameManager.Instance.Timer * 0.044444444f - redPortal.HP *
            redPortal.InvMAXHP * 50.0f + (5 - towersRemaining) * 9.0f;
        lazer.siegeProjectileDamage = portalInfo.Damage * towersRemaining;
        if (!LastResortActive)
            switch (GetTriggered)
            {
                case DangerLevel.EXTREME:
                    lastResort = extraSiegeMinion = upgradeSiege = huntHero = towerMovement =
                        reinforcements = spawnSiegeMinion = selfRecover = true;
                    siegeTime = reinforcementsTime = 10.0f;
                    selfRecoveryBase = 30.0f;
                    break;
                case DangerLevel.CRITICAL:
                    extraSiegeMinion = upgradeSiege = huntHero = towerMovement = reinforcements =
                        spawnSiegeMinion = selfRecover = true;
                    lastResort = false;
                    reinforcementsTime = 20.0f;
                    siegeTime = 15.0f;
                    selfRecoveryBase = 20.0f;
                    break;
                case DangerLevel.HIGH:
                    towerMovement = reinforcements = spawnSiegeMinion = selfRecover = true;
                    huntHero = false;
                    extraSiegeMinion = upgradeSiege = true;
                    lastResort = false;
                    reinforcementsTime = 40.0f;
                    siegeTime = 25.0f;
                    selfRecoveryBase = 15.0f;
                    break;
                case DangerLevel.MODERATE:
                    towerMovement = reinforcements = spawnSiegeMinion = selfRecover = true;
                    huntHero = false;
                    upgradeSiege = true;
                    lastResort = extraSiegeMinion = false;
                    reinforcementsTime = 60.0f;
                    siegeTime = 40.0f;
                    selfRecoveryBase = 10.0f;
                    break;
                case DangerLevel.MEDIUM:
                    selfRecover = false;
                    towerMovement = reinforcements = spawnSiegeMinion = true;
                    lastResort = extraSiegeMinion = upgradeSiege = huntHero = false;
                    reinforcementsTime = 80.0f;
                    siegeTime = 60.0f;
                    break;
                case DangerLevel.LOW:
                    selfRecover = false;
                    spawnSiegeMinion = true;
                    lastResort = extraSiegeMinion = upgradeSiege = huntHero = towerMovement =
                        reinforcements = false;
                    siegeTime = reinforcementsTime = 80.0f;
                    break;
                case DangerLevel.MINIMAL:
                    lastResort = extraSiegeMinion = upgradeSiege = huntHero = towerMovement =
                        reinforcements = spawnSiegeMinion = selfRecover = false;
                    siegeTime = reinforcementsTime = 80.0f;
                    break;
                default:
                    break;
            }
        if (spawnSiegeMinion)
            siegeTimer -= Time.deltaTime;
        if (reinforcements)
            reinforcementsTimer -= Time.deltaTime;
        if (lastResort && lastResortTimer >= 15.0f)
            LastResortActive = true;
        if (redPortal.Alive && selfRecover)
        {
            redPortal.HP = remainingHealth + CalcSelfRecovery * Time.deltaTime;
            if (LastResortActive)
            {
                lrpPs.Play();
                lastResortTimer -= Time.deltaTime;
                selfRecoveryBase *= 3.0f;
                redPortal.HP = remainingHealth + CalcSelfRecovery * Time.deltaTime;
                if (lastResortTimer <= 0.0f)
                    LastResortActive = false;
            }
            else
            {
                lrpPs.Stop();
                lrpPs.Clear();
                lastResortTimer = System.Math.Min(20.0f, lastResortTimer + 0.6f * Time.deltaTime);
            }
        }
        if (spawnSiegeMinion && siegeTimer <= 0.0f)
        {
            SpawnSiegeMinion(Team.RED_TEAM, heroPresence);
            if (extraSiegeMinion)
                SpawnSiegeMinion(Team.RED_TEAM, heroPresence);
            siegeTimer = siegeTime;
        }
        if (reinforcements && reinforcementsTimer <= 0.0f)
        {
            SetupMinion(Instantiate(redCaster, redPortal.LeftSpawn[0].position, faceDown), Path
                .SOUTH_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redStriker, redPortal.LeftSpawn[3].position, faceDown),
                Path.SOUTH_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redTank, redPortal.LeftSpawn[1].position, faceDown), Path.
                SOUTH_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redCaster, redPortal.MidSpawn[0].position, faceLeft), Path.
                CENTER_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redStriker, redPortal.MidSpawn[4].position, faceLeft), Path
                .CENTER_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redTank, redPortal.MidSpawn[2].position, faceLeft), Path.
                CENTER_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redCaster, redPortal.RightSpawn[0].position, faceUp), Path.
                NORTH_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redStriker, redPortal.RightSpawn[4].position, faceUp), Path
                .NORTH_PATH, Team.RED_TEAM);
            SetupMinion(Instantiate(redTank, redPortal.RightSpawn[2].position, faceUp), Path.
                NORTH_PATH, Team.RED_TEAM);
            reinforcementsTimer = reinforcementsTime;
        }
        if (Vector3.Distance(Hero.transform.position, redPortal.transform.position) <= 210.0f)
            heroPresence = HeroLocation.Close;
        else if (Hero.transform.position.z > UpperSplit.transform.position.z)
            heroPresence = HeroLocation.TOP_Lane;
        else if (Hero.transform.position.z < LowerSplit.transform.position.z)
            heroPresence = HeroLocation.BOT_Lane;
        else
            heroPresence = HeroLocation.MID_Lane;
        redPortal.DmgDamp = 10.0f * towersRemaining;
    }
    private void FixedUpdate()
    {
#if DEBUG
        second -= Time.fixedDeltaTime;
        temp += CalcSelfRecovery * Time.fixedDeltaTime;
        if (second <= 0.0f)
        {
            temp2 = temp;
            second = 1.0f;
            temp = 0.0f;
        }
#endif
        if (!lazer)
            return;
        if (0 == targets.Count || !targets[0] || !targets[0].gameObject.GetComponent<Info>().
            Alive)
        {
            for (int i = 0; i < targets.Count; ++i)
                if (!(targets[i] && targets[i].gameObject.activeInHierarchy))
                    targets.RemoveAt(i--);
            if (targets.Count >= 1 && !targets[0].gameObject.GetComponent<Info>().Alive)
                AttackRange_triggerExit(targets[0].gameObject);
        }
        if ((HeroLocation.TOO_Close == heroPresence || huntHero && HeroLocation.Close ==
            heroPresence) && heroInfo.Alive)
        {
            portalLazer.transform.LookAt(Hero.transform);
            lazer.autofire = true;
        }
        else if (targets.Count > 0)
        {
            portalLazer.transform.LookAt(targets[0]);
            lazer.autofire = true;
        }
        else if (HeroLocation.Close == heroPresence && heroInfo.Alive)
        {
            portalLazer.transform.LookAt(Hero.transform);
            lazer.autofire = true;
        }
        else
            lazer.autofire = false;
    }
    private void AttackRange_triggerExit(GameObject obj)
    {
        targets.Remove(obj.transform);
    }
    private void AttackRange_triggerEnter(GameObject obj)
    {
        if (redPortal.Alive)
            if (obj && obj.CompareTag("Minion"))
                if (Team.BLUE_TEAM == obj.GetComponent<Info>().team)
                    targets.Add(obj.transform);
    }
    private void SetupMinion(Object _minion, Path lane, Team team)
    {
        min_go = _minion as GameObject;
        min_go.GetComponent<MinionMovement>().ChangeLane(lane);
        nma = min_go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nma.enabled = true;
        nma.destination = bluePortal.gameObject.transform.position;
    }
}