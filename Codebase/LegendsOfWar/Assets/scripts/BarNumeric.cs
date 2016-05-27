using UnityEngine;
using UnityEngine.UI;
public class BarNumeric : MonoBehaviour
{
	[SerializeField]
	Text hpText = null, manaText = null;
	HeroInfo info;
	void Start()
	{
		info = GameManager.Instance.Player.GetComponent<HeroInfo>();
	}
	void Update()
	{
		hpText.text = Mathf.RoundToInt( info.HP ) + " / " + Mathf.RoundToInt( info.MAXHP );
		manaText.text = Mathf.RoundToInt( info.Mana ) + " / " + Mathf.RoundToInt( info.MaxMana );
	}
}