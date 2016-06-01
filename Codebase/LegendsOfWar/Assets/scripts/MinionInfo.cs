using UnityEngine;
public enum MinionClass { STRIKER_MINION, TANK_MINION, CASTER_MINION, SIEGE_MINION }
public class MinionInfo : Info
{
	[SerializeField]
	private MinionClass type = MinionClass.STRIKER_MINION;
	[SerializeField]
	private int movementSpeed = 0;
	private float baseDamage = 0.0f;
	private bool m_soulDefense = false, isBasicMinionType = true;
	public int MovementSpeed
	{ get { return movementSpeed; } }
	public float Damage
	{
		get { return damage; }
		set { damage = value; }
	}
	public float Range
	{
		get { return attackRange; }
		set { attackRange = value; }
	}
	public float AttackSpeed
	{
		get { return attackSpeed; }
		set { attackSpeed = value; }
	}
	public float AgroRange
	{ get { return agroRange; } }
	public bool soulDefense
	{
		get { return m_soulDefense; }
		set { m_soulDefense = value; }
	}
	public bool IsBasicMinionType
	{ get { return isBasicMinionType; } }
	public override void TakeDamage( float damage )
	{
		if ( m_soulDefense )
			base.TakeDamage( damage * 0.5f );
		else
			base.TakeDamage( damage );
	}
	public void Cyclone()
	{
		baseDamage *= 0.75f;
	}
	protected override void Start()
	{
		base.Start();
		Attacked += MinionAttacked;
		Destroyed += MinionDeath;
		if ( MinionClass.SIEGE_MINION == type )
			isBasicMinionType = false;
		baseDamage = damage;
	}
	private void Update()
	{
		damage = SupportRange.InSupportRange( gameObject ) ? baseDamage * 1.5f : baseDamage;
	}
	private void MinionAttacked()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxMinionAttacked, transform.position );
	}
	private void MinionDeath()
	{
		AudioManager.PlaySoundEffect( AudioManager.sfxMinionDeath, transform.position );
	}
}