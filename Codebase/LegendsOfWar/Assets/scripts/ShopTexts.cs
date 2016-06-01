using UnityEngine;
using UnityEngine.UI;
public class ShopTexts : MonoBehaviour
{
	[SerializeField]
	private Text heroName = null, abilityQName = null, abilityWName = null, abilityEName = null,
		abilityRName = null, abilityQCost = null, abilityWCost = null, abilityECost = null,
		abilityRCost = null;
	private GameObject player;
	private HeroInfo info;
	private HeroAbilities abilities;
	private static string getVertical( string inputText )
	{
		string outputText = "";
		foreach ( char c in inputText )
			if ( 'ー' == c )
				outputText += "｜\n";
			else
				outputText += c + "\n";
		return outputText;
	}
	private void Start()
	{
		Options.onChangedLanguage += SetTexts;
		player = GameManager.Instance.Player;
		if ( player )
		{
			info = player.GetComponent<HeroInfo>();
			abilities = player.GetComponent<HeroAbilities>();
		}
		SetTexts();
	}
	private void OnDestroy()
	{
		Options.onChangedLanguage -= SetTexts;
	}
	private void SetTexts()
	{
		if ( !player || !info || !abilities )
			return;
		abilityQCost.text = abilities.abilityQ.abilityCost.ToString();
		abilityWCost.text = abilities.abilityW.abilityCost.ToString();
		abilityECost.text = abilities.abilityE.abilityCost.ToString();
		abilityRCost.text = abilities.abilityR.abilityCost.ToString();
		if ( Options.Japanese )
		{
			abilityQName.transform.localRotation = abilityWName.transform.localRotation =
				abilityEName.transform.localRotation = abilityRName.transform.localRotation =
				Quaternion.identity;
			abilityQName.verticalOverflow = abilityWName.verticalOverflow = abilityEName.
				verticalOverflow = abilityRName.verticalOverflow = VerticalWrapMode.Overflow;
			abilityQName.rectTransform.sizeDelta = abilityWName.rectTransform.sizeDelta =
				abilityEName.rectTransform.sizeDelta = abilityRName.rectTransform.sizeDelta = new
				Vector2( 250.0f, 650.0f );
			heroName.text = info.heroNameJp;
			abilityQName.text = getVertical( abilities.abilityQ.abilityNameJp );
			abilityWName.text = getVertical( abilities.abilityW.abilityNameJp );
			abilityEName.text = getVertical( abilities.abilityE.abilityNameJp );
			abilityRName.text = getVertical( abilities.abilityR.abilityNameJp );
		}
		else
		{
			abilityQName.transform.localRotation = abilityWName.transform.localRotation =
				abilityEName.transform.localRotation = abilityRName.transform.localRotation =
				Quaternion.Euler( 0.0f, 0.0f, 90.0f );
			abilityQName.verticalOverflow = abilityWName.verticalOverflow = abilityEName.
				verticalOverflow = abilityRName.verticalOverflow = VerticalWrapMode.Truncate;
			abilityQName.rectTransform.sizeDelta = abilityWName.rectTransform.sizeDelta =
				abilityEName.rectTransform.sizeDelta = abilityRName.rectTransform.sizeDelta = new
				Vector2( 650.0f, 250.0f );
			heroName.text = info.heroNameEn;
			abilityQName.text = abilities.abilityQ.abilityNameEn;
			abilityWName.text = abilities.abilityW.abilityNameEn;
			abilityEName.text = abilities.abilityE.abilityNameEn;
			abilityRName.text = abilities.abilityR.abilityNameEn;
		}
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE