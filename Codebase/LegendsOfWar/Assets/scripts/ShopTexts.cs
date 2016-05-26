using UnityEngine;
using UnityEngine.UI;

public class ShopTexts : MonoBehaviour
{
	[SerializeField]
	Text heroName = null,
		abilityQName = null,
		abilityWName = null,
		abilityEName = null,
		abilityRName = null,
		abilityQCost = null,
		abilityWCost = null,
		abilityECost = null,
		abilityRCost = null;

	void Start()
	{
		Options.onChangedLanguage += SetTexts;
		SetTexts();
	}

	void OnDestroy()
	{
		Options.onChangedLanguage -= SetTexts;
	}

	void SetEnStyle()
	{
		abilityQName.transform.localRotation =
		abilityWName.transform.localRotation =
		abilityEName.transform.localRotation =
		abilityRName.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, 90.0f );
		abilityQName.verticalOverflow =
		abilityWName.verticalOverflow =
		abilityEName.verticalOverflow =
		abilityRName.verticalOverflow = VerticalWrapMode.Truncate;
		abilityQName.rectTransform.sizeDelta =
		abilityWName.rectTransform.sizeDelta =
		abilityEName.rectTransform.sizeDelta =
		abilityRName.rectTransform.sizeDelta = new Vector2( 650.0f, 250.0f );
	}

	void SetJpStyle()
	{
		abilityQName.transform.localRotation =
		abilityWName.transform.localRotation =
		abilityEName.transform.localRotation =
		abilityRName.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, 0.0f );
		abilityQName.verticalOverflow =
		abilityWName.verticalOverflow =
		abilityEName.verticalOverflow =
		abilityRName.verticalOverflow = VerticalWrapMode.Overflow;
		abilityQName.rectTransform.sizeDelta =
		abilityWName.rectTransform.sizeDelta =
		abilityEName.rectTransform.sizeDelta =
		abilityRName.rectTransform.sizeDelta = new Vector2( 250.0f, 650.0f );

	}

	void SetTexts()
	{
		GameObject player = GameManager.Instance.Player;
		if ( !player )
			return;
		HeroInfo info = player.GetComponent<HeroInfo>();
		if ( !info )
			return;
		HeroAbilities abilities = player.GetComponent<HeroAbilities>();
		if ( !abilities )
			return;
		abilityQCost.text = abilities.abilityQ.abilityCost.ToString();
		abilityWCost.text = abilities.abilityW.abilityCost.ToString();
		abilityECost.text = abilities.abilityE.abilityCost.ToString();
		abilityRCost.text = abilities.abilityR.abilityCost.ToString();
		if ( Options.Japanese )
		{
			SetJpStyle();
			heroName.text = info.heroNameJp;
			abilityQName.text = getVertical( abilities.abilityQ.abilityNameJp );
			abilityWName.text = getVertical( abilities.abilityW.abilityNameJp );
			abilityEName.text = getVertical( abilities.abilityE.abilityNameJp );
			abilityRName.text = getVertical( abilities.abilityR.abilityNameJp );
		}
		else
		{
			SetEnStyle();
			heroName.text = info.heroNameEn;
			abilityQName.text = abilities.abilityQ.abilityNameEn;
			abilityWName.text = abilities.abilityW.abilityNameEn;
			abilityEName.text = abilities.abilityE.abilityNameEn;
			abilityRName.text = abilities.abilityR.abilityNameEn;
		}
	}

	static string getVertical( string inputText )
	{
		string outputText = "";
		for ( int i = 0; i < inputText.Length; ++i )
		{
			if ( 'ー' == inputText[ i ] )
				outputText += "｜\n";
			else
				outputText += inputText[ i ] + "\n";
		}
		return outputText;
	}
}