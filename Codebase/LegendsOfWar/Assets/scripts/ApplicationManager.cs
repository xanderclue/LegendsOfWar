using UnityEngine.SceneManagement;
using UnityEngine;
public enum StateID
{
	STATE_MAIN_MENU, STATE_OPTIONS_MENU, STATE_INGAME, STATE_PAUSED, STATE_INTRODUCTION,
	STATE_GAME_WON, STATE_GAME_LOST, STATE_CREDITS, STATE_EXIT, STATE_HELP, STATE_GAME_DRAW,
	STATE_SHOP, STATE_SELECTION
}
public class ApplicationManager : MonoBehaviour
{
	public bool transitioning = false;
	public StateID currentState = StateID.STATE_MAIN_MENU, prevState = StateID.STATE_MAIN_MENU;
	public void ChangeAppState( string nextState )
	{
		switch ( nextState )
		{
			case "STATE_MAIN_MENU":
				ChangeAppState( StateID.STATE_MAIN_MENU );
				break;
			case "STATE_OPTIONS_MENU":
				ChangeAppState( StateID.STATE_OPTIONS_MENU );
				break;
			case "STATE_INGAME":
				ChangeAppState( StateID.STATE_INGAME );
				break;
			case "STATE_PAUSED":
				ChangeAppState( StateID.STATE_PAUSED );
				break;
			case "STATE_GAME_WON":
				ChangeAppState( StateID.STATE_GAME_WON );
				break;
			case "STATE_GAME_LOST":
				ChangeAppState( StateID.STATE_GAME_LOST );
				break;
			case "STATE_CREDITS":
				ChangeAppState( StateID.STATE_CREDITS );
				break;
			case "STATE_EXIT":
				ChangeAppState( StateID.STATE_EXIT );
				break;
			case "STATE_HELP":
				ChangeAppState( StateID.STATE_HELP );
				break;
			case "STATE_GAME_DRAW":
				ChangeAppState( StateID.STATE_GAME_DRAW );
				break;
			case "STATE_SHOP":
				ChangeAppState( StateID.STATE_SHOP );
				break;
			case "STATE_SELECTION":
				ChangeAppState( StateID.STATE_SELECTION );
				break;
			case "STATE_INTRODUCTION":
				ChangeAppState( StateID.STATE_INTRODUCTION );
				break;
			default:
				break;
		}
	}
	public void ChangeAppState( StateID nextState )
	{
		if ( nextState != currentState )
		{
			prevState = currentState;
			currentState = nextState;
			transitioning = true;
		}
	}
	public StateID GetAppState()
	{
		return currentState;
	}
	private static ApplicationManager instance = null;
	public static ApplicationManager Instance
	{
		get
		{
			if ( !instance )
			{
				instance = FindObjectOfType<ApplicationManager>();
				if ( !instance )
					instance = new GameObject( "ApplicationManager" ).AddComponent<
						ApplicationManager>();
			}
			return instance;
		}
	}
	private void Awake()
	{
		if ( instance )
			Destroy( this );
		else
		{
			instance = this;
			DontDestroyOnLoad( gameObject );
			if ( SceneManager.GetActiveScene().name == "WorldMap" )
				currentState = StateID.STATE_INGAME;
		}
	}
	private void OnDestroy()
	{
		if ( this == instance )
			instance = null;
	}
	public static void ReturnToPreviousState()
	{
		Instance.ChangeAppState( Instance.prevState );
	}
	private void Update()
	{
		if ( transitioning )
		{
			Time.timeScale = 1.0f;
			switch ( currentState )
			{
				case StateID.STATE_MAIN_MENU:
					SceneManager.LoadScene( "MainMenu" );
					break;
				case StateID.STATE_INTRODUCTION:
					SceneManager.LoadScene( "Introduction Scene" );
					break;
				case StateID.STATE_OPTIONS_MENU:
					if ( StateID.STATE_PAUSED == prevState )
					{
						pauseMenuEvents.EventSystem = false;
						Options.IsAdditive = true;
						SceneManager.LoadScene( "OptionsMenu", LoadSceneMode.Additive );
						Time.timeScale = 0.0f;
					}
					else
					{
						Options.IsAdditive = false;
						SceneManager.LoadScene( "OptionsMenu" );
					}
					break;
				case StateID.STATE_INGAME:
					if ( StateID.STATE_PAUSED == prevState )
					{
						SceneManager.UnloadScene( "PauseScreen" );
						GameManager.eventSystem = true;
					}
					else if ( StateID.STATE_SHOP == prevState )
					{
						SceneManager.UnloadScene( "Shop" );
						GameManager.eventSystem = true;
					}
					else
						SceneManager.LoadScene( "WorldMap" );
					break;
				case StateID.STATE_PAUSED:
					if ( StateID.STATE_OPTIONS_MENU == prevState )
					{
						SceneManager.UnloadScene( "OptionsMenu" );
						pauseMenuEvents.EventSystem = true;
					}
					else
					{
						GameManager.eventSystem = false;
						SceneManager.LoadScene( "PauseScreen", LoadSceneMode.Additive );
					}
					Time.timeScale = 0.0f;
					break;
				case StateID.STATE_GAME_WON:
					GameManager.eventSystem = false;
					SceneManager.LoadScene( "GameEndWin", LoadSceneMode.Additive );
					Time.timeScale = 0.0f;
					break;
				case StateID.STATE_GAME_LOST:
					GameManager.eventSystem = false;
					SceneManager.LoadScene( "GameEndLose", LoadSceneMode.Additive );
					Time.timeScale = 0.0f;
					break;
				case StateID.STATE_GAME_DRAW:
					GameManager.eventSystem = false;
					SceneManager.LoadScene( "GameEndDraw", LoadSceneMode.Additive );
					Time.timeScale = 0.0f;
					break;
				case StateID.STATE_CREDITS:
					SceneManager.LoadScene( "Credits" );
					break;
				case StateID.STATE_EXIT:
#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
#else
					Application.Quit();
#endif
					break;
				case StateID.STATE_HELP:
					SceneManager.LoadScene( "Instruction" );
					break;
				case StateID.STATE_SHOP:
					GameManager.eventSystem = false;
					SceneManager.LoadScene( "Shop", LoadSceneMode.Additive );
					break;
				case StateID.STATE_SELECTION:
					SceneManager.LoadScene( "Character Selection" );
					break;
				default:
					break;
			}
			transitioning = false;
		}
		if ( Input.GetKeyDown( KeyCode.F12 ) )
			Options.toggleLanguage_Static();
		if ( Input.GetKey( KeyCode.F11 ) )
			Time.timeScale = Mathf.Min( Time.timeScale * 1.01f, 100.0f );
		else if ( Input.GetKey( KeyCode.F10 ) )
			Time.timeScale = Mathf.Max( 0.0f, Time.timeScale * 0.99f );
		else if ( Input.GetKey( KeyCode.F9 ) && GameManager.GameRunning )
			Time.timeScale = 1.0f;
	}
}