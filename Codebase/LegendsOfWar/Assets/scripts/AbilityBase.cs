using UnityEngine;
public abstract class AbilityBase : MonoBehaviour
{
	[SerializeField]
	protected Effect m_effect;
	public Sprite abilityIcon;
	[SerializeField]
	protected float cooldownTime = 10.0f;
	[SerializeField]
	protected GameObject cursor = null;
	[SerializeField]
	protected Texture2D CursorIcon = null;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	public string abilityDescEn = "";
	public string abilityDescJp = "";
	public string abilityNameEn = "Ability";
	public string abilityNameJp = "スペル";
	public float abilityCost = 10.0f;
	protected bool abilityEnabled = true;
	protected float cooldownTimer = 0.0f;
	protected bool abilityOn = false;
	protected float skillTimer = 0.0f;
	protected bool aimingSkill = false;
	protected HeroInfo heroInfo;

	public bool AbilityEnabled { get { return abilityEnabled; } set { abilityEnabled = value; } }
	public Effect Effect { get { return m_effect; } }
	protected virtual void Start()
	{
		if ( m_effect.m_name == "" )
			m_effect.m_name = "<n/a>";
		if ( cooldownTime <= m_effect.m_duration )
			cooldownTime = m_effect.m_duration;
		heroInfo = GetComponentInParent<HeroInfo>();
		cursor = GameManager.cursor;
		if ( CursorIcon )
			hotSpot.Set( CursorIcon.width * 0.5f, CursorIcon.height * 0.5f );
	}
	public float Timer { get { return cooldownTimer; } set { cooldownTimer = value; } }
	public bool AbilityOn
	{
		get { return abilityOn; }
		set
		{
			if ( value )
				TryCast();
			else
				AbilityDeactivate();
		}
	}
	protected virtual void AbilityActivate()
	{
		abilityOn = true;
		cooldownTimer = cooldownTime;
		skillTimer = m_effect.m_duration;
		heroInfo.Deidle();
	}
	protected virtual void AbilityDeactivate()
	{
		abilityOn = false;
		skillTimer = 0.0f;
	}
	public void TryCast()
	{
		if ( GameManager.GameRunning )
			if ( abilityEnabled )
				if ( gameObject.activeInHierarchy )
					if ( cooldownTimer <= 0.0f )
						if ( heroInfo.UseMana( abilityCost ) )
							AbilityActivate();
	}
	protected virtual void Update()
	{
		skillTimer -= Time.deltaTime;
		if ( abilityOn && skillTimer <= 0.0f )
			AbilityDeactivate();
	}
	public bool EnoughMana { get { return heroInfo.Mana >= abilityCost; } }
	protected void ToggleCursor( bool _bool )
	{
		if ( cursor )
			if ( !( Input.GetKey( KeyCode.Q ) || Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.
				E ) || Input.GetKey( KeyCode.R ) ) )
				cursor.SetActive( _bool );
		if ( _bool )
			Cursor.SetCursor( CursorIcon, hotSpot, cursorMode );
		else
			Cursor.SetCursor( null, Vector2.zero, cursorMode );
	}
}