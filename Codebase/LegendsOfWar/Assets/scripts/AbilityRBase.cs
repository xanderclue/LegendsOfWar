using UnityEngine;
public abstract class AbilityRBase : AbilityBase
{
	protected override void Update()
	{
		base.Update();
		if ( ( Input.GetKeyDown( KeyCode.R ) && !HeroCamScript.onHero ) || Input.GetKeyDown( KeyCode
			.Alpha4 ) || Input.GetKeyDown( KeyCode.Keypad4 ) )
			TryCast();
		ToggleCursor( ( Input.GetKey( KeyCode.R ) && !HeroCamScript.onHero ) || Input.GetKey(
			KeyCode.Alpha4 ) || Input.GetKey( KeyCode.Keypad4 ) );
	}
	private HeroMovement movement;
	protected override void Start()
	{
		base.Start();
		movement = GetComponentInParent<HeroMovement>();
	}
	protected override void AbilityActivate()
	{
		heroInfo.heroAudio.PlayClip( "HeroCastAbilityR" );
		base.AbilityActivate();
		movement.SprintingAbility = true;
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		movement.SprintingAbility = false;
	}
}