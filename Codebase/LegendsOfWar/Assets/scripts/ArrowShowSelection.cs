using UnityEngine;

public class ArrowShowSelection : MonoBehaviour
{

	const float zdepth = 10.0f;
	Vector3 newVector;
	[SerializeField]
	GameObject top = null, center = null, bot = null;
	void Start()
	{

		top.SetActive( false );
		center.SetActive( false );
		bot.SetActive( false );

	}

	Vector3 test = Vector3.zero;
	void Update()
	{


		test.Set( Input.mousePosition.x, Input.mousePosition.y, zdepth );
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
