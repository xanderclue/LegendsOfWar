using UnityEngine;

public class TutSpawnRed : MonoBehaviour
{

	[SerializeField]
	GameObject IntroductionManager;

	[SerializeField]
	GameObject Health;
	[SerializeField]
	GameObject Hero;

	public bool Battle;
	void Start()
	{
		Battle = false;
	}


	void OnTriggerEnter()
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
