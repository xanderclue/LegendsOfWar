using UnityEngine;
public class CharacterSelectionManager : MonoBehaviour
{
	public GameObject[ ] Legends;
	public int Index;
	private static CharacterSelectionManager inst;
	public static CharacterSelectionManager Instance { get { return inst; } }
	private bool[ ] available;
	public bool[ ] Available { get { return available; } }
	public delegate void ChangedCharacterEvent();
	public static event ChangedCharacterEvent OnChangedCharacter;
	public static void ChangedCharacter()
	{
		if ( OnChangedCharacter != null )
			OnChangedCharacter();
	}
	private void Awake()
	{
		if ( inst && inst.gameObject.activeInHierarchy )
			Destroy( inst.gameObject );
		inst = this;
		DontDestroyOnLoad( transform.gameObject );
	}
	private void Start()
	{
		available = new bool[ Legends.Length ];
		for ( int i = 0; i < Legends.Length; ++i )
			available[ i ] = Legends[ i ];
	}
	static public GameObject LegendChoice
	{ get { return inst ? inst.Legends[ inst.Index ] : null; } }
	static public HeroInfo heroInfo { get { return LegendChoice.GetComponent<HeroInfo>(); } }
}