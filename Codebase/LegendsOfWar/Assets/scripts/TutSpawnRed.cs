using UnityEngine;
public class TutSpawnRed : MonoBehaviour
{
	[SerializeField]
	private GameObject IntroductionManager = null;
	[SerializeField]
	private GameObject Health = null;
	[SerializeField]
	private GameObject Hero = null;
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