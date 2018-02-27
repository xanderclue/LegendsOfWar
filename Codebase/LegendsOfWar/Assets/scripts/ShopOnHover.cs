using UnityEngine;
public enum Description { Q, W, E, R, DEFAULT }
public class ShopOnHover : MonoBehaviour
{
    [SerializeField]
    private Description disc = Description.Q;
    [SerializeField]
    private string textEn = "", textJp = "";
    private Rect labelRect;
    private bool isHovering = false;
    public void OnHover()
    {
        isHovering = true;
    }
    public void OnExit()
    {
        isHovering = false;
    }
    private void Start()
    {
        HeroAbilities abilities = GameManager.Instance.Player.GetComponent<HeroAbilities>();
        switch (disc)
        {
            case Description.Q:
                textEn = abilities.GetAbilityQ.abilityDescEn;
                textJp = abilities.GetAbilityQ.abilityDescEn;
                break;
            case Description.W:
                textEn = abilities.GetAbilityW.abilityDescEn;
                textJp = abilities.GetAbilityW.abilityDescJp;
                break;
            case Description.E:
                textEn = abilities.GetAbilityE.abilityDescEn;
                textJp = abilities.GetAbilityE.abilityDescEn;
                break;
            case Description.R:
                textEn = abilities.GetAbilityR.abilityDescEn;
                textJp = abilities.GetAbilityR.abilityDescJp;
                break;
            default:
                break;
        }
    }
    private void OnGUI()
    {
        if (isHovering)
            GenerateBox(Options.Japanese ? textJp : textEn);
    }
    private void GenerateBox(string words)
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.normal.textColor = Color.green;
        style.fontSize = 20;
        labelRect = GUILayoutUtility.GetRect(new GUIContent(words), style);
        labelRect.x = Input.mousePosition.x + 25.0f;
        labelRect.y = Screen.height - Input.mousePosition.y;
        GUI.Box(labelRect, words, style);
    }
}