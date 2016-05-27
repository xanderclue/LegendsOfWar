using UnityEngine;
public class ExplosiveProjectileInfo : BaseProjectileInfo
{
	[SerializeField]
	private float AOERadius = 30.0f, AOEDamage = 20.0f;
	public float aoeRadius { get { return AOERadius; } }
	public float aoeDamage { get { return AOEDamage; } }
}