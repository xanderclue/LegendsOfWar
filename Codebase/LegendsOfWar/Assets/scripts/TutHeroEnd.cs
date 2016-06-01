using UnityEngine;
public class TutHeroEnd : MonoBehaviour
{
	[SerializeField]
	private GameObject HeroOnly = null;
	private void OnTriggerEnter()
	{
		HeroOnly.SetActive( false );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE