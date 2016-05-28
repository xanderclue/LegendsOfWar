using UnityEngine;
public enum TurnState { Still, Left, Right, Fix }
public enum Character
{ Default, Support, Hunter, Tanker, Character_5, Character_6, Character_7, Assassin, Total }
public class TurnManager : MonoBehaviour
{
	[SerializeField]
	private menuEvents menuEventsObj = null;
	[SerializeField]
	private Transform CharacterSelectionSpace = null;
	[SerializeField]
	private float rotationSpeed = 5.0f;
	[SerializeField]
	private Light[ ] spotlights = null;
	private const int m = ( int )Character.Total - 1;
	private static TurnManager inst;
	private static Quaternion[ ] rotations = new Quaternion[ ] { Quaternion.Euler( 0.0f, 0.0f, 0.0f
		), Quaternion.Euler( 0.0f, 45.0f, 0.0f ), Quaternion.Euler( 0.0f, 90.0f, 0.0f ), Quaternion.
		Euler( 0.0f, 135.0f, 0.0f ), Quaternion.Euler( 0.0f, 180.0f, 0.0f ), Quaternion.Euler( 0.0f,
			225.0f, 0.0f ), Quaternion.Euler( 0.0f, 270.0f, 0.0f ), Quaternion.Euler( 0.0f, 315.0f,
				0.0f ) };
	private TurnState turnState = TurnState.Fix;
	private Character current = 0;
	private int c;

	public static TurnManager Instance { get { return inst; } }
	private Character next
	{
		get
		{
			c = CurrentInt;
			++c;
			if ( m < c )
				c = 0;
			return ( Character )c;
		}
	}
	private Character prev
	{
		get
		{
			c = CurrentInt;
			--c;
			if ( c < 0 )
				c = m;
			return ( Character )c;
		}
	}
	public int CurrentInt { get { return ( int )current; } }
	public void TurnRight()
	{
		spLight = false;
		current = next;
		AudioManager.KillSingle();
		if ( CharacterSelectionManager.Instance.Available[ CurrentInt ] )
		{
			CharacterSelectionManager.Instance.Index = CurrentInt;
			PlayVoice();
		}
		turnState = TurnState.Right;
		spLight = true;
		CharacterSelectionManager.ChangedCharacter();
	}
	public void TurnLeft()
	{
		spLight = false;
		current = prev;
		AudioManager.KillSingle();
		if ( CharacterSelectionManager.Instance.Available[ CurrentInt ] )
		{
			CharacterSelectionManager.Instance.Index = CurrentInt;
			PlayVoice();
		}
		turnState = TurnState.Left;
		spLight = true;
		CharacterSelectionManager.ChangedCharacter();
	}
	private void PlayVoice()
	{
		sub.SetSub( "", -0.0f );
		CharacterSelectionManager.LegendChoice.GetComponent<HeroAudio>().PlayClip( "HeroSelected" );
	}
	private bool spLight { set { spotlights[ ( int )current ].enabled = value; } }
	private void Awake()
	{
		inst = this;
	}
	private void Start()
	{
		CharacterSelectionManager.Instance.Index = CurrentInt;
		spLight = true;
	}
	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.LeftArrow ) )
			TurnLeft();
		else if ( Input.GetKeyDown( KeyCode.RightArrow ) )
			TurnRight();
		switch ( turnState )
		{
			case TurnState.Still:
				break;
			case TurnState.Left:
				CharacterSelectionSpace.Rotate( 0.0f, -rotationSpeed * Time.deltaTime, 0.0f );
				if ( check( CharacterSelectionSpace.rotation ) )
					turnState = TurnState.Fix;
				break;
			case TurnState.Right:
				CharacterSelectionSpace.Rotate( 0.0f, rotationSpeed * Time.deltaTime, 0.0f );
				if ( check( CharacterSelectionSpace.rotation ) )
					turnState = TurnState.Fix;
				break;
			case TurnState.Fix:
				CharacterSelectionSpace.rotation = rotations[ CurrentInt ];
				turnState = TurnState.Still;
				break;
			default:
				break;
		}
		if ( Input.GetKeyDown( KeyCode.Escape ) )
			ApplicationManager.ReturnToPreviousState();
		else if ( Input.GetKeyDown( KeyCode.Return ) )
			if ( CharacterSelectionManager.Instance.Available[ CurrentInt ] )
				menuEventsObj.ChangeAppState( "STATE_HELP" );
	}
	private void OnDestroy()
	{
		inst = null;
	}
	private bool check( Quaternion rot )
	{
		return Quaternion.Angle( rot, rotations[ CurrentInt ] ) < 3.0f;
	}
}