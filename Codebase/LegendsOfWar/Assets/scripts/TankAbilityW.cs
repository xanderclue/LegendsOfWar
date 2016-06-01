using UnityEngine;
public class TankAbilityW : AbilityWBase
{
	public bool skillON = false;
	public float Wdamage;
	public CollisionDetector coll;
	private ParticleSystem AbilityWParticle;
	private HeroMovement movement;
	protected override void Start()
	{
		base.Start();
		AbilityWParticle = GameObject.FindGameObjectWithTag( "PW" ).GetComponent<ParticleSystem>();
		AbilityWParticle.Stop();
		movement = heroInfo.GetComponent<HeroMovement>();
	}
	protected override void AbilityActivate()
	{
		base.AbilityActivate();
		skillON = true;
		AbilityWParticle.transform.position = new Vector3( heroInfo.transform.position.x, 1.0f,
			heroInfo.transform.position.z );
		AbilityWParticle.Play();
		movement.SprintingAbility = true;
		coll.DealDamage( CCharge );
	}
	protected override void AbilityDeactivate()
	{
		base.AbilityDeactivate();
		movement.SprintingAbility = false;
		skillON = false;
		AbilityWParticle.transform.localPosition += new Vector3( 0.0f, -10.0f );
		AbilityWParticle.Stop();
		AbilityWParticle.Clear();
	}
	private void CCharge( Info entity )
	{
		if ( entity )
			if ( entity is MinionInfo )
				entity.TakeDamage( Wdamage );
	}
}
#region OLD_CODE
#if false
using UnityEngine;

public class TankAbilityW : AbilityWBase
{
    GameObject AbilityWParticle;

    public bool skillON = false;
    public float Wdamage;

    HeroMovement movement;

    public CollisionDetector coll;

    protected override void Start()
    {
        base.Start();
        AbilityWParticle = GameObject.FindGameObjectWithTag("PW");
        AbilityWParticle.GetComponent<ParticleSystem>().Stop();
        movement = GetComponentInParent<HeroMovement>();
    }

    protected override void AbilityActivate()
	{
		base.AbilityActivate();
        skillON = true;
        AbilityWParticle.GetComponent<Transform>().localPosition = new Vector3(GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>().localPosition.x, 1, GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>().localPosition.z);
        AbilityWParticle.GetComponent<ParticleSystem>().Play();

        movement.SprintingAbility = true;
        //if (coll.metEnemy == true)
        //{
        //    coll.metEnemy = false;
        //    AbilityDeactivate();
        //}
        coll.DealDamage(CCharge);

    }


    protected override void AbilityDeactivate()
    {
        movement.SprintingAbility = false;
        base.AbilityDeactivate();
        skillON = false;
        AbilityWParticle.GetComponent<Transform>().localPosition -= new Vector3(0, 10);
        AbilityWParticle.GetComponent<ParticleSystem>().Stop();
        AbilityWParticle.GetComponent<ParticleSystem>().Clear();

    }

    void CCharge(Info entity)
    {
        if (entity)
            if (entity is MinionInfo)
            {
                entity.TakeDamage(Wdamage);
            }
    }
}
#endif
#endregion //OLD_CODE