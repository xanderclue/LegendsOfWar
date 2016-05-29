using UnityEngine;
public class AbilityOnHover : MonoBehaviour
{
	[SerializeField]
	private char abilityChoice = '\0';
	private GameObject legend;
	private HeroAbilities abilities;
	private AbilityBase ability;
	private string textEn, textJp;
	private bool isHovering = false;
	public void OnMouseEnter()
	{
		isHovering = true;
	}
	public void OnMouseExit()
	{
		isHovering = false;
	}
	private void Start()
	{
		CharacterSelectionManager.OnChangedCharacter += changedCharacter;
		changedCharacter();
	}
	private void Update()
	{
		if ( !ability )
			changedCharacter();
	}
	private void OnGUI()
	{
		if ( isHovering )
			GenerateBox( Options.Japanese ? textJp : textEn );
	}
	private void OnDestroy()
	{
		CharacterSelectionManager.OnChangedCharacter -= changedCharacter;
	}
	private void changedCharacter()
	{
		legend = CharacterSelectionManager.LegendChoice;
		if ( legend )
		{
			abilities = legend.GetComponent<HeroAbilities>();
			ability = GetAbility();
			if ( ability )
			{
				textEn = ability.abilityNameEn + ": " + ability.abilityDescEn;
				textJp = ability.abilityNameJp + ": " + ability.abilityDescJp;
				return;
			}
		}
		textEn = textJp = "";
	}
	private AbilityBase GetAbility()
	{
		if ( abilities )
			switch ( abilityChoice )
			{
				case 'Q':
				case 'q':
				case '1':
					return abilities.abilityQ;
				case 'W':
				case 'w':
				case '2':
					return abilities.abilityW;
				case 'E':
				case 'e':
				case '3':
					return abilities.abilityE;
				case 'R':
				case 'r':
				case '4':
					return abilities.abilityR;
				default:
					break;
			}
		return null;
	}
	private void GenerateBox( string words )
	{
		if ( words.Length <= 0 )
			return;
		GUIStyle style = new GUIStyle( GUI.skin.box );
		style.normal.textColor = Color.cyan;
		style.fontSize = 24;
		Rect labelRect = GUILayoutUtility.GetRect( new GUIContent( words ), style );
		labelRect.x = Input.mousePosition.x + 25;
		labelRect.y = Screen.height - Input.mousePosition.y;
		GUI.Box( labelRect, words, style );
	}
}