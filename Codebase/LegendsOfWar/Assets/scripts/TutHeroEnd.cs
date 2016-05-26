using UnityEngine;

public class TutHeroEnd : MonoBehaviour
{

	[SerializeField]
	GameObject HeroOnly;


	void OnTriggerEnter()
	{
		HeroOnly.SetActive( false );
	}
}
