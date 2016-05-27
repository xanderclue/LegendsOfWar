using UnityEngine;
using UnityEngine.UI;
public class localizationScript : MonoBehaviour
{
	[SerializeField]
	string english = "", japanese = "";
	Text text;
	bool pendingChange = false;
	void Awake()
	{
		text = GetComponent<Text>();
	}
	void Start()
	{
		Options.onChangedLanguage += changeText;
		changeText();
	}
	void OnDestroy()
	{
		Options.onChangedLanguage -= changeText;
	}
	void Update()
	{
		if ( pendingChange )
		{
			pendingChange = false;
			text = GetComponent<Text>();
			changeText();
		}
	}
	void changeText()
	{
		if ( text == null )
			pendingChange = true;
		else if ( Options.Japanese )
			text.text = japanese;
		else
			text.text = english;
	}
}