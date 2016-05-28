using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour
{
	[SerializeField]
	private Slider bgmSlider = null, sfxSlider = null, voiceSlider = null;
	[SerializeField]
	private GameObject menuCam = null;
	[SerializeField]
	private AudioClip voice = null;
	public delegate void optionsChangedEvent();
	public static event optionsChangedEvent onChangedLanguage;
	public static event optionsChangedEvent onChangedBgmVolume;
	public static event optionsChangedEvent onChangedSfxVolume;
	public static event optionsChangedEvent onChangedVoiceVolume;
	public static bool IsAdditive
	{ get; set; }
	public static SystemLanguage applicationLanguage
	{ get; private set; }
	public static float bgmVolume
	{ get; private set; }
	public static float sfxVolume
	{ get; private set; }
	public static float voiceVolume
	{ get; private set; }
	public static bool Japanese
	{ get { return SystemLanguage.Japanese == applicationLanguage; } }
	private static string language
	{ get; set; }

	public static void Init()
	{
		IsAdditive = false;
		applicationLanguage = SystemLanguage.English;
		bgmVolume = 0.25f;
		sfxVolume = 0.8f;
		voiceVolume = 1.0f;
		language = "English";
		bgmVolume = PlayerPrefs.GetFloat( "MusicVolume", bgmVolume );
		PlayerPrefs.SetFloat( "MusicVolume", bgmVolume );
		if ( onChangedBgmVolume != null )
			onChangedBgmVolume();
		sfxVolume = PlayerPrefs.GetFloat( "SfxVolume", sfxVolume );
		PlayerPrefs.SetFloat( "SfxVolume", sfxVolume );
		if ( onChangedSfxVolume != null )
			onChangedSfxVolume();
		voiceVolume = PlayerPrefs.GetFloat( "VoiceVolume", voiceVolume );
		PlayerPrefs.SetFloat( "VoiceVolume", voiceVolume );
		if ( onChangedVoiceVolume != null )
			onChangedVoiceVolume();
		language = PlayerPrefs.GetString( "Language", language );
		PlayerPrefs.SetString( "Language", language );
		applicationLanguage = "Japanese" == language ?
			SystemLanguage.Japanese : SystemLanguage.English;
		if ( onChangedLanguage != null )
			onChangedLanguage();
	}
	private void Awake()
	{
		if ( IsAdditive )
			menuCam.SetActive( false );
	}
	private void Start()
	{
		bgmSlider.normalizedValue = bgmVolume;
		sfxSlider.normalizedValue = sfxVolume;
		voiceSlider.normalizedValue = voiceVolume;
	}
	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Escape ) )
			ApplicationManager.ReturnToPreviousState();
	}
	public void PlayTestSound()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxTowerAttacked );
	}
	public void PlayVoiceSound()
	{
		AudioManager.PlayClipRaw( voice, null, true );
	}
	public void BgmVolumeChanging()
	{
		bgmVolume = bgmSlider.normalizedValue;
		PlayerPrefs.SetFloat( "MusicVolume", bgmVolume );
		if ( onChangedBgmVolume != null )
			onChangedBgmVolume();
	}
	public void SfxVolumeChanging()
	{
		sfxVolume = sfxSlider.normalizedValue;
		PlayerPrefs.SetFloat( "SfxVolume", sfxVolume );
		if ( onChangedSfxVolume != null )
			onChangedSfxVolume();
	}
	public void VoiceVolumeChanging()
	{
		voiceVolume = voiceSlider.normalizedValue;
		PlayerPrefs.SetFloat( "VoiceVolume", voiceVolume );
		if ( onChangedVoiceVolume != null )
			onChangedVoiceVolume();
	}
	public void toggleLanguage()
	{
		toggleLanguage_Static();
	}
	public static void toggleLanguage_Static()
	{
		switch ( applicationLanguage )
		{
			case SystemLanguage.English:
				applicationLanguage = SystemLanguage.Japanese;
				break;
			default:
				applicationLanguage = SystemLanguage.English;
				break;
		}
		PlayerPrefs.SetString( "Language", Japanese ? "Japanese" : "English" );
		if ( onChangedLanguage != null )
			onChangedLanguage();
	}
}