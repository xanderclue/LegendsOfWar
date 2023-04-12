using UnityEngine;
using UnityEngine.Video;

public class PlayMovie : MonoBehaviour
{
    [SerializeField]
    // private MovieTexture myMovie = null;
    private VideoPlayer myVideo = null;
    [SerializeField]
    private AudioClip LegendsOfWarMovieBGM = null;
    private void Start()
    {
        // myMovie.Play();
        myVideo.Play();
        AudioManager.PlaySoundEffect(LegendsOfWarMovieBGM);
    }
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), /*myMovie*/myVideo.texture);
    }
}