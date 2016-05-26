using UnityEngine;

public class AudioClipVol : MonoBehaviour
{
	public AudioSource aud;
	float maxDist, dist;

	public bool isVoice = false;

	void Start()
	{
		if ( !aud )
			Destroy( this );
	}

	void Update()
	{
		maxDist = CameraControl.AudioDistance;
		dist = ( AudioManager.ListenerPosition - transform.position ).magnitude;
		if ( dist >= maxDist - 5.0f )
			aud.volume = 0.0f;
		else
			aud.volume = ( 1.0f - ( dist / maxDist ) ) *
				( isVoice ? Options.voiceVolume : Options.sfxVolume );
	}
}