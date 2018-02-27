using UnityEngine;
public abstract class AbilityEBase : AbilityBase
{
    protected override void Update()
    {
        base.Update();
        if ((Input.GetKeyDown(KeyCode.E) && !HeroCamScript.IsOnHero) || Input.GetKeyDown(KeyCode
            .Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            TryCast();
        ToggleCursor((Input.GetKey(KeyCode.E) && !HeroCamScript.IsOnHero) || Input.GetKey(
            KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3));
    }
    protected override void AbilityActivate()
    {
        heroInfo.TheHeroAudio.PlayClip("HeroCastAbilityE");
        base.AbilityActivate();
    }
}