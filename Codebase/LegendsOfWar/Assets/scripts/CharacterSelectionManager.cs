using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
	public GameObject[ ] Legends;
	public int Index;

	static CharacterSelectionManager inst;
	public static CharacterSelectionManager Instance { get { return inst; } }

	bool[ ] available;
	public bool[ ] Available { get { return available; } }

	public delegate void ChangedCharacterEvent();
	public static event ChangedCharacterEvent OnChangedCharacter;
	public static void ChangedCharacter()
	{
		if ( OnChangedCharacter != null )
			OnChangedCharacter();
	}

	void Awake()
	{
		if ( inst && inst.gameObject.activeInHierarchy )
			Destroy( inst.gameObject );
		inst = this;
		DontDestroyOnLoad( transform.gameObject );
	}

	void Start()
	{
		available = new bool[ Legends.Length ];
		for ( int i = 0; i < Legends.Length; ++i )
			available[ i ] = Legends[ i ];
	}

	static public GameObject LegendChoice
	{ get { if ( inst ) return inst.Legends[ inst.Index ]; return null; } }
	static public HeroInfo heroInfo { get { return LegendChoice.GetComponent<HeroInfo>(); } }
}