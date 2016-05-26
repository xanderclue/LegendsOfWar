using UnityEngine;

public class AbilityOnHover : MonoBehaviour
{
	bool isHovering = false;
	HeroAbilities abilities;
	GameObject legend;
	AbilityBase ability;
	[SerializeField]
	char abilityChoice;
	string textEn, textJp;

	void Start()
	{
		CharacterSelectionManager.OnChangedCharacter += changedCharacter;
		changedCharacter();
	}

	void OnDestroy()
	{
		CharacterSelectionManager.OnChangedCharacter -= changedCharacter;
	}

	void changedCharacter()
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

	void Update()
	{
		if ( !ability )
			changedCharacter();
	}

	AbilityBase GetAbility()
	{
		if ( abilities )
		{
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
		}
		return null;
	}

	public void OnMouseEnter()
	{
		isHovering = true;
	}

	public void OnMouseExit()
	{
		isHovering = false;
	}

	void OnGUI()
	{
		if ( isHovering )
			GenerateBox( Options.Japanese ? textJp : textEn );
	}

	void GenerateBox( string words )
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
