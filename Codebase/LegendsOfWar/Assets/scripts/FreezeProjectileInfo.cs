using UnityEngine;

public class FreezeProjectileInfo : BaseProjectileInfo
{
	[SerializeField]
	float slowAmount = 5.0f, AOERadius = 30.0f, AOEDamagePerTick = 20.0f, AOETotalTicks = 3.0f, AOETickTime = 1.0f;

	public float SlowAmount { get { return slowAmount; } }
	public float aoeRadius { get { return AOERadius; } }
	public float aoeDamagePerTick { get { return AOEDamagePerTick; } }
	public float aoeTotalTicks { get { return AOETotalTicks; } }
	public float aoeTickTime { get { return AOETickTime; } }
}
