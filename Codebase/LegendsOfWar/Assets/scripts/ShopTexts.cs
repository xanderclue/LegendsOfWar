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
    private static string GetVertical(string inputText)
    {
        string outputText = "";
        foreach (char c in inputText)
            if ('ー' == c)
                outputText += "｜\n";
            else
                outputText += c + "\n";
        return outputText;
    }
    private void Start()
    {
        Options.OnChangedLanguage += SetTexts;
        player = GameManager.Instance.Player;
        if (player)
        {
            info = player.GetComponent<HeroInfo>();
            abilities = player.GetComponent<HeroAbilities>();
        }
        SetTexts();
    }
    private void OnDestroy()
    {
        Options.OnChangedLanguage -= SetTexts;
    }
    private void SetTexts()
    {
        if (!player || !info || !abilities)
            return;
        abilityQCost.text = abilities.GetAbilityQ.abilityCost.ToString();
        abilityWCost.text = abilities.GetAbilityW.abilityCost.ToString();
        abilityECost.text = abilities.GetAbilityE.abilityCost.ToString();
        abilityRCost.text = abilities.GetAbilityR.abilityCost.ToString();
        if (Options.Japanese)
        {
            abilityQName.transform.localRotation = abilityWName.transform.localRotation =
                abilityEName.transform.localRotation = abilityRName.transform.localRotation =
                Quaternion.identity;
            abilityQName.verticalOverflow = abilityWName.verticalOverflow = abilityEName.
                verticalOverflow = abilityRName.verticalOverflow = VerticalWrapMode.Overflow;
            abilityQName.rectTransform.sizeDelta = abilityWName.rectTransform.sizeDelta =
                abilityEName.rectTransform.sizeDelta = abilityRName.rectTransform.sizeDelta = new
                Vector2(250.0f, 650.0f);
            heroName.text = info.heroNameJp;
            abilityQName.text = GetVertical(abilities.GetAbilityQ.abilityNameJp);
            abilityWName.text = GetVertical(abilities.GetAbilityW.abilityNameJp);
            abilityEName.text = GetVertical(abilities.GetAbilityE.abilityNameJp);
            abilityRName.text = GetVertical(abilities.GetAbilityR.abilityNameJp);
        }
        else
        {
            abilityQName.transform.localRotation = abilityWName.transform.localRotation =
                abilityEName.transform.localRotation = abilityRName.transform.localRotation =
                Quaternion.Euler(0.0f, 0.0f, 90.0f);
            abilityQName.verticalOverflow = abilityWName.verticalOverflow = abilityEName.
                verticalOverflow = abilityRName.verticalOverflow = VerticalWrapMode.Truncate;
            abilityQName.rectTransform.sizeDelta = abilityWName.rectTransform.sizeDelta =
                abilityEName.rectTransform.sizeDelta = abilityRName.rectTransform.sizeDelta = new
                Vector2(650.0f, 250.0f);
            heroName.text = info.heroNameEn;
            abilityQName.text = abilities.GetAbilityQ.abilityNameEn;
            abilityWName.text = abilities.GetAbilityW.abilityNameEn;
            abilityEName.text = abilities.GetAbilityE.abilityNameEn;
            abilityRName.text = abilities.GetAbilityR.abilityNameEn;
        }
    }
}