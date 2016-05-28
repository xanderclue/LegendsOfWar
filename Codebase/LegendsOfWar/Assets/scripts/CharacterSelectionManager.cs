using UnityEngine;
public class CharacterSelectionManager : MonoBehaviour
{
	public GameObject[ ] Legends;
	public int Index;
	public delegate void ChangedCharacterEvent();
	public static event ChangedCharacterEvent OnChangedCharacter;
	private static CharacterSelectionManager inst;
	private bool[ ] available;
	public static CharacterSelectionManager Instance
	{ get { return inst; } }
	public static GameObject LegendChoice
	{ get { return inst ? inst.Legends[ inst.Index ] : null; } }
	public static HeroInfo heroInfo
	{ get { return LegendChoice.GetComponent<HeroInfo>(); } }
	public bool[ ] Available
	{ get { return available; } }
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
}