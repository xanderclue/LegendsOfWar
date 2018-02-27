using UnityEngine;
public class SiegeProjectile : MonoBehaviour
{
    public float speed, damage;
    [SerializeField]
    private bool lazer = false;
    public float projectileLifetime = 2.0f;
    private Info colInfo;
    private float projectileTimer;
    private void Start()
    {
        projectileTimer = projectileLifetime;
    }
    private void Update()
    {
        if (projectileTimer <= 0.0f || GameManager.GameEnded)
            Destroy(gameObject);
        else
            projectileTimer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider col)
    {
        colInfo = col.gameObject.GetComponent<Info>();
        if (colInfo)
            if (Team.BLUE_TEAM == colInfo.team)
            {
                colInfo.TakeDamage(damage + 1.0f);
                if (lazer)
                {
                    Effect effect = new Effect
                    {
                        m_type = StatusEffectType.DOT,
                        m_duration = 3.0f,
                        m_name = "Lazer Burn",
                        m_tickRate = 0.5f,
                        m_damage = Mathf.Max(damage, 25.0f)
                    };
                    StatusEffects.Inflict(col.gameObject, effect.CreateEffect());
                }
                Destroy(gameObject);
            }
    }
}