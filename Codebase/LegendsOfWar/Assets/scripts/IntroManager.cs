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
#endif
#endregion //OLD_CODE