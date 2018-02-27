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
    public static event optionsChangedEvent OnChangedLanguage, OnChangedBgmVolume,
        OnChangedSfxVolume, OnChangedVoiceVolume;
    public static SystemLanguage TheApplicationLanguage
    { get; private set; }
    public static float BgmVolume
    { get; private set; }
    public static float SfxVolume
    { get; private set; }
    public static float VoiceVolume
    { get; private set; }
    public static bool IsAdditive
    { private get; set; }
    public static bool Japanese
    { get { return SystemLanguage.Japanese == TheApplicationLanguage; } }
    private static string TheLanguage
    { get; set; }
    public static void Init()
    {
        IsAdditive = false;
        TheApplicationLanguage = SystemLanguage.English;
        BgmVolume = 0.25f;
        SfxVolume = 0.8f;
        VoiceVolume = 1.0f;
        TheLanguage = "English";
        BgmVolume = PlayerPrefs.GetFloat("MusicVolume", BgmVolume);
        PlayerPrefs.SetFloat("MusicVolume", BgmVolume);
        OnChangedBgmVolume?.Invoke();
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        OnChangedSfxVolume?.Invoke();
        VoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        OnChangedVoiceVolume?.Invoke();
        TheLanguage = PlayerPrefs.GetString("Language", TheLanguage);
        PlayerPrefs.SetString("Language", TheLanguage);
        TheApplicationLanguage = "Japanese" == TheLanguage ?
            SystemLanguage.Japanese : SystemLanguage.English;
        OnChangedLanguage?.Invoke();
    }
    public static void ToggleLanguage_Static()
    {
        switch (TheApplicationLanguage)
        {
            case SystemLanguage.English:
                TheApplicationLanguage = SystemLanguage.Japanese;
                break;
            default:
                TheApplicationLanguage = SystemLanguage.English;
                break;
        }
        PlayerPrefs.SetString("Language", Japanese ? "Japanese" : "English");
        OnChangedLanguage?.Invoke();
    }
    public void PlayTestSound()
    {
        AudioManager.PlaySoundEffect(AudioManager.sfxTowerAttacked);
    }
    public void PlayVoiceSound()
    {
        AudioManager.PlayClipRaw(voice, null, true);
    }
    public void BgmVolumeChanging()
    {
        BgmVolume = bgmSlider.normalizedValue;
        PlayerPrefs.SetFloat("MusicVolume", BgmVolume);
        OnChangedBgmVolume?.Invoke();
    }
    public void SfxVolumeChanging()
    {
        SfxVolume = sfxSlider.normalizedValue;
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        OnChangedSfxVolume?.Invoke();
    }
    public void VoiceVolumeChanging()
    {
        VoiceVolume = voiceSlider.normalizedValue;
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        OnChangedVoiceVolume?.Invoke();
    }
    public void ToggleLanguage()
    {
        ToggleLanguage_Static();
    }
    private void Awake()
    {
        if (IsAdditive)
            menuCam.SetActive(false);
    }
    private void Start()
    {
        bgmSlider.normalizedValue = BgmVolume;
        sfxSlider.normalizedValue = SfxVolume;
        voiceSlider.normalizedValue = VoiceVolume;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ApplicationManager.ReturnToPreviousState();
    }
}