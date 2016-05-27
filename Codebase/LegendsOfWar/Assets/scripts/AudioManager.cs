using UnityEngine;
public class AudioManager : MonoBehaviour
{
	static Transform listenerTransform = null;
	static AudioSource BgmSource;
	static AudioSource SfxSource = null;
	[SerializeField]
	AudioSource clickSoundSource = null;
	public static Vector3 ListenerPosition
	{
		get
		{
			if ( listenerTransform )
				return listenerTransform.position;
			return Vector3.zero;
		}
	}
	[SerializeField]
	private AudioClip gameBGM = null, menuBGM = null, endGameBGM = null, HeroCam = null, WaveSpawn = null, TowerProjectile = null, PortalDestroyed = null, PortalAttacked = null, TowerDestroyed = null, TowerAttacked = null, MinionAttack = null, MinionAttacked = null, MinionDeath = null, HeroAttacked = null, HeroDeath = null, ClickSound = null;
	public static AudioClip bgmGame, bgmMenu, bgmEndGame, sfxHeroCam, sfxWaveSpawn, sfxTowerProjectile, sfxPortalDestroyed, sfxPortalAttacked, sfxTowerDestroyed, sfxTowerAttacked, sfxMinionAttack, sfxMinionAttacked, sfxMinionDeath, sfxHeroAttacked, sfxHeroDeath, sfxClickSound;
	static AudioManager instance = null;
	public static AudioManager Instance { get { return instance; } }
	void Awake()
	{
		if ( instance )
			Destroy( gameObject );
		else
		{
			instance = this;
			DontDestroyOnLoad( gameObject );
			InitStatic();
		}
	}
	public void InitStatic()
	{
		bgmGame = gameBGM;
		bgmMenu = menuBGM;
		bgmEndGame = endGameBGM;
		sfxHeroCam = HeroCam;
		sfxWaveSpawn = WaveSpawn;
		sfxTowerProjectile = TowerProjectile;
		sfxPortalDestroyed = PortalDestroyed;
		sfxPortalAttacked = PortalAttacked;
		sfxTowerDestroyed = TowerDestroyed;
		sfxTowerAttacked = TowerAttacked;
		sfxMinionAttack = MinionAttack;
		sfxMinionAttacked = MinionAttacked;
		sfxMinionDeath = MinionDeath;
		sfxHeroAttacked = HeroAttacked;
		sfxHeroDeath = HeroDeath;
		sfxClickSound = ClickSound;
	}
	void OnDestroy()
	{
		if ( this == instance )
			instance = null;
		Options.onChangedBgmVolume -= OnChangedBgmVol;
		Options.onChangedSfxVolume -= OnChangedSfxVol;
	}
	void Start()
	{
		clickSoundSource.clip = ClickSound;
		Options.onChangedBgmVolume += OnChangedBgmVol;
		Options.onChangedSfxVolume += OnChangedSfxVol;
		BgmSource = GetComponent<AudioSource>();
		if ( gameBGM && menuBGM )
			BGM_Switch();
		else
			Destroy( gameObject );
		FindListener();
		OnChangedBgmVol();
		OnChangedSfxVol();
	}
	void FindListener()
	{
		if ( null == listenerTransform )
		{
			AudioListener listener = FindObjectOfType<AudioListener>();
			if ( null == listener )
			{
				GameObject go = new GameObject( "AudioListener" );
				go.AddComponent<AudioListener>();
				listenerTransform = go.transform;
			}
			else
				listenerTransform = listener.transform;
			listener.enabled = true;
		}
		if ( null == SfxSource )
		{
			SfxSource = listenerTransform.GetComponent<AudioSource>();
			if ( null == SfxSource )
				SfxSource = listenerTransform.gameObject.AddComponent<AudioSource>();
			SfxSource.volume = Options.sfxVolume;
		}
		BgmSource.transform.position = ListenerPosition;
	}
	void Update()
	{
		FindListener();
		BGM_Switch();
	}
	public static void PlayClickSound()
	{
		instance.clickSoundSource.volume = Options.sfxVolume;
		instance.clickSoundSource.PlayOneShot( sfxClickSound );
	}
	public static void PlaySoundEffect( AudioClip ac )
	{
		if ( SfxSource )
		{
			SfxSource.volume = Options.sfxVolume;
			if ( sfxClickSound == ac )
				PlayClickSound();
			else if ( ac )
				SfxSource.PlayOneShot( ac );
		}
		else
			PlaySoundEffect( ac, Vector3.zero );
	}
	public static void PlaySoundEffect( AudioClip ac, Vector3 _position )
	{
		PlayClipAtPoint( ac, _position );
	}
	private void BGM_Switch()
	{
		switch ( ApplicationManager.Instance.currentState )
		{
			case StateID.STATE_INGAME:
			case StateID.STATE_HELP:
			case StateID.STATE_SELECTION:
			case StateID.STATE_INTRODUCTION:
				if ( BgmSource.clip != gameBGM )
				{
					BgmSource.Stop();
					BgmSource.clip = gameBGM;
					BgmSource.Play();
				}
				break;
			case StateID.STATE_GAME_WON:
			case StateID.STATE_GAME_LOST:
			case StateID.STATE_GAME_DRAW:
				if ( BgmSource.clip != endGameBGM )
				{
					BgmSource.Stop();
					BgmSource.clip = endGameBGM;
					BgmSource.Play();
				}
				break;
			case StateID.STATE_MAIN_MENU:
			case StateID.STATE_CREDITS:
			case StateID.STATE_EXIT:
				if ( BgmSource.clip != menuBGM )
				{
					BgmSource.Stop();
					BgmSource.clip = menuBGM;
					BgmSource.Play();
				}
				break;
			default:
				break;
		}
	}
	private static void PlayClipAtPoint( AudioClip clip, Vector3 _position )
	{
		if ( clip )
		{
			GameObject temp = new GameObject( "AudioClip" );
			temp.transform.position = _position;
			AudioSource aud = temp.AddComponent<AudioSource>();
			aud.clip = clip;
			aud.volume = 0.0f;
			temp.AddComponent<AudioClipVol>().aud = aud;
			aud.Play();
			Destroy( temp, clip.length );
		}
	}
	void OnChangedBgmVol()
	{
		if ( BgmSource )
			BgmSource.volume = Options.bgmVolume;
	}
	void OnChangedSfxVol()
	{
		if ( SfxSource )
			clickSoundSource.volume = SfxSource.volume = Options.sfxVolume;
	}
	static AudioSource singleAud = null;
	public static void KillSingle()
	{
		if ( singleAud )
			singleAud.mute = true;
		singleAud = null;
	}
	public static void PlayClipRaw( AudioClip clp, Transform loc = null, bool isVoice = false, bool single = false )
	{
		if ( !clp )
			return;
		GameObject temp = new GameObject( "AudioClipRaw" );
		if ( loc )
			temp.transform.position = loc.position;
		else
			temp.transform.position = ListenerPosition;
		AudioSource aud = temp.AddComponent<AudioSource>();
		if ( single )
		{
			KillSingle();
			singleAud = aud;
		}
		aud.clip = clp;
		aud.volume = 0.0f;
		if ( isVoice )
		{
			AudioClipVol acv = temp.AddComponent<AudioClipVol>();
			acv.aud = aud;
			acv.isVoice = true;
		}
		else
			temp.AddComponent<AudioClipVol>().aud = aud;
		aud.Play();
		Destroy( temp, clp.length );
	}
}