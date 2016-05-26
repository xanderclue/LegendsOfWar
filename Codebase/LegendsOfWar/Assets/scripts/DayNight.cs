using UnityEngine;

public class DayNight : MonoBehaviour
{
	public float dayNightCycleDuration = 5.0f;
	[SerializeField]
	Transform sun = null, moon = null;
	bool night;
	public delegate void DayNightEvent();
	public static event DayNightEvent OnDay, OnNight;
	void Update()
	{
		transform.Rotate( Vector3.right, 360.0f / dayNightCycleDuration * Time.deltaTime );
		if ( night != ( moon.position.y >= ( sun.position.y + 1.5f ) ) )
		{
			night = !night;
			if ( night && OnNight != null )
				OnNight();
			else if ( OnDay != null )
				OnDay();
		}
	}
}