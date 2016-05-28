using UnityEngine;
using UnityEngine.UI;
public class BarNumeric : MonoBehaviour
{
	[SerializeField]
	private Text hpText = null, manaText = null;
	private HeroInfo info;

	private void Start()
	{
		info = GameManager.Instance.Player.GetComponent<HeroInfo>();
	}
	private void Update()
	{
		hpText.text = Mathf.RoundToInt( info.HP ) + " / " + Mathf.RoundToInt( info.MAXHP );
		manaText.text = Mathf.RoundToInt( info.Mana ) + " / " + Mathf.RoundToInt( info.MaxMana );
	}
}