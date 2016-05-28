using UnityEngine;
using System.Collections;
public enum STATES { STATE_INTRO, STATE_HERO, STATE_MINION, STATE_MAIN, STATE_END, STATE_TOTAL }
public class IntroManager : MonoBehaviour
{
	[SerializeField]
	private GameObject IntroSequence = null;
	[SerializeField]
	private GameObject MainGame = null;
	[SerializeField]
	private GameObject GameHUD = null;
	[SerializeField]
	private GameObject RedTankMinion = null;
	[SerializeField]
	private GameObject RedStrikerMinion = null;
	[SerializeField]
	private GameObject RedCasterMinion = null;
	[SerializeField]
	private GameObject BlueTankMinion = null;
	[SerializeField]
	private GameObject BlueStrikerMinion = null;
	[SerializeField]
	private GameObject BlueCasterMinion = null;
	[SerializeField]
	private GameObject HeroTutorial = null;
	[SerializeField]
	private GameObject HeroHUD = null;
	[SerializeField]
	private GameObject[ ] RedSpawns = null;
	[SerializeField]
	private GameObject[ ] HeroInstructions = null;
	[SerializeField]
	private GameObject MechanicsList = null;
	[SerializeField]
	private GameObject[ ] Mechanics = null;
	[SerializeField]
	private GameObject RedSpawn = null;
	[SerializeField]
	private GameObject End = null;
	[SerializeField]
	private GameObject Ending = null;
	[SerializeField]
	private GameObject[ ] MinionRedSpawns = null;
	[SerializeField]
	private GameObject[ ] MinionBlueSpawns = null;
	[SerializeField]
	private GameObject MoveMainCam = null;
	[SerializeField]
	private GameObject RedTower = null;
	[SerializeField]
	private GameObject MinionEnd = null;
	[SerializeField]
	private GameObject Death = null;
	[SerializeField]
	private GameObject Player = null;
	public bool pause = false;

	private bool PlayedIntro = false;
	private bool HeroInstanciate = false;
	private STATES currentState = STATES.STATE_INTRO;
	public void NextState()
	{
		currentState += 1;
	}
	private bool SpawnMinionTutRed = false;
	public void ToggleSpawnMinionRed()
	{
		SpawnMinionTutRed = !SpawnMinionTutRed;
	}
	private bool SpawnMinionTutBlue = false;
	public void ToggleSpawnMinionBlue()
	{
		SpawnMinionTutBlue = !SpawnMinionTutBlue;
	}
	private bool firstswitch = false;
	private bool Welcome, Camera, Movement;
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
	private void Start()
	{
		PlayedIntro = false;
		HeroInstanciate = false;
		pause = false;
		Welcome = Camera = Movement = false;
		StartCoroutine( LateStart( 0.001f ) );
	}
	private IEnumerator LateStart( float waitTime )
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
	private void Update()
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
}