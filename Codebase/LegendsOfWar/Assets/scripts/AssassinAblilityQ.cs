using UnityEngine;
public class AssassinAblilityQ : AbilityQBase
{
	[SerializeField]
	private GameObject Target = null;
	[SerializeField]
	private int Damage = 0, Speed = 0;
	public Detector attackTrigger;
	[SerializeField]
	private GameObject weapon = null, projectile = null, Indicator = null;
	private bool aiming = false;
	protected override void Start()
	{
		base.Start();
		Indicator.SetActive( false );
	}
	protected override void Update()
	{
		skillTimer -= Time.deltaTime;
		if ( abilityOn && skillTimer <= 0.0f )
			AbilityDeactivate();
		if ( !aiming )
		{
			Indicator.SetActive( false );
			if ( ( Input.GetKeyDown( KeyCode.Q ) && !HeroCamScript.onHero ) || Input.GetKeyDown(
				KeyCode.Alpha1 ) || Input.GetKeyDown( KeyCode.Keypad1 ) )
				aiming = true;
		}
		if ( !EnoughMana )
			aiming = false;
		if ( aiming )
		{
			Indicator.SetActive( true );
			if ( Input.GetMouseButtonDown( 0 ) )
				TryCast();
			else if ( Input.GetMouseButtonDown( 1 ) )
				aiming = false;
		}
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		if ( Target )
		{
			SkillShot p = ( Instantiate( projectile, weapon.transform.position, weapon.transform.
				rotation ) as GameObject ).GetComponent<SkillShot>();
			p.speed = Speed;
			p.damage = Damage;
			p.target = Target.transform;
			p.Shooter = weapon;
			p.effect = m_effect.CreateEffect();
			p.Fire();
		}
		Indicator.SetActive( true );
		aiming = false;
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		Indicator.SetActive( false );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE