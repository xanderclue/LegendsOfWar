using UnityEngine;
public class HeroUIScript : MonoBehaviour
{
	[SerializeField]
	private GameObject damageNumberPrefab = null;
	[SerializeField]
	private float damageNumberHeight = 10.0f, damageNumberDuration = 1.0f,
		AttackedNotificationDuration = 5.0f;
	[SerializeField]
	private AudioSource heroWarning = null;
	public static float heroDamageNotifTimer = 0.0f;
	private static HeroUIScript inst;
	public static HeroUIScript Instance
	{ get { return inst; } }
	public static bool HeroBeingAttacked
	{
		get { return heroDamageNotifTimer > 0.0f; }
		set
		{
			if ( value && !HeroCamScript.onHero )
				heroDamageNotifTimer = inst.AttackedNotificationDuration;
			else
				heroDamageNotifTimer = -0.0f;
		}
	}
	public static void Damage( float amount, Vector3 position )
	{
		Instantiate( inst.damageNumberPrefab ).GetComponent<DamageNumber>().CreateNumber( -amount,
			position, inst.damageNumberHeight, inst.damageNumberDuration, Color.red );
	}
	public static void Mana( float amount, Transform transf )
	{
		Instantiate( inst.damageNumberPrefab ).GetComponent<DamageNumber>().CreateNumber( -amount,
			transf, inst.damageNumberHeight, inst.damageNumberDuration, Color.blue );
	}
	private void Awake()
	{
		inst = this;
	}
	private void Start()
	{
		heroDamageNotifTimer = 0.0f;
	}
	private void Update()
	{
		if ( HeroCamScript.onHero || !HeroCamScript.heroAlive )
			heroDamageNotifTimer = -0.0f;
		heroDamageNotifTimer -= Time.deltaTime;
		if ( heroWarning )
			heroWarning.mute = !HeroBeingAttacked;
	}
}