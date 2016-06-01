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