using UnityEngine;
public class TutSpawnRed : MonoBehaviour
{
	[SerializeField]
	private GameObject IntroductionManager = null, Health = null, Hero = null;
	public bool Battle;
	private IntroManager introManager;
	private void Start()
	{
		Battle = false;
		introManager = IntroductionManager.GetComponent<IntroManager>();
	}
	private void OnTriggerEnter()
	{
		Hero.SetActive( true );
		Health.SetActive( true );
		introManager.SpawnRedStrikerMinion();
		introManager.SpawnRedStrikerMinion();
		introManager.SpawnRedStrikerMinion();
		Battle = true;
		this.gameObject.SetActive( false );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE