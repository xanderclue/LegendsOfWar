using UnityEngine;
public class HeroAudio : MonoBehaviour
{
	public AudioClip CHeroTaunt1 = null, CHeroTaunt2 = null, CHeroIdle1 = null, CHeroIdle2 = null;
	[SerializeField]
	private AudioClip CHeroSelected = null, CHeroAttack = null, CHeroCastAbilityQ = null,
		CHeroCastAbilityW = null, CHeroCastAbilityE = null, CHeroCastAbilityR = null;
	[SerializeField]
	private string JHeroTaunt1 = "", JHeroTaunt2 = "", JHeroIdle1 = "", JHeroIdle2 = "",
		JHeroSelected = "", JHeroAttack = "", JHeroCastAbilityQ = "", JHeroCastAbilityW = "",
		JHeroCastAbilityE = "", JHeroCastAbilityR = "";
	private AudioSource source;
	public float PlayClip( string clip )
	{
		AudioClip clp = null;
		string jStr = "";
		switch ( clip )
		{
			case "HeroTaunt1":
				clp = CHeroTaunt1;
				jStr = JHeroTaunt1;
				break;
			case "HeroTaunt2":
				clp = CHeroTaunt2;
				jStr = JHeroTaunt2;
				break;
			case "HeroIdle1":
				clp = CHeroIdle1;
				jStr = JHeroIdle1;
				break;
			case "HeroIdle2":
				clp = CHeroIdle2;
				jStr = JHeroIdle2;
				break;
			case "HeroSelected":
				clp = CHeroSelected;
				jStr = JHeroSelected;
				break;
			case "HeroAttack":
				clp = CHeroAttack;
				jStr = JHeroAttack;
				break;
			case "HeroCastAbilityQ":
				clp = CHeroCastAbilityQ;
				jStr = JHeroCastAbilityQ;
				break;
			case "HeroCastAbilityW":
				clp = CHeroCastAbilityW;
				jStr = JHeroCastAbilityW;
				break;
			case "HeroCastAbilityE":
				clp = CHeroCastAbilityE;
				jStr = JHeroCastAbilityE;
				break;
			case "HeroCastAbilityR":
				clp = CHeroCastAbilityR;
				jStr = JHeroCastAbilityR;
				break;
			default:
				break;
		}
		if ( clp )
		{
			if ( clp == CHeroSelected && CHeroSelected )
				AudioManager.PlayClipRaw( CHeroSelected, null, true, true );
			else
				source.PlayOneShot( clp );
			if ( Options.Japanese )
				sub.SetSub( jStr, clp.length + 0.35f );
			return clp.length;
		}
		return 0.0f;
	}
	private void Start()
	{
		source = GetComponent<AudioSource>();
		Options.onChangedVoiceVolume += SetVoiceVolume;
		SetVoiceVolume();
	}
	private void OnDestroy()
	{
		Options.onChangedVoiceVolume -= SetVoiceVolume;
	}
	private void SetVoiceVolume()
	{
		source.volume = Options.voiceVolume;
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public class HeroAudio : MonoBehaviour
{
	//MUST PUT FILES IN THIS ORDER, IF THERE IS NOT A SOUND FOR IT LEAVE IT NULL
	public AudioClip
	CHeroTaunt1 = null,
	CHeroTaunt2 = null,
	CHeroIdle1 = null,
	CHeroIdle2 = null,
	CHeroSelected = null,
	CHeroAttack = null,
	CHeroCastAbilityQ = null,
	CHeroCastAbilityW = null,
	CHeroCastAbilityE = null,
	CHeroCastAbilityR = null;
	[SerializeField]
	private string
	JHeroTaunt1 = "",
	JHeroTaunt2 = "",
	JHeroIdle1 = "",
	JHeroIdle2 = "",
	JHeroSelected = "",
	JHeroAttack = "",
	JHeroCastAbilityQ = "",
	JHeroCastAbilityW = "",
	JHeroCastAbilityE = "",
	JHeroCastAbilityR = "";
	AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();
		Options.onChangedVoiceVolume += SetVoiceVolume;
		SetVoiceVolume();
	}

	void OnDestroy() { Options.onChangedVoiceVolume -= SetVoiceVolume; }

	void SetVoiceVolume()
	{
		source.volume = Options.voiceVolume;
	}

	public float PlayClip( string clip )
	{
		AudioClip clp = null;
		string jStr = "";
		switch ( clip )
		{
			case "HeroTaunt1":
				clp = CHeroTaunt1;
				jStr = JHeroTaunt1;
				break;
			case "HeroTaunt2":
				clp = CHeroTaunt2;
				jStr = JHeroTaunt2;
				break;
			case "HeroIdle1":
				clp = CHeroIdle1;
				jStr = JHeroIdle1;
				break;
			case "HeroIdle2":
				clp = CHeroIdle2;
				jStr = JHeroIdle2;
				break;
			case "HeroSelected":
				clp = CHeroSelected;
				jStr = JHeroSelected;
				break;
			case "HeroAttack":
				clp = CHeroAttack;
				jStr = JHeroAttack;
				break;
			case "HeroCastAbilityQ":
				clp = CHeroCastAbilityQ;
				jStr = JHeroCastAbilityQ;
				break;
			case "HeroCastAbilityW":
				clp = CHeroCastAbilityW;
				jStr = JHeroCastAbilityW;
				break;
			case "HeroCastAbilityE":
				clp = CHeroCastAbilityE;
				jStr = JHeroCastAbilityE;
				break;
			case "HeroCastAbilityR":
				clp = CHeroCastAbilityR;
				jStr = JHeroCastAbilityR;
				break;
			default:
				break;
		}

		if ( clp )
		{
			if ( clp == CHeroSelected )
				HeroSelectedSound();
			else
				source.PlayOneShot( clp );
			if ( Options.Japanese )
				sub.SetSub( jStr, clp.length + 0.35f );
			return clp.length;
		}
		return 0.0f;
	}

	void HeroSelectedSound()
	{
		if ( CHeroSelected )
			AudioManager.PlayClipRaw( CHeroSelected, null, true, true );
	}
}
#endif
#endregion //OLD_CODE