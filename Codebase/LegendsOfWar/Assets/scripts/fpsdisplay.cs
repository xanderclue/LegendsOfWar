using UnityEngine;
using UnityEngine.UI;

public class fpsdisplay : MonoBehaviour
{
	int s, f, n;
	[SerializeField]
	Text fpsText = null;
	bool start = false;

	void Start()
	{
		Application.targetFrameRate = 60;
		if ( fpsText )
		{
			fpsText.text = "";
			s = Mathf.FloorToInt( Time.unscaledTime );
			f = 0;
		}
		else
			Destroy( this );
	}

	void Update()
	{
		++f;
		n = Mathf.FloorToInt( Time.unscaledTime );
		if ( s < n )
		{
			if ( start )
			{
				fpsText.text = f.ToString();
				//if ( f < 20 )
				//	Debug.LogWarning(
				//		"FRAMERATE BELOW 30FPS: fps=" + f
				//		+ "@time=" + Time.unscaledTime );
			}
			else
				start = true;
			s = n;
			f = 0;
		}
	}
}