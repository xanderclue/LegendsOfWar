using UnityEngine;
public class TutHeroEnd : MonoBehaviour
{
	[SerializeField]
	private GameObject HeroOnly;
	private void OnTriggerEnter()
	{
		HeroOnly.SetActive( false );
	}
}