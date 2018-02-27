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
        CharacterSelectionManager.OnChangedCharacter += ChangedCharacter;
        ChangedCharacter();
    }
    private void Update()
    {
        if (!ability)
            ChangedCharacter();
    }
    private void OnGUI()
    {
        if (isHovering)
            GenerateBox(Options.Japanese ? textJp : textEn);
    }
    private void OnDestroy()
    {
        CharacterSelectionManager.OnChangedCharacter -= ChangedCharacter;
    }
    private void ChangedCharacter()
    {
        legend = CharacterSelectionManager.LegendChoice;
        if (legend)
        {
            abilities = legend.GetComponent<HeroAbilities>();
            ability = GetAbility();
            if (ability)
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
        if (abilities)
            switch (abilityChoice)
            {
                case 'Q':
                case 'q':
                case '1':
                    return abilities.GetAbilityQ;
                case 'W':
                case 'w':
                case '2':
                    return abilities.GetAbilityW;
                case 'E':
                case 'e':
                case '3':
                    return abilities.GetAbilityE;
                case 'R':
                case 'r':
                case '4':
                    return abilities.GetAbilityR;
                default:
                    break;
            }
        return null;
    }
    private void GenerateBox(string words)
    {
        if (0 < words.Length)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.normal.textColor = Color.cyan;
            style.fontSize = 24;
            Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(words), style);
            labelRect.x = Input.mousePosition.x + 25.0f;
            labelRect.y = Screen.height - Input.mousePosition.y;
            GUI.Box(labelRect, words, style);
        }
    }
}