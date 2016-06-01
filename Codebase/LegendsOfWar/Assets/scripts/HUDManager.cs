using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
	[SerializeField]
	private Text characterName = null;
	[SerializeField]
	private GameObject start = null, NoStart = null, InGameInfo = null, LoreInfo = null, abilityInfo
		= null, iconsPanel = null;
	[SerializeField]
	private Text tHealth = null, tMana = null, tDamage = null, tAttackRange = null, tDps = null,
		tDifficulty = null;
	private Text loreInfoText, abilityInfoText;
	private TurnManager turnManager;
	private HeroInfo info;
	private HeroAbilities abilities;
	private AbilityIcon aIcon;
	private HeroIcon hIcon;
	private void Start()
	{
		turnManager = GetComponent<TurnManager>();
		aIcon = iconsPanel.GetComponent<AbilityIcon>();
		hIcon = iconsPanel.GetComponent<HeroIcon>();
		loreInfoText = LoreInfo.GetComponentInChildren<Text>();
		abilityInfoText = abilityInfo.GetComponentInChildren<Text>();
	}
	private void Update()
	{
		if ( !turnManager || !GameObject.Find( "Characters" ) )
			return;
		if ( CharacterSelectionManager.LegendChoice )
		{
			info = CharacterSelectionManager.heroInfo;
			abilities = info.GetComponent<HeroAbilities>();
			loreInfoText.text = Options.Japanese ? info.roaa : info.Lore;
			abilityInfoText.text = abilities.abilityInfo;
		}
		if ( start || NoStart || InGameInfo || LoreInfo )
		{
			if ( CharacterSelectionManager.Instance.Available[ turnManager.CurrentInt ] )
			{
				start.SetActive( true );
				NoStart.SetActive( false );
				InGameInfo.SetActive( true );
				LoreInfo.SetActive( true );
				abilityInfo.SetActive( true );
				aIcon.Force();
				hIcon.Force();
				iconsPanel.SetActive( true );
				SetGameInfo();
			}
			else
			{
				start.SetActive( false );
				NoStart.SetActive( true );
				InGameInfo.SetActive( false );
				LoreInfo.SetActive( false );
				abilityInfo.SetActive( false );
				iconsPanel.SetActive( false );
				characterName.text = Options.Japanese ? "名前無し" : "NoName";
			}
		}
	}
	private void SetGameInfo()
	{
		info = CharacterSelectionManager.heroInfo;
		if ( Options.Japanese )
		{
			characterName.text = info.heroNameJp;
			tHealth.text = "HP : " + info.MAXHP;
			tMana.text = "MP : " + info.MaxMana;
			tDamage.text = "攻撃力 : " + info.Damage;
			tAttackRange.text = "攻撃範囲 : " + info.Range;
			tDps.text = "火力 : " + info.AttackSpeed * info.Damage;
			switch ( info.difficulty )
			{
				case Difficulty.Easy:
					tDifficulty.text = "難易度 : 簡単";
					break;
				case Difficulty.Hard:
					tDifficulty.text = "難易度 : 難しい";
					break;
				default:
					tDifficulty.text = "難易度 : " + info.difficulty;
					break;
			}
		}
		else
		{
			characterName.text = info.heroNameEn;
			tHealth.text = "Health : " + info.MAXHP;
			tMana.text = "Mana : " + info.MaxMana;
			tDamage.text = "Damage : " + info.Damage;
			tAttackRange.text = "Range : " + info.Range;
			tDps.text = "DPS : " + info.AttackSpeed * info.Damage;
			tDifficulty.text = "Difficulty : " + info.difficulty;
		}
	}
}