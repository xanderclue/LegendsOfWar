using UnityEngine;
using System.Collections.Generic;
public class TankAbilityE : AbilityEBase
{
    public CollisionDetector coll;
    public float Edamage;
    private List<Info> slowed;
    private ParticleSystem AbilityEParticle;
    protected override void Start()
    {
        base.Start();
        AbilityEParticle = GameObject.FindGameObjectWithTag("PE").GetComponent<ParticleSystem>();
        AbilityEParticle.Stop();
        slowed = new List<Info>();
    }
    protected override void AbilityActivate()
    {
        base.AbilityActivate();
        AbilityEParticle.transform.localPosition = new Vector3(heroInfo.transform.localPosition.x,
            1.0f, heroInfo.transform.localPosition.z);
        AbilityEParticle.Play();
        if (coll)
            coll.DealDamage(Slow);
    }
    protected override void AbilityDeactivate()
    {
        base.AbilityDeactivate();
        if (0 != slowed.Count)
            for (int i = 0; i < slowed.Count; ++i)
                if (slowed[i])
                    slowed[i].gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed += 10.0f;
        slowed.Clear();
        AbilityEParticle.transform.localPosition += new Vector3(0.0f, -10.0f);
        AbilityEParticle.Stop();
        AbilityEParticle.Clear();
    }
    private void Slow(Info entity)
    {
        if (entity)
            if (entity is MinionInfo)
            {
                slowed.Add(entity);
                entity.TakeDamage(Edamage);
                entity.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed -= 10.0f;
            }
    }
}