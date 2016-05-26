using UnityEngine;

public class localization2 : MonoBehaviour
{
	[SerializeField]
	GameObject englishObj = null, japaneseObj = null;

	void Start()
	{
		Options.onChangedLanguage += changeObj;
		changeObj();
	}

	void OnDestroy()
	{
		Options.onChangedLanguage -= changeObj;
	}

	void changeObj()
	{
		if ( Options.Japanese )
		{
			japaneseObj.SetActive( true );
			englishObj.SetActive( false );
		}
		else
		{
			englishObj.SetActive( true );
			japaneseObj.SetActive( false );
		}
	}
}