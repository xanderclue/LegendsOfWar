using UnityEngine;
using System.Collections;

public class AssassinAblilityQ : AbilityQBase
{

	// This ability follows a typical skillshot, throwing a poisoned dagger to deal damage to whoever it hits
	//    Poison Shot :
	//- show skill-shot indicator
	//- throw greenish dagger
	//- add green particles on hit enemy

	[SerializeField]
	GameObject Target = null;
	[SerializeField]
	int Damage = 0, Speed = 0;
	[SerializeField]
	protected Detector attackTrigger;
	[SerializeField]
	protected GameObject weapon, projectile;

	[SerializeField]
	GameObject Indicator;

	bool aiming, waitForCast;

	protected override void Start()
	{
		base.Start();
		Indicator.SetActive( false );
	}



	// Update is called once per frame
	protected override void Update()
	{
		//base.Update();
		skillTimer -= Time.deltaTime;
		if ( abilityOn && skillTimer <= 0.0f )
			AbilityDeactivate();

		if ( !aiming )
		{
			Indicator.SetActive( false );

			//GetComponent<LineRenderer>().enabled = false;
			if ( ( Input.GetKeyDown( KeyCode.Q ) && !HeroCamScript.onHero ) ||
			Input.GetKeyDown( KeyCode.Alpha1 ) ||
			Input.GetKeyDown( KeyCode.Keypad1 ) )
				aiming = true;
		}
		// <BUGFIX: Test Team #32>
		if ( !EnoughMana )
			aiming = false;
		// </BUGFIX: Test Team #32>
		if ( aiming )
		{
			Indicator.SetActive( true );

			//transform.position.Set(transform.position.x, transform.position.y, this.range);
			//Vector3[] range = new Vector3[] { transform.parent.position, transform.position };
			//GetComponent<LineRenderer>().SetPositions(range);
			//GetComponent<LineRenderer>().enabled = true;
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

		if ( null == _target.gameObject )
			return;

		//weapon.transform.LookAt(_target, _target.up);
		SkillShot p = ( Instantiate( projectile, weapon.transform.position, weapon.transform.rotation ) as GameObject ).GetComponent<SkillShot>();
		p.speed = _speed;
		p.damage = _damage;
		p.target = _target;
		p.Shooter = weapon;

		p.effect = m_effect.CreateEffect();
		//p.transform.parent = info.projectileSpawnPoint.transform;
		p.Fire();
		//AudioManager.Instance.PlaySoundEffect("TowerProjectile", transform.position);

	}


}