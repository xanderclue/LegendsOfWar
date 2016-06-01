using UnityEngine;
using System.Collections;
public enum STATES { STATE_INTRO, STATE_HERO, STATE_MINION, STATE_MAIN, STATE_END, STATE_TOTAL }
public class IntroManager : MonoBehaviour
{
	[SerializeField]
	private GameObject IntroSequence = null, MainGame = null, GameHUD = null, RedTankMinion = null,
		RedStrikerMinion = null, RedCasterMinion = null, BlueTankMinion = null, BlueStrikerMinion =
		null, BlueCasterMinion = null, HeroTutorial = null, HeroHUD = null;
	[SerializeField]
	private GameObject[ ] RedSpawns = null, HeroInstructions = null;
	[SerializeField]
	private GameObject MechanicsList = null;
	[SerializeField]
	private GameObject[ ] Mechanics = null;
	[SerializeField]
	private GameObject RedSpawn = null, End = null, Ending = null;
	[SerializeField]
	private GameObject[ ] MinionRedSpawns = null, MinionBlueSpawns = null;
	[SerializeField]
	private GameObject MoveMainCam = null, RedTower = null, MinionEnd = null, Death = null, Player =
		null;
	[SerializeField]
	private bool pause = false;
	private STATES currentState = STATES.STATE_INTRO;
	private bool PlayedIntro = false, HeroInstanciate = false, SpawnMinionTutRed = false,
		SpawnMinionTutBlue = false, firstswitch = false, Welcome, Camera, Movement;
	public void NextState()
	{
		++currentState;
	}
	public void ToggleSpawnMinionRed()
	{
		SpawnMinionTutRed = !SpawnMinionTutRed;
	}
	public void ToggleSpawnMinionBlue()
	{
		SpawnMinionTutBlue = !SpawnMinionTutBlue;
	}
	public void ToggleWelcome()
	{
		Welcome = !Welcome;
	}
	public void ToggleCamera()
	{
		Camera = !Camera;
	}
	public void ToggleMovement()
	{
		Movement = !Movement;
	}
	public void SpawnRedTankMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( RedTankMinion, RedSpawns[ Random.Range( 0, 5 ) ].transform.position, face );
	}
	public void SpawnRedCasterMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( RedCasterMinion, RedSpawns[ Random.Range( 0, 5 ) ].transform.position, face );
	}
	public void SpawnRedStrikerMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( RedStrikerMinion, RedSpawns[ Random.Range( 0, 5 ) ].transform.position, face );
	}
	public void SpawnBlueTankMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( BlueTankMinion, MinionBlueSpawns[ Random.Range( 0, 5 ) ].transform.position,
			face );
	}
	public void SpawnBlueCasterMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( BlueCasterMinion, MinionBlueSpawns[ Random.Range( 0, 5 ) ].transform.position,
			face );
	}
	public void SpawnBlueStrikerMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( BlueStrikerMinion, MinionBlueSpawns[ Random.Range( 0, 5 ) ].transform.position,
			face );
	}
	public void TogglePause()
	{
		pause = !pause;
		Time.timeScale = 1.0f - Time.timeScale;
	}
	private void Start()
	{
		Welcome = Camera = Movement = pause = HeroInstanciate = PlayedIntro = false;
		StartCoroutine( LateStart() );
	}
	private void Update()
	{
		Player = GameManager.Instance.Player;
		switch ( currentState )
		{
			case STATES.STATE_HERO:
				if ( !HeroInstanciate )
				{
					HeroTutorial.SetActive( true );
					HeroHUD.SetActive( true );
					HeroCamScript.inst.SwitchView();
					HeroInstanciate = true;
					HeroInstructions[ 0 ].SetActive( true );
				}
				if ( Welcome )
				{
					HeroInstructions[ 0 ].SetActive( false );
					HeroInstructions[ 1 ].SetActive( true );
					MechanicsList.SetActive( true );
					Welcome = false;
				}
				else if ( Camera )
				{
					HeroInstructions[ 1 ].SetActive( false );
					HeroInstructions[ 2 ].SetActive( true );
					Mechanics[ 1 ].SetActive( true );
					Camera = false;
				}
				else if ( Movement )
				{
					HeroInstructions[ 2 ].SetActive( false );
					Movement = false;
				}
				if ( RedSpawn.GetComponent<TutSpawnRed>().Battle )
				{
					if ( GameObject.FindGameObjectsWithTag( "Minion" ).Length <= 1 )
					{
						RedSpawn.GetComponent<TutSpawnRed>().Battle = false;
						End.SetActive( true );
						Ending.SetActive( true );
					}
				}
				break;
			case STATES.STATE_MINION:
				if ( SpawnMinionTutRed )
				{
					RedSpawns = MinionRedSpawns;
					SpawnRedStrikerMinion();
					SpawnRedStrikerMinion();
					SpawnRedTankMinion();
					SpawnRedTankMinion();
					SpawnRedTankMinion();
					SpawnRedStrikerMinion();
					SpawnRedStrikerMinion();
					SpawnRedStrikerMinion();
					SpawnMinionTutRed = false;
				}
				if ( SpawnMinionTutBlue )
				{
					SpawnBlueStrikerMinion();
					SpawnBlueStrikerMinion();
					SpawnBlueStrikerMinion();
					SpawnBlueTankMinion();
					SpawnBlueTankMinion();
					SpawnBlueCasterMinion();
					SpawnMinionTutBlue = false;
				}
				if ( Input.GetKeyDown( KeyCode.C ) && !firstswitch )
				{
					MoveMainCam.SetActive( true );
					firstswitch = true;
				}
				if ( RedTower && !RedTower.activeInHierarchy )
				{
					TogglePause();
					MinionEnd.SetActive( true );
				}
				break;
			default:
				break;
		}
		if ( !PlayedIntro )
			if ( Input.GetKeyDown( KeyCode.Return ) )
			{
				PlayedIntro = true;
				IntroSequence.SetActive( false );
				currentState = STATES.STATE_HERO;
			}
		if ( Input.GetKeyDown( KeyCode.Backspace ) )
		{
			SpawnRedTankMinion();
			SpawnRedCasterMinion();
			SpawnRedStrikerMinion();
		}
		if ( Player && !Player.activeInHierarchy )
			Death.SetActive( true );
	}
	private IEnumerator LateStart()
	{
		yield return new WaitForSeconds( 0.001f );
		MainGame.SetActive( false );
		GameHUD.SetActive( false );
		HeroTutorial.SetActive( false );
		HeroHUD.SetActive( false );
		for ( int i = 0; i < HeroInstructions.Length - 1; ++i )
			HeroInstructions[ i ].SetActive( false );
		MechanicsList.SetActive( false );
		for ( int i = 0; i < Mechanics.Length - 1; ++i )
			Mechanics[ i ].SetActive( false );
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
    static IntroManager inst = null;
    public static IntroManager instance { get { return inst; } }

    bool PlayedIntro = false;
    bool HeroInstanciate = false;
    enum STATES
    {
        STATE_INTRO,
        STATE_HERO,
        STATE_MINION,
        STATE_MAIN,
        STATE_END,

        STATE_TOTAL
    }
    STATES currentState = STATES.STATE_INTRO;
    public void NextState() { currentState += 1; }
    //Full Controlers
    [SerializeField]
    GameObject IntroSequence;
    [SerializeField]
    GameObject MainGame;
    [SerializeField]
    GameObject GameHUD;

    //Minion Prefabs
    [SerializeField]
    GameObject RedTankMinion;
    [SerializeField]
    GameObject RedStrikerMinion;
    [SerializeField]
    GameObject RedCasterMinion;
    [SerializeField]
    GameObject BlueTankMinion;
    [SerializeField]
    GameObject BlueStrikerMinion;
    [SerializeField]
    GameObject BlueCasterMinion;

#region Hero Tutorial Requirements
    [SerializeField]
    GameObject HeroTutorial;
    [SerializeField]
    GameObject HeroHUD;

    [SerializeField]
    GameObject [] RedSpawns;

    //[SerializeField]
    //GameObject Instructions;
    [SerializeField]
    GameObject[] HeroInstructions;

    [SerializeField]
    GameObject MechanicsList;
    [SerializeField]
    GameObject[] Mechanics;

    [SerializeField]
    GameObject RedSpawn;

    [SerializeField]
    GameObject End;

    [SerializeField]
    GameObject Ending;
#endregion

#region Minion tut Requirements

    bool SpawnMinionTutRed = false;
    public void ToggleSpawnMinionRed() { SpawnMinionTutRed = !SpawnMinionTutRed; }
    bool SpawnMinionTutBlue = false;
    public void ToggleSpawnMinionBlue() { SpawnMinionTutBlue = !SpawnMinionTutBlue; }
    bool firstswitch = false;

    [SerializeField]
    GameObject[] MinionRedSpawns;
    [SerializeField]
    GameObject[] MinionBlueSpawns;
    [SerializeField]
    GameObject MoveMainCam;
    [SerializeField]
    GameObject RedTower;
    [SerializeField]
    GameObject MinionEnd;

#endregion

    [SerializeField]
    GameObject Death;
    [SerializeField]
    GameObject Player;
#region ToggleHeroTut
    bool Welcome, Camera, Movement;
    public void ToggleWelcome() { Welcome = !Welcome; }
    public void ToggleCamera() { Camera = !Camera; }
    public void ToggleMovement() { Movement = !Movement; }
#endregion

    // Use this for initialization
    void Start()
    {
        PlayedIntro = false;
        HeroInstanciate = false;
        pause = false;
        Welcome = Camera = Movement = false;

        StartCoroutine(LateStart(0.001f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("DelayStart");
        MainGame.SetActive(false);
        GameHUD.SetActive(false);
        HeroTutorial.SetActive(false);
        HeroHUD.SetActive(false);

        //Instructions.SetActive(false);
        for(int i = 0; i < HeroInstructions.Length - 1; ++i)
        {
            HeroInstructions[i].SetActive(false);
        }

        MechanicsList.SetActive(false);
        for (int i = 0; i < Mechanics.Length - 1; ++i)
        {
            Mechanics[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Player = GameManager.Instance.Player;
        switch (currentState)
        {
            case STATES.STATE_INTRO:
                break;

            case STATES.STATE_HERO:
                if (HeroInstanciate == false)
                {
                    HeroTutorial.SetActive(true);
                    HeroHUD.SetActive(true);
                    HeroCamScript.inst.SwitchView();
                    HeroInstanciate = true;
                    //Instructions.SetActive(true);
                    HeroInstructions[0].SetActive(true);
                    
                }
                if(Welcome == true)
                {
                    HeroInstructions[0].SetActive(false);
                    HeroInstructions[1].SetActive(true);
                    MechanicsList.SetActive(true);
                    Welcome = false;
                }
                else if (Camera == true)
                {
                    HeroInstructions[1].SetActive(false);
                    HeroInstructions[2].SetActive(true);
                    Mechanics[1].SetActive(true);
                    Camera = false;
                }
                else if( Movement == true)
                {
                    HeroInstructions[2].SetActive(false);
                    Movement = false;
                    //Instructions.SetActive(false);
                }
                if(RedSpawn.GetComponent<TutSpawnRed>().Battle == true)
                {
                    GameObject[] Minions = GameObject.FindGameObjectsWithTag("Minion");
                    //Debug.Log("Searching");

                    if(Minions.Length <= 1)
                    {
                        //Debug.Log("None Found");
                        RedSpawn.GetComponent<TutSpawnRed>().Battle = false;
                        End.SetActive(true);
                        //Instructions.SetActive(true);
        Ending.SetActive(true);
                    }
                }
                break;

            case STATES.STATE_MINION:
                if(SpawnMinionTutRed == true)
                {
                    RedSpawns = MinionRedSpawns;
                    SpawnRedStrikerMinion();
                    SpawnRedStrikerMinion();
                    SpawnRedTankMinion();
                    SpawnRedTankMinion();
                    SpawnRedTankMinion();
                    SpawnRedStrikerMinion();
                    SpawnRedStrikerMinion();
                    SpawnRedStrikerMinion();

                    SpawnMinionTutRed = false;
                }
                if(SpawnMinionTutBlue == true)
                {
                    SpawnBlueStrikerMinion();
                    SpawnBlueStrikerMinion();
                    SpawnBlueStrikerMinion();
                    SpawnBlueTankMinion();
                    SpawnBlueTankMinion();
                    SpawnBlueCasterMinion();
                    SpawnMinionTutBlue = false;
                }
                if (Input.GetKeyDown(KeyCode.C) && firstswitch == false)
                {
                    MoveMainCam.SetActive(true);
                    firstswitch = true;
                    //Instructions.SetActive(true);
                }
                if(RedTower != null && RedTower.activeInHierarchy == false)
                {
                    TogglePause();
                    MinionEnd.SetActive(true);
                    Debug.Log("Tower Destroyed");
                }
                break;

            default:
                break;
        }


        //Hero Instructions
        //End Intro
        if (PlayedIntro == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("KeyDown");
                PlayedIntro = true;
                IntroSequence.SetActive(false);
                currentState = STATES.STATE_HERO;

            }
        }


        //Start Hero Tutorial

        //End Hero Tutoral


        //Minion Tutorial

        //End Minion Tutorial


        //Main Game Tutorial

        //End Game Tutorial


        //Testing Code
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("test");
            SpawnRedTankMinion();
            SpawnRedCasterMinion();
            SpawnRedStrikerMinion();
        }
        if (Player != null && Player.activeInHierarchy == false)
        {
            Death.SetActive(true);
            //Instructions.SetActive(true);

        }
    }


#region Spawing

    public void SpawnRedTankMinion()
    {
       Quaternion face = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

       Instantiate(RedTankMinion, RedSpawns[Random.Range(0, 5)].transform.position, face);
    }
    public void SpawnRedCasterMinion()
    {
        Quaternion face = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        Instantiate(RedCasterMinion, RedSpawns[Random.Range(0, 5)].transform.position, face);
    }
    public void SpawnRedStrikerMinion()
    {
        Quaternion face = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        Instantiate(RedStrikerMinion, RedSpawns[Random.Range(0, 5)].transform.position, face);
    }

    public void SpawnBlueTankMinion()
    {
        Quaternion face = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        Instantiate(BlueTankMinion, MinionBlueSpawns[Random.Range(0, 5)].transform.position, face);
    }
    public void SpawnBlueCasterMinion()
    {
        Quaternion face = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        Instantiate(BlueCasterMinion, MinionBlueSpawns[Random.Range(0, 5)].transform.position, face);
    }
    public void SpawnBlueStrikerMinion()
    {
        Quaternion face = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        Instantiate(BlueStrikerMinion, MinionBlueSpawns[Random.Range(0, 5)].transform.position, face);
    }
#endregion

    public bool pause = false;
    public void TogglePause()
    {
        pause = !pause;
        Time.timeScale = 1.0f - Time.timeScale;
    }
}

#endif
#endregion //OLD_CODE