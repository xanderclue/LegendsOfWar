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
	private GameObject[ ] Marked = null;
	private static int MarkNum = 0;
	public bool MarkHit( GameObject _mark )
	{
		if ( MarkNum <= 3 )
		{
			if ( MarkNum >= 1 )
				if ( 3 == MarkNum || Marked[ MarkNum - 1 ] != _mark )
					MarkNum = 0;
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
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class AssassinAbilityW : AbilityWBase {

    // Use this for initialization
    //void Start () {
    //
    //}
    [SerializeField]
    GameObject Target = null;
    [SerializeField]
    int Damage = 0, Speed = 0;
    [SerializeField]
    protected Detector attackTrigger;
    [SerializeField]
    protected GameObject weapon, projectile;


    static int MarkNum = 0;

    public GameObject[] Marked;

    protected override void AbilityActivate()
    {
        base.AbilityActivate();
        FireAtTarget(Target.transform, Speed, Damage);

    }

    protected void FireAtTarget(Transform _target, float _speed, float _damage)
    {

        if (null == _target.gameObject)
            return;

        //weapon.transform.LookAt(_target, _target.up);
        SkillShot p = (Instantiate(projectile, weapon.transform.position, weapon.transform.rotation) as GameObject).GetComponent<SkillShot>();
        p.MarkingAttack = true;
        p.speed = _speed;
        p.damage = _damage;
        p.target = _target;
        p.Shooter = weapon;
        p.effect = m_effect.CreateEffect();


        //TODO: IDK WHAT THIS IS!? //*** This is for the status effects. Abilities have an m_effect which and be modified; 
                                    //** Useful to be able tp PASS the effect so something else (like a skillshot projectile) 
                                     // * can apply it later
        //p.effect.Name = "AssW";
        //p.effect.Damage = 100;
        //p.effect.Duration = 10;
        //p.effect.TickRate = 1;
        //p.transform.parent = info.projectileSpawnPoint.transform;
        p.Fire();
        //AudioManager.Instance.PlaySoundEffect("TowerProjectile", transform.position);

        //Do mark stuff
        //Mark Target
        //If target is marked do higher damage
        //Mark up to 3 times

    }

    public bool MarkHit(GameObject _mark)
    {
        if (MarkNum <= 3)
        {
            if(MarkNum >=1)
            {
                
                if (Marked[MarkNum - 1] != _mark)
                    MarkNum = 0;
                if (MarkNum == 3)
                    MarkNum = 0;
            }


            Marked[MarkNum++] = _mark;
            Debug.Log(Marked);
            return true;
        }
        else
        {
            return false;
        }
    }
}

#endif
#endregion //OLD_CODE