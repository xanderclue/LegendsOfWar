using UnityEngine;
public class BaseProjectileInfo : MonoBehaviour
{
	[SerializeField]
	private float projectileSpeed = 400.0f, attackSpeed = 0.75f, agroRange = 60.0f, damage = 30.0f;
	public float ProjectileSpeed
	{ get { return projectileSpeed; } }
	public float AttackSpeed
	{ get { return attackSpeed; } }
	public float AgroRange
	{ get { return agroRange; } }
	public float Damage
	{ get { return damage; } }
}