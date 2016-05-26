using UnityEngine;

public class ShopOnHover : MonoBehaviour
{
	enum Description { Q, W, E, R, DEFAULT };

	[SerializeField]
	Description disc = Description.Q;

	bool isHovering = false;
	[SerializeField]
	string textEn = "", textJp = "";

	void Start()
	{
		HeroAbilities abilities = GameManager.Instance.Player.GetComponent<HeroAbilities>();
		switch ( disc )
		{
			case Description.Q:
				textEn = abilities.abilityQ.abilityDescEn;
				textJp = abilities.abilityQ.abilityDescEn;
				break;
			case Description.W:
				textEn = abilities.abilityW.abilityDescEn;
				textJp = abilities.abilityW.abilityDescJp;
				break;
			case Description.E:
				textEn = abilities.abilityE.abilityDescEn;
				textJp = abilities.abilityE.abilityDescEn;
				break;
			case Description.R:
				textEn = abilities.abilityR.abilityDescEn;
				textJp = abilities.abilityR.abilityDescJp;
				break;
			default:
				break;
		}

	}

	public void OnHover()
	{
		isHovering = true;
	}

	public void OnExit()
	{
		isHovering = false;
	}

	void OnGUI()
	{
		if ( isHovering == true )
			GenerateBox( Options.Japanese ? textJp : textEn );
	}

	void GenerateBox( string words )
	{
		GUIStyle style = new GUIStyle( GUI.skin.box );
		style.normal.textColor = Color.green;
		style.fontSize = 20;

		Rect labelRect = GUILayoutUtility.GetRect( new GUIContent( words ), style );
		labelRect.x = Input.mousePosition.x + 25;
		labelRect.y = Screen.height - Input.mousePosition.y;
		GUI.Box( labelRect, words, style );
	}
}
