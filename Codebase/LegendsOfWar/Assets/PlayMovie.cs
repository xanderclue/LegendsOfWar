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
