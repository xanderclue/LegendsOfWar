using UnityEngine;
public abstract class AbilityEBase : AbilityBase
{
	protected override void Update()
	{
		base.Update();
		if ( ( Input.GetKeyDown( KeyCode.E ) && !HeroCamScript.onHero ) || Input.GetKeyDown( KeyCode
			.Alpha3 ) || Input.GetKeyDown( KeyCode.Keypad3 ) )
			TryCast();
		ToggleCursor( ( Input.GetKey( KeyCode.E ) && !HeroCamScript.onHero ) || Input.GetKey(
			KeyCode.Alpha3 ) || Input.GetKey( KeyCode.Keypad3 ) );
	}
	protected override void AbilityActivate()
	{
		heroInfo.heroAudio.PlayClip( "HeroCastAbilityE" );
		base.AbilityActivate();
	}
}