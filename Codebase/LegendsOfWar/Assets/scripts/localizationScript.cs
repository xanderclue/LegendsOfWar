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
        Options.OnChangedLanguage += ChangeText;
        ChangeText();
    }
    private void Update()
    {
        if (pendingChange)
        {
            pendingChange = false;
            text = GetComponent<Text>();
            ChangeText();
        }
    }
    private void OnDestroy()
    {
        Options.OnChangedLanguage -= ChangeText;
    }
    private void ChangeText()
    {
        if (text)
            text.text = Options.Japanese ? japanese : english;
        else
            pendingChange = true;
    }
}