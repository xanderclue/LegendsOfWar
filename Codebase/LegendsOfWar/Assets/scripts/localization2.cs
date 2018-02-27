using UnityEngine;
public class localization2 : MonoBehaviour
{
    [SerializeField]
    private GameObject englishObj = null, japaneseObj = null;
    private void Start()
    {
        Options.OnChangedLanguage += ChangeObj;
        ChangeObj();
    }
    private void OnDestroy()
    {
        Options.OnChangedLanguage -= ChangeObj;
    }
    private void ChangeObj()
    {
        if (Options.Japanese)
        {
            japaneseObj.SetActive(true);
            englishObj.SetActive(false);
        }
        else
        {
            englishObj.SetActive(true);
            japaneseObj.SetActive(false);
        }
    }
}