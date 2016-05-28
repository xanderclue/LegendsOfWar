using UnityEngine;
public class localization2 : MonoBehaviour
{
	[SerializeField]
	private GameObject englishObj = null, japaneseObj = null;

	private void Start()
	{
		Options.onChangedLanguage += changeObj;
		changeObj();
	}
	private void OnDestroy()
	{
		Options.onChangedLanguage -= changeObj;
	}
	private void changeObj()
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