using UnityEngine;
public abstract class AbilityWBase : AbilityBase
{
	protected override void Update()
	{
		base.Update();
		if ( ( Input.GetKeyDown( KeyCode.W ) && !HeroCamScript.onHero ) || Input.GetKeyDown( KeyCode.Alpha2 ) || Input.GetKeyDown( KeyCode.Keypad2 ) )
			TryCast();
		ToggleCursor( ( Input.GetKey( KeyCode.W ) && !HeroCamScript.onHero ) || Input.GetKey( KeyCode.Alpha2 ) || Input.GetKey( KeyCode.Keypad2 ) );
	}
	protected override void AbilityActivate()
	{
		heroInfo.heroAudio.PlayClip( "HeroCastAbilityW" );
		base.AbilityActivate();
	}
}