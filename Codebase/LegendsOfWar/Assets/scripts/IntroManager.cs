using UnityEngine;
using System.Collections;
public class IntroManager : MonoBehaviour
{
	static IntroManager inst = null;
	public static IntroManager instance { get { return inst; } }
	bool PlayedIntro = false;
	bool HeroInstanciate = false;
	enum STATES { STATE_INTRO, STATE_HERO, STATE_MINION, STATE_MAIN, STATE_END, STATE_TOTAL }
	STATES currentState = STATES.STATE_INTRO;
	public void NextState()
	{
		currentState += 1;
	}
	[SerializeField]
	GameObject IntroSequence = null;
	[SerializeField]
	GameObject MainGame = null;
	[SerializeField]
	GameObject GameHUD = null;
	[SerializeField]
	GameObject RedTankMinion = null;
	[SerializeField]
	GameObject RedStrikerMinion = null;
	[SerializeField]
	GameObject RedCasterMinion = null;
	[SerializeField]
	GameObject BlueTankMinion = null;
	[SerializeField]
	GameObject BlueStrikerMinion = null;
	[SerializeField]
	GameObject BlueCasterMinion = null;
	[SerializeField]
	GameObject HeroTutorial = null;
	[SerializeField]
	GameObject HeroHUD = null;
	[SerializeField]
	GameObject[ ] RedSpawns;
	[SerializeField]
	GameObject[ ] HeroInstructions = null;
	[SerializeField]
	GameObject MechanicsList = null;
	[SerializeField]
	GameObject[ ] Mechanics = null;
	[SerializeField]
	GameObject RedSpawn = null;
	[SerializeField]
	GameObject End = null;
	[SerializeField]
	GameObject Ending = null;
	bool SpawnMinionTutRed = false;
	public void ToggleSpawnMinionRed()
	{
		SpawnMinionTutRed = !SpawnMinionTutRed;
	}
	bool SpawnMinionTutBlue = false;
	public void ToggleSpawnMinionBlue()
	{
		SpawnMinionTutBlue = !SpawnMinionTutBlue;
	}
	bool firstswitch = false;
	[SerializeField]
	GameObject[ ] MinionRedSpawns = null;
	[SerializeField]
	GameObject[ ] MinionBlueSpawns = null;
	[SerializeField]
	GameObject MoveMainCam = null;
	[SerializeField]
	GameObject RedTower = null;
	[SerializeField]
	GameObject MinionEnd = null;
	[SerializeField]
	GameObject Death = null;
	[SerializeField]
	GameObject Player = null;
	bool Welcome, Camera, Movement;
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
	void Start()
	{
		PlayedIntro = false;
		HeroInstanciate = false;
		pause = false;
		Welcome = Camera = Movement = false;
		StartCoroutine( LateStart( 0.001f ) );
	}
	IEnumerator LateStart( float waitTime )
	{
		yield return new WaitForSeconds( waitTime );
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
	void Update()
	{
		Player = GameManager.Instance.Player;
		switch ( currentState )
		{
			case STATES.STATE_INTRO:
				break;
			case STATES.STATE_HERO:
				if ( HeroInstanciate == false )
				{
					HeroTutorial.SetActive( true );
					HeroHUD.SetActive( true );
					HeroCamScript.inst.SwitchView();
					HeroInstanciate = true;
					HeroInstructions[ 0 ].SetActive( true );
				}
				if ( Welcome == true )
				{
					HeroInstructions[ 0 ].SetActive( false );
					HeroInstructions[ 1 ].SetActive( true );
					MechanicsList.SetActive( true );
					Welcome = false;
				}
				else if ( Camera == true )
				{
					HeroInstructions[ 1 ].SetActive( false );
					HeroInstructions[ 2 ].SetActive( true );
					Mechanics[ 1 ].SetActive( true );
					Camera = false;
				}
				else if ( Movement == true )
				{
					HeroInstructions[ 2 ].SetActive( false );
					Movement = false;
				}
				if ( RedSpawn.GetComponent<TutSpawnRed>().Battle == true )
				{
					GameObject[ ] Minions = GameObject.FindGameObjectsWithTag( "Minion" );
					if ( Minions.Length <= 1 )
					{
						RedSpawn.GetComponent<TutSpawnRed>().Battle = false;
						End.SetActive( true );
						Ending.SetActive( true );
					}
				}
				break;
			case STATES.STATE_MINION:
				if ( SpawnMinionTutRed == true )
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
				if ( SpawnMinionTutBlue == true )
				{
					SpawnBlueStrikerMinion();
					SpawnBlueStrikerMinion();
					SpawnBlueStrikerMinion();
					SpawnBlueTankMinion();
					SpawnBlueTankMinion();
					SpawnBlueCasterMinion();
					SpawnMinionTutBlue = false;
				}
				if ( Input.GetKeyDown( KeyCode.C ) && firstswitch == false )
				{
					MoveMainCam.SetActive( true );
					firstswitch = true;
				}
				if ( RedTower != null && RedTower.activeInHierarchy == false )
				{
					TogglePause();
					MinionEnd.SetActive( true );
				}
				break;
			default:
				break;
		}
		if ( PlayedIntro == false )
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
		if ( Player != null && Player.activeInHierarchy == false )
			Death.SetActive( true );
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
		Instantiate( BlueTankMinion, MinionBlueSpawns[ Random.Range( 0, 5 ) ].transform.position, face );
	}
	public void SpawnBlueCasterMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( BlueCasterMinion, MinionBlueSpawns[ Random.Range( 0, 5 ) ].transform.position, face );
	}
	public void SpawnBlueStrikerMinion()
	{
		Quaternion face = new Quaternion( 0.0f, 1.0f, 0.0f, 0.0f );
		Instantiate( BlueStrikerMinion, MinionBlueSpawns[ Random.Range( 0, 5 ) ].transform.position, face );
	}
	public bool pause = false;
	public void TogglePause()
	{
		pause = !pause;
		Time.timeScale = 1.0f - Time.timeScale;
	}
}