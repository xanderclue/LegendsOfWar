using UnityEngine;

public class HeroAudio : MonoBehaviour
{
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