using UnityEngine;
public class TankAbilityQ : AbilityQBase
{
	public ParticleSystem AbilityQParticle;
	public float increaseHP;

	private float heromaxhp;
	protected override void Start()
	{
		base.Start();
		AbilityQParticle.Stop();
		heromaxhp = heroInfo.MAXHP;
	}
	protected override void AbilityActivate()
	{
		AbilityQParticle.gameObject.SetActive( true );
		base.AbilityActivate();
		AbilityQParticle.Play();
		if ( heroInfo.HP + increaseHP > heroInfo.MAXHP )
		{
			heroInfo.MAXHP = heroInfo.HP + increaseHP;
			heroInfo.HP = heroInfo.MAXHP;
		}
		else
			heroInfo.HP += increaseHP;
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		AbilityQParticle.Stop();
		AbilityQParticle.Clear();
		heroInfo.MAXHP = heromaxhp;
		if ( heroInfo.HP > heromaxhp )
			heroInfo.HP = heromaxhp;
		else
			heroInfo.HP -= increaseHP;
	}
}