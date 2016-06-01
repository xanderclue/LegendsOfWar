using UnityEngine;
public class PlayMovie : MonoBehaviour
{
	[SerializeField]
	private MovieTexture myMovie = null;
	[SerializeField]
	private AudioClip LegendsOfWarMovieBGM = null;
	private void Start()
	{
		myMovie.Play();
		AudioManager.PlaySoundEffect( LegendsOfWarMovieBGM );
	}
	private void OnGUI()
	{
		GUI.DrawTexture( new Rect( 0.0f, 0.0f, Screen.width, Screen.height ), myMovie );
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {
    [SerializeField]
    MovieTexture myMovie;
    [SerializeField]
    AudioClip LegendsOfWarMovieBGM = null;
    void Start()
    {
        myMovie.Play();

        AudioManager.PlaySoundEffect(LegendsOfWarMovieBGM);
    }


    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myMovie);
    }
}

#endif
#endregion //OLD_CODE