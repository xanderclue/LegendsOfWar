using UnityEngine;
public class FreezeProjectileInfo : BaseProjectileInfo
{
    [SerializeField]
    private float slowAmount = 5.0f, AOERadius = 30.0f, AOEDamagePerTick = 20.0f, AOETotalTicks =
        3.0f, AOETickTime = 1.0f;
    public float SlowAmount
    { get { return slowAmount; } }
    public float AoeRadius
    { get { return AOERadius; } }
    public float AoeDamagePerTick
    { get { return AOEDamagePerTick; } }
    public float AoeTotalTicks
    { get { return AOETotalTicks; } }
    public float AoeTickTime
    { get { return AOETickTime; } }
}