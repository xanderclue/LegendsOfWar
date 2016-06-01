using UnityEngine;
public abstract class AbilityQBase : AbilityBase
{
	protected override void Update()
	{
		base.Update();
		if ( ( Input.GetKeyDown( KeyCode.Q ) && !HeroCamScript.onHero ) || Input.GetKeyDown( KeyCode
			.Alpha1 ) || Input.GetKeyDown( KeyCode.Keypad1 ) )
			TryCast();
		ToggleCursor( ( Input.GetKey( KeyCode.Q ) && !HeroCamScript.onHero ) || Input.GetKey(
			KeyCode.Alpha1 ) || Input.GetKey( KeyCode.Keypad1 ) );
	}
	protected override void AbilityActivate()
	{
		heroInfo.heroAudio.PlayClip( "HeroCastAbilityQ" );
		base.AbilityActivate();
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public abstract class AbilityQBase : AbilityBase
{
	protected override void Update()
	{
		base.Update();
		if ( ( Input.GetKeyDown( KeyCode.Q ) && !HeroCamScript.onHero ) ||
			Input.GetKeyDown( KeyCode.Alpha1 ) ||
			Input.GetKeyDown( KeyCode.Keypad1 ) )
			TryCast();
		ToggleCursor( ( Input.GetKey( KeyCode.Q ) && !HeroCamScript.onHero ) ||
			Input.GetKey( KeyCode.Alpha1 ) ||
			Input.GetKey( KeyCode.Keypad1 ) );
	}

	protected override void AbilityActivate()
	{
		heroInfo.heroAudio.PlayClip( "HeroCastAbilityQ" );
		base.AbilityActivate();
	}
}
#endif
#endregion //OLD_CODE