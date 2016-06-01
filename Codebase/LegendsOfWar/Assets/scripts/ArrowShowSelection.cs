using UnityEngine;
public class ArrowShowSelection : MonoBehaviour
{
	[SerializeField]
	private GameObject top = null, center = null, bot = null;
	private Vector3 newVector, test = Vector3.zero;
	private void Start()
	{
		top.SetActive( false );
		center.SetActive( false );
		bot.SetActive( false );
	}
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
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class ArrowShowSelection : MonoBehaviour {

    //Vector3 v3;
    // Use this for initialization
    const float zdepth = 10.0f;
    Vector3 newVector;
    // Retrieve mouse position
	[SerializeField]
    GameObject top = null, center = null, bot = null;
    void Start ()
    {

        top.SetActive(false);
        center.SetActive(false);
        bot.SetActive(false);

    }

	Vector3 test = Vector3.zero;
    // Update is called once per frame
    void Update () {
        //if (CameraControl.Current == null/* && Interactive.minSelected == false*/)
        //    return;
        //Debug.Log(topSplit.position);
        //Debug.Log(botSplit.position);
        //Debug.Log(v3);

        //float x = Input.mousePosition.x;
        //float y = Input.mousePosition.y;

        test.Set(Input.mousePosition.x, Input.mousePosition.y, zdepth);
        newVector = CameraControl.Current.ScreenToWorldPoint(test);


        if (newVector.z < GameManager.botSplitZ )
        {
            //Debug.Log("Bottom");
            top.SetActive(false);
            center.SetActive(false);
            bot.SetActive(true);
        }
        else if(newVector.z > GameManager.topSplitZ)
        {
            //Debug.Log("Top");
            top.SetActive(true);
            center.SetActive(false);
            bot.SetActive(false);
        }
        else
        {
            //Debug.Log("Middle");
            top.SetActive(false);
            center.SetActive(true);
            bot.SetActive(false);
        }


    }
}

#endif
#endregion //OLD_CODE