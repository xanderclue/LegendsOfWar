using UnityEngine;
public class Interactive : MonoBehaviour
{
	[SerializeField]
	private bool selected = false;
	private GameObject Circle;
	private MinionInfo info;
	public bool Selected
	{ get { return selected; } }
	private void Start()
	{
		Circle = transform.Find( "Selection Circle" ).gameObject;
		info = GetComponent<MinionInfo>();
	}
	private void Update()
	{
		if ( GameManager.GameRunning )
		{
			if ( !HeroCamScript.onHero )
				if ( info.Alive && Input.GetMouseButton( 0 ) && Team.BLUE_TEAM == info.team )
				{
					Vector3 camPos = CameraControl.Current.WorldToScreenPoint( transform.position );
					camPos.y = Screen.height - camPos.y;
					selected = CameraControl.Selection.Contains( camPos );
				}
			if ( Circle )
				if ( selected )
				{
					Circle.SetActive( true );
					Circle.transform.Rotate( Vector3.up, 60.0f * Time.deltaTime, Space.World );
				}
				else
					Circle.SetActive( false );
		}
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField]
    bool selected = false;
    private GameObject Circle;
	MinionInfo info;
    public static bool minSelected = false;
    
    void Start()
    {
        Circle = transform.Find("Selection Circle").gameObject;
		info = GetComponent<MinionInfo>();
	}
    
    void Update()
    {
		if ( GameManager.GameRunning )
		{
			// <BUGFIX: Test Team #28>
			if (HeroCamScript.onHero == false)
				if (info.Alive && Input.GetMouseButton(0) && Team.BLUE_TEAM == info.team) {
					Vector3 camPos = CameraControl.Current.WorldToScreenPoint(transform.position);
					camPos.y = Screen.height - camPos.y;
					selected = CameraControl.Selection.Contains(camPos);
				}
			// </BUGFIX: Test Team #28>
			if ( Circle != null )
			{
                if (selected)
                {
                    Circle.SetActive(true);
                    Circle.transform.Rotate(Vector3.up, 60.0f * Time.deltaTime, Space.World);
                    minSelected = true;
                }
                else
                {
                    Circle.SetActive(false);
                    minSelected = false;
                }
			}
		}
    }

	public bool Selected { get { return selected; } }
    public GameObject GameEntity { get { return gameObject; } }
}
#endif
#endregion //OLD_CODE