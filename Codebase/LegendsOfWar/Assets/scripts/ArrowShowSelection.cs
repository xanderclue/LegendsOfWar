using UnityEngine;
public class ArrowShowSelection : MonoBehaviour
{
	private Vector3 newVector;
	[SerializeField]
	private GameObject top = null, center = null, bot = null;
	private void Start()
	{
		top.SetActive( false );
		center.SetActive( false );
		bot.SetActive( false );
	}
	private Vector3 test = Vector3.zero;
	private void Update()
	{
		test.Set( Input.mousePosition.x, Input.mousePosition.y, 10.0f );
		newVector = CameraControl.Current.ScreenToWorldPoint( test );
		if ( newVector.z < GameManager.botSplitZ )
		{
			top.SetActive( false );
			center.SetActive( false );
			bot.SetActive( true );
		}
		else if ( newVector.z > GameManager.topSplitZ )
		{
			top.SetActive( true );
			center.SetActive( false );
			bot.SetActive( false );
		}
		else
		{
			top.SetActive( false );
			center.SetActive( true );
			bot.SetActive( false );
		}
	}
}