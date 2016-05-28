using UnityEngine;
public class TutSpawnRed : MonoBehaviour
{
	[SerializeField]
	private GameObject IntroductionManager;
	[SerializeField]
	private GameObject Health;
	[SerializeField]
	private GameObject Hero;
	public bool Battle;

	private void Start()
	{
		Battle = false;
	}
	private void OnTriggerEnter()
	{
		Hero.SetActive( true );
		Health.SetActive( true );
		IntroductionManager.GetComponent<IntroManager>().SpawnRedStrikerMinion();
		IntroductionManager.GetComponent<IntroManager>().SpawnRedStrikerMinion();
		IntroductionManager.GetComponent<IntroManager>().SpawnRedStrikerMinion();
		Battle = true;
		this.gameObject.SetActive( false );
	}
}