using UnityEngine;
using UnityEngine.UI;
public class localizationScript : MonoBehaviour
{
	[SerializeField]
	private string english = "", japanese = "";
	private Text text;
	private bool pendingChange = false;
	private void Awake()
	{
		text = GetComponent<Text>();
	}
	private void Start()
	{
		Options.onChangedLanguage += changeText;
		changeText();
	}
	private void Update()
	{
		if ( pendingChange )
		{
			pendingChange = false;
			text = GetComponent<Text>();
			changeText();
		}
	}
	private void OnDestroy()
	{
		Options.onChangedLanguage -= changeText;
	}
	private void changeText()
	{
		if ( text )
			text.text = Options.Japanese ? japanese : english;
		else
			pendingChange = true;
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using UnityEngine.UI;

public class localizationScript : MonoBehaviour
{
	[SerializeField]
	string english = "", japanese = "";//, german = "";
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
#endif
#endregion //OLD_CODE