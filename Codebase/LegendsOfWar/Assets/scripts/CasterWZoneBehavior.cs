using UnityEngine;
public class CasterWZoneBehavior : MonoBehaviour
{
    public float zoneDuration;
    [SerializeField]
    private Effect m_effect = null;
    public bool Activate = false;
    private ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (Activate)
        {
            if (ps.isStopped)
                ps.Play();
            zoneDuration -= Time.deltaTime;
            if (zoneDuration <= 0.0f)
                Destroy(gameObject);
        }
    }
    private void OnParticleCollision(GameObject _other)
    {
        if (_other.GetComponentInParent<StatusEffects>())
            if (Team.RED_TEAM == _other.GetComponent<Info>().team)
                StatusEffects.Inflict(_other.gameObject, m_effect.CreateEffect());
    }
}