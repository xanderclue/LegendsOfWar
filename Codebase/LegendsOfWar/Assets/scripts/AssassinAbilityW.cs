using UnityEngine;
public class AssassinAbilityW : AbilityWBase
{
	[SerializeField]
	private GameObject Target = null;
	[SerializeField]
	private int Damage = 0, Speed = 0;
	public Detector attackTrigger;
	[SerializeField]
	private GameObject weapon = null, projectile = null;
	[SerializeField]
	private GameObject[ ] Marked;
	private static int MarkNum = 0;
	public bool MarkHit( GameObject _mark )
	{
		if ( MarkNum <= 3 )
		{
			if ( MarkNum >= 1 )
			{
				if ( Marked[ MarkNum - 1 ] != _mark )
					MarkNum = 0;
				if ( 3 == MarkNum )
					MarkNum = 0;
			}
			Marked[ MarkNum++ ] = _mark;
			return true;
		}
		return false;
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		if ( Target )
		{
			SkillShot p = ( Instantiate( projectile, weapon.transform.position, weapon.transform.
				rotation ) as GameObject ).GetComponent<SkillShot>();
			p.MarkingAttack = true;
			p.speed = Speed;
			p.damage = Damage;
			p.target = Target.transform;
			p.Shooter = weapon;
			p.effect = m_effect.CreateEffect();
			p.Fire();
		}
	}
}