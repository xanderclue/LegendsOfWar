using UnityEngine;
using UnityEngine.UI;
public class AbilityIcon : MonoBehaviour
{
	[SerializeField]
	private Image q = null, w = null, e = null, r = null;
	private HeroAbilities abilities = null;

	private void SetIcons()
	{
		if ( CharacterSelectionManager.LegendChoice )
			abilities = CharacterSelectionManager.LegendChoice.GetComponent<HeroAbilities>();
		if ( abilities )
		{
			q.sprite = abilities.abilityQ.abilityIcon;
			w.sprite = abilities.abilityW.abilityIcon;
			e.sprite = abilities.abilityE.abilityIcon;
			r.sprite = abilities.abilityR.abilityIcon;
		}
	}
	private void Update()
	{
		SetIcons();
	}
	public void Force()
	{
		SetIcons();
	}
}