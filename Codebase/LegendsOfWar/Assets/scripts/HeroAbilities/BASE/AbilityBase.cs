using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityBase : MonoBehaviour
{
	[SerializeField]
    protected Effect m_effect;
	public Sprite abilityIcon;

    public Effect Effect
    {
        get { return m_effect; }
    }
    protected float cooldownTimer = 0.0f;
    [SerializeField]
    protected float cooldownTime = 10.0f;
    protected bool abilityOn = false;
//    [SerializeField]
//    protected float skillDuration = 0.0f;
    protected float skillTimer = 0.0f;
    [SerializeField]
    protected GameObject cursor = null;
    [SerializeField]
    protected Texture2D CursorIcon = null;

    //public float manaCost = 10.0f;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    protected bool aimingSkill = false;
	protected HeroInfo heroInfo;

    protected virtual void Start()
    { 
		if (m_effect.m_name == "")
			m_effect.m_name = "<n/a>";
		if ( cooldownTime <= m_effect.m_duration )
			cooldownTime = m_effect.m_duration;
		heroInfo = GetComponentInParent<HeroInfo>();
        cursor = GameManager.cursor;
        if (CursorIcon != null)
            hotSpot.Set(CursorIcon.width * 0.5f, CursorIcon.height * 0.5f);
        //cursor.GetComponent<RawImage>().texture = CursorIcon;
    }
    public float Timer { get { return cooldownTimer; } set { cooldownTimer = value; } }
	public bool AbilityOn
	{
		get { return abilityOn; }
		set { if ( value ) TryCast(); else AbilityDeactivate(); }
	}
	protected virtual void AbilityActivate()
    {
        abilityOn = true;
        cooldownTimer = cooldownTime;
        skillTimer = m_effect.m_duration;
        heroInfo.Deidle();
        //AudioManager.Instance.PlaySoundEffect("HeroCastAbility", transform.position);
    }
    protected virtual void AbilityDeactivate()
    {
        abilityOn = false;
        skillTimer = 0.0f;
    }
    public void TryCast()
    {
        if (GameManager.GameRunning)
			// <BUGFIX: Dev Team #22>
			if ( abilityEnabled )
			// <BUGFIX: Dev Team #22>
                if (gameObject.activeInHierarchy)
                    if (cooldownTimer <= 0.0f)
                        if (heroInfo.UseMana(abilityCost))
                            AbilityActivate();
    }
    protected virtual void Update()
    {
        skillTimer -= Time.deltaTime;
        if (abilityOn && skillTimer <= 0.0f)
            AbilityDeactivate();
    }
	// <BUGFIX: Test Team #32>
	public bool EnoughMana { get { return heroInfo.Mana >= abilityCost; } }
	// </BUGFIX: Test Team #32>
    protected void ToggleCursor(bool _bool)
    {

        if (cursor != null)
        {
            if (!(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R)))
                cursor.SetActive(_bool);
        }

        if (_bool)
            Cursor.SetCursor(CursorIcon, hotSpot, cursorMode);
        else
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

	public string abilityDescEn = "";
	public string abilityDescJp = "";
	public string abilityNameEn = "Ability";
	public string abilityNameJp = "スペル";
	public float abilityCost = 10.0f;
	// <BUGFIX: Dev Team #22>
	[HideInInspector] public bool abilityEnabled = true;
	// </BUGFIX: Dev Team #22>
}