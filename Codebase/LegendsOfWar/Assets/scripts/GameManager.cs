using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool tutorial = false;
    [SerializeField]
    private PortalInfo redPortal = null, bluePortal = null;
    [SerializeField]
    private GameObject redTankMinion = null, redCasterMinion = null, redStrikerMinion = null,
        blueTankMinion = null, blueCasterMinion = null, blueStrikerMinion = null;
    [SerializeField]
    private EventSystem uiEventSystem = null;
    [SerializeField]
    private float maxTime = 900.0f, waveTime = 60.0f;
    [SerializeField]
    private HudScript hudScript = null;
    [SerializeField]
    private GameObject cursorObject = null;
    [SerializeField]
    private Transform HeroSpawnPoint = null;
    public GameObject Player;
    [SerializeField]
    private ParticleSystem BoomRed = null, BoomBlue = null, ExitRed = null, ExitBlue = null;
    [SerializeField]
    private GameObject defaultHeroPrefab = null;
    [SerializeField]
    private Transform topSplit = null, botSplit = null;
    [SerializeField]
    private List<Button> buttons = null;
    [SerializeField]
    private ResourceBarScript heroHealthPanel = null, heroManaPanel = null;
    public delegate void GameEndEvent();
    public static event GameEndEvent OnBlueWin, OnRedWin;
    private static readonly Quaternion faceRight = new Quaternion(0.0f, 0.707106781f, 0.0f,
        0.707106781f), faceLeft = new Quaternion(0.0f, -0.707106781f, 0.0f, 0.707106781f), faceUp
        = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f), faceDown = new Quaternion(0.0f, 1.0f, 0.0f,
            0.0f);
    public static float topSplitZ, botSplitZ;
    public static List<HeroAbilities> abilities;
    private static GameManager instance = null;
    private MinionInfo[] endminions;
    private List<HeroInfo> heros = null;
    private GameObject min_go;
    private UnityEngine.AI.NavMeshAgent nma;
    private MinionInfo mInfo;
    private float waveTimer, timer;
    private int wave = 0;
    private bool gameEnded = false, gameRunning = false, shopActive = false;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<GameManager>();
                if (!instance)
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }
    public static HudScript Hud
    { get { return instance.hudScript; } }
    public static GameObject TheCursor
    { get { return instance.cursorObject; } }
    public static Transform RedPortalTransform
    { get { return Instance.redPortal.transform; } }
    public static Transform BluePortalTransform
    { get { return Instance.bluePortal.transform; } }
    public static Vector3 BlueHeroSpawnPosition
    { get { return Instance.HeroSpawnPoint.position; } }
    public static bool Avail
    { get { return instance != null; } }
    public static bool Tutorial
    {
        get
        {
            if (instance)
                return instance.tutorial;
            return false;
        }
    }
    public static bool TheEventSystem
    {
        set
        {
            Instance.uiEventSystem.enabled = value;
            foreach (Button button in Instance.buttons)
                button.enabled = value;
        }
    }
    public static bool GameRunning
    {
        get
        {
            if (instance)
                return Instance.gameRunning;
            else
                return false;
        }
    }
    public static bool GameEnded
    { get { return instance.gameEnded; } }
    public List<HeroInfo> Heros
    { get { return heros; } }
    public string WaveTimer
    { get { return waveTimer.ToString("F2"); } }
    public string Wave
    { get { return wave.ToString(); } }
    public float Timer
    { get { return timer; } }
    public void InitiateGame()
    {
        gameRunning = true;
        gameEnded = false;
        timer = maxTime;
        waveTimer = waveTime;
        EconomyManager.Instance.StartingGame();
        ResetUpgrades();
    }
    public void SpawnHero()
    {
        if (CharacterSelectionManager.Instance)
            Player = Instantiate(CharacterSelectionManager.LegendChoice, HeroSpawnPoint.position,
                faceRight) as GameObject;
        else
            Player = Instantiate(defaultHeroPrefab, HeroSpawnPoint.position, faceRight) as
                GameObject;
        heroHealthPanel.Host = heroManaPanel.Host = Player;
        if (tutorial)
        {
            HeroAbilities abilities = Player.GetComponent<HeroAbilities>();
            abilities.GetAbilityW.AbilityEnabled = false;
            abilities.GetAbilityE.AbilityEnabled = false;
            abilities.GetAbilityR.AbilityEnabled = false;
        }
    }
    public void AddHero(HeroInfo info)
    {
        heros.Add(info);
    }
    public void Pause()
    {
        ApplicationManager.Instance.ChangeAppState(StateID.STATE_PAUSED);
        gameRunning = false;
    }
    public void Unpause()
    {
        ApplicationManager.Instance.ChangeAppState(StateID.STATE_INGAME);
        gameRunning = true;
    }
    public void EnterShop()
    {
        ApplicationManager.Instance.ChangeAppState(StateID.STATE_SHOP);
    }
    public void ExitShop()
    {
        ApplicationManager.Instance.ChangeAppState(StateID.STATE_INGAME);
    }
    public void InstaRespawn(Team team, HeroInfo hero)
    {
        if (hero.team == team && !hero.Alive)
            hero.RespawnTimer = 0.0f;
    }
    public void SpawnStrikerMinion(Team team, int lane)
    {
        if (Team.BLUE_TEAM == team)
        {
            switch (lane)
            {
                case 1:
                    SetupMinion(Instantiate(blueStrikerMinion, bluePortal.LeftSpawn[Random.Range(
                        0, 5)].position, faceUp), Path.NORTH_PATH, Team.BLUE_TEAM);
                    break;
                case 2:
                    SetupMinion(Instantiate(blueStrikerMinion, bluePortal.MidSpawn[Random.Range(
                        0, 5)].position, faceRight), Path.CENTER_PATH, Team.BLUE_TEAM);
                    break;
                case 3:
                    SetupMinion(Instantiate(blueStrikerMinion, bluePortal.RightSpawn[Random.Range
                        (0, 5)].position, faceDown), Path.SOUTH_PATH, Team.BLUE_TEAM);
                    break;
                default:
                    break;
            }
        }
    }
    public void SpawnTankMinion(Team team, int lane)
    {
        if (Team.BLUE_TEAM == team)
        {
            switch (lane)
            {
                case 1:
                    SetupMinion(Instantiate(blueTankMinion, bluePortal.LeftSpawn[Random.Range(0,
                        5)].position, faceUp), Path.NORTH_PATH, Team.BLUE_TEAM);
                    break;
                case 2:
                    SetupMinion(Instantiate(blueTankMinion, bluePortal.MidSpawn[Random.Range(0,
                        5)].position, faceRight), Path.CENTER_PATH, Team.BLUE_TEAM);
                    break;
                case 3:
                    SetupMinion(Instantiate(blueTankMinion, bluePortal.RightSpawn[Random.Range(0
                        , 5)].position, faceDown), Path.SOUTH_PATH, Team.BLUE_TEAM);
                    break;
                default:
                    break;
            }
        }
    }
    public void SpawnCasterMinion(Team team, int lane)
    {
        if (Team.BLUE_TEAM == team)
        {
            switch (lane)
            {
                case 1:
                    SetupMinion(Instantiate(blueCasterMinion, bluePortal.LeftSpawn[Random.Range(
                        0, 5)].position, faceUp), Path.NORTH_PATH, Team.BLUE_TEAM);
                    break;
                case 2:
                    SetupMinion(Instantiate(blueCasterMinion, bluePortal.MidSpawn[Random.Range(0
                        , 5)].position, faceRight), Path.CENTER_PATH, Team.BLUE_TEAM);
                    break;
                case 3:
                    SetupMinion(Instantiate(blueCasterMinion, bluePortal.RightSpawn[Random.Range(
                        0, 5)].position, faceDown), Path.SOUTH_PATH, Team.BLUE_TEAM);
                    break;
                default:
                    break;
            }
        }
    }
    public void UpgradeStrikerMinions(Team team)
    {
        if (Team.BLUE_TEAM == team)
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
    public void UpgradeTankMinions(Team team)
    {
        if (Team.BLUE_TEAM == team)
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
    public void UpgradeCasterMinions(Team team)
    {
        if (Team.BLUE_TEAM == team)
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
    public void EndGame()
    {
        gameEnded = true;
        gameRunning = false;
        ResetUpgrades();
        if (bluePortal.HP < redPortal.HP)
            ApplicationManager.Instance.ChangeAppState(StateID.STATE_GAME_LOST);
        else if (bluePortal.HP > redPortal.HP)
            ApplicationManager.Instance.ChangeAppState(StateID.STATE_GAME_WON);
        else
            ApplicationManager.Instance.ChangeAppState(StateID.STATE_GAME_DRAW);
    }
    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            abilities = new List<HeroAbilities>();
            heros = new List<HeroInfo>();
            topSplitZ = topSplit.position.z;
            botSplitZ = botSplit.position.z;
            SpawnHero();
        }
    }
    private void Start()
    {
        InitiateGame();
        BoomBlue.gameObject.SetActive(false);
        BoomRed.gameObject.SetActive(false);
        ExitBlue.gameObject.SetActive(false);
        ExitRed.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!gameRunning || gameEnded)
            return;
        if (Input.GetKeyDown(KeyCode.Escape) && StateID.STATE_SHOP != ApplicationManager.Instance
            .GetAppState())
            Pause();
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!shopActive)
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
        else if (Input.GetKeyDown(KeyCode.Escape) && StateID.STATE_SHOP == ApplicationManager.
            Instance.GetAppState())
            ExitShop();
        else if (timer <= 0.0f || redPortal.HP <= 0.0f || bluePortal.HP <= 0.0f)
            StartCoroutine(GameEnding());
        else
        {
            timer -= Time.deltaTime;
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0.0f)
            {
                waveTimer = waveTime;
                ++wave;
                SpawnMinionWaves();
            }
            DoAbilityCooldowns();
            RespawnTimers();
        }
    }
    private void OnDestroy()
    {
        if (this == instance)
            instance = null;
    }
    private void SpawnMinionWaves()
    {
        SetupMinion(Instantiate(redCasterMinion, redPortal.LeftSpawn[0].position, faceDown),
            Path.SOUTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redStrikerMinion, redPortal.LeftSpawn[3].position, faceDown),
            Path.SOUTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redStrikerMinion, redPortal.LeftSpawn[4].position, faceDown),
            Path.SOUTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redTankMinion, redPortal.LeftSpawn[1].position, faceDown), Path
            .SOUTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redTankMinion, redPortal.LeftSpawn[2].position, faceDown), Path
            .SOUTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redCasterMinion, redPortal.MidSpawn[0].position, faceLeft),
            Path.CENTER_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redStrikerMinion, redPortal.MidSpawn[3].position, faceLeft),
            Path.CENTER_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redStrikerMinion, redPortal.MidSpawn[4].position, faceLeft),
            Path.CENTER_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redTankMinion, redPortal.MidSpawn[1].position, faceLeft), Path.
            CENTER_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redTankMinion, redPortal.MidSpawn[2].position, faceLeft), Path.
            CENTER_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redCasterMinion, redPortal.RightSpawn[0].position, faceUp),
            Path.NORTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redStrikerMinion, redPortal.RightSpawn[3].position, faceUp),
            Path.NORTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redStrikerMinion, redPortal.RightSpawn[4].position, faceUp),
            Path.NORTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redTankMinion, redPortal.RightSpawn[1].position, faceUp), Path.
            NORTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(redTankMinion, redPortal.RightSpawn[2].position, faceUp), Path.
            NORTH_PATH, Team.RED_TEAM);
        SetupMinion(Instantiate(blueCasterMinion, bluePortal.LeftSpawn[0].position, faceUp),
            Path.NORTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueStrikerMinion, bluePortal.LeftSpawn[3].position, faceUp),
            Path.NORTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueStrikerMinion, bluePortal.LeftSpawn[4].position, faceUp),
            Path.NORTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueTankMinion, bluePortal.LeftSpawn[1].position, faceUp), Path
            .NORTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueTankMinion, bluePortal.LeftSpawn[2].position, faceUp), Path
            .NORTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueCasterMinion, bluePortal.MidSpawn[0].position, faceRight),
            Path.CENTER_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueStrikerMinion, bluePortal.MidSpawn[3].position, faceRight),
            Path.CENTER_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueStrikerMinion, bluePortal.MidSpawn[4].position, faceRight),
            Path.CENTER_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueTankMinion, bluePortal.MidSpawn[1].position, faceRight),
            Path.CENTER_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueTankMinion, bluePortal.MidSpawn[2].position, faceRight),
            Path.CENTER_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueCasterMinion, bluePortal.RightSpawn[0].position, faceDown),
            Path.SOUTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueStrikerMinion, bluePortal.RightSpawn[3].position, faceDown)
            , Path.SOUTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueStrikerMinion, bluePortal.RightSpawn[4].position, faceDown)
            , Path.SOUTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueTankMinion, bluePortal.RightSpawn[1].position, faceDown),
            Path.SOUTH_PATH, Team.BLUE_TEAM);
        SetupMinion(Instantiate(blueTankMinion, bluePortal.RightSpawn[2].position, faceDown),
            Path.SOUTH_PATH, Team.BLUE_TEAM);
        EconomyManager.Instance.NewWave();
        AudioManager.PlaySoundEffect(AudioManager.sfxWaveSpawn);
    }
    private void SetupMinion(Object _minion, Path lane, Team team)
    {
        min_go = _minion as GameObject;
        min_go.GetComponent<MinionMovement>().ChangeLane(lane);
        nma = min_go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nma.enabled = true;
        if (Team.BLUE_TEAM == team)
            nma.destination = RedPortalTransform.position;
        else
            nma.destination = BluePortalTransform.position;
    }
    private void DoAbilityCooldowns()
    {
        foreach (HeroAbilities abs in abilities)
        {
            abs.GetAbilityQ.Timer -= Time.deltaTime;
            abs.GetAbilityW.Timer -= Time.deltaTime;
            abs.GetAbilityE.Timer -= Time.deltaTime;
            abs.GetAbilityR.Timer -= Time.deltaTime;
        }
    }
    private void RespawnTimers()
    {
        foreach (HeroInfo hero in heros)
        {
            hero.RespawnTimer -= Time.deltaTime;
            if (hero.WaitingRespawn)
                hero.Respawn();
        }
    }
    private void ResetUpgrades()
    {
        mInfo = blueCasterMinion.GetComponent<MinionInfo>();
        mInfo.MAXHP = 290.0f;
        mInfo.AttackSpeed = 0.7f;
        mInfo.Range = 50.0f;
        mInfo.Damage = 30.0f;
        mInfo = blueTankMinion.GetComponent<MinionInfo>();
        mInfo.MAXHP = 515.0f;
        mInfo.AttackSpeed = 0.8f;
        mInfo.Damage = 60.0f;
        mInfo = blueStrikerMinion.GetComponent<MinionInfo>();
        mInfo.MAXHP = 380.0f;
        mInfo.AttackSpeed = 0.8f;
        mInfo.Damage = 50.0f;
    }
    private IEnumerator GameEnding()
    {
        gameEnded = true;
        if (StateID.STATE_SHOP == ApplicationManager.Instance.GetAppState())
            ExitShop();
        if (redPortal.HP <= 0.0f)
        {
            OnBlueWin?.Invoke();
            if (!BoomRed.isPlaying)
                BoomRed.gameObject.SetActive(true);
        }
        else if (bluePortal.HP <= 0.0f)
        {
            OnRedWin?.Invoke();
            if (!BoomBlue.isPlaying)
                BoomBlue.gameObject.SetActive(true);
        }
        endminions = FindObjectsOfType<MinionInfo>();
        foreach (MinionInfo endminion in endminions)
            Destroy(endminion.gameObject);
        yield return new WaitForSeconds(4.0f);
        redPortal.gameObject.SetActive(false);
        bluePortal.gameObject.SetActive(false);
        if (redPortal.HP <= 0.0f)
            ExitRed.gameObject.SetActive(true);
        else if (bluePortal.HP <= 0.0f)
            ExitBlue.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        EndGame();
    }
}