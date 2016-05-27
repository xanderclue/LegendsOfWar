using UnityEngine;
public class AssassinAblilityQ : AbilityQBase
{
	[SerializeField]
	GameObject Target = null;
	[SerializeField]
	int Damage = 0, Speed = 0;
	[SerializeField]
	protected Detector attackTrigger;
	[SerializeField]
	protected GameObject weapon, projectile;
	[SerializeField]
	GameObject Indicator = null;
	bool aiming, waitForCast;
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
			if ( ( Input.GetKeyDown( KeyCode.Q ) && !HeroCamScript.onHero ) || Input.GetKeyDown( KeyCode.Alpha1 ) || Input.GetKeyDown( KeyCode.Keypad1 ) )
				aiming = true;
		}
		if ( !EnoughMana )
			aiming = false;
		if ( aiming )
		{
			Indicator.SetActive( true );
			if ( Input.GetMouseButtonDown( 0 ) )
			{
				waitForCast = false;
				TryCast();
			}
			else if ( Input.GetMouseButtonDown( 1 ) )
				aiming = false;
		}
	}
	protected override void AbilityActivate()
	{
		if ( waitForCast )
			return;
		base.AbilityActivate();
		FireAtTarget( Target.transform, Speed, Damage );
		Indicator.SetActive( true );
		aiming = false;
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		Indicator.SetActive( false );
	}
	protected void FireAtTarget( Transform _target, float _speed, float _damage )
	{
		if ( _target )
		{
			SkillShot p = ( Instantiate( projectile, weapon.transform.position, weapon.transform.rotation ) as GameObject ).GetComponent<SkillShot>();
			p.speed = _speed;
			p.damage = _damage;
			p.target = _target;
			p.Shooter = weapon;
			p.effect = m_effect.CreateEffect();
			p.Fire();
		}
	}
}