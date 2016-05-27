using UnityEngine;
public class Wind : MonoBehaviour
{
	void Start()
	{
		transform.rotation = Quaternion.Euler( new Vector3( 0.0f, Random.Range( -30.0f, 100.0f ),
			0.0f ) );
	}
}