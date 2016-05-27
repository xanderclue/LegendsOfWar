using UnityEngine;
public class TutMinionStart : MonoBehaviour
{
	[SerializeField]
	GameObject MininionStart = null;
	void OnTriggerEnter()
	{
		MininionStart.SetActive( true );
		this.gameObject.SetActive( false );
	}
}