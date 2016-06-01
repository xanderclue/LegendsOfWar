using UnityEngine;
public class Wind : MonoBehaviour
{
	private void Start()
	{
		transform.rotation = Quaternion.Euler( new Vector3( 0.0f, Random.Range( -30.0f, 100.0f ),
			0.0f ) );
		Destroy( this );
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public class Wind : MonoBehaviour
{
	void Start()
	{
		transform.rotation = Quaternion.Euler( new Vector3( 0.0f,
			Random.Range( -30.0f, 100.0f ), 0.0f ) );
	}
}
#endif
#endregion //OLD_CODE