using UnityEngine;
using System.Collections.Generic;



public class TankAbilityE : AbilityEBase {

    public CollisionDetector coll;
    GameObject AbilityEParticle;


    public float Edamage;

    List<Info> slowed;


    protected override void Start()
    {
        base.Start();
        AbilityEParticle = GameObject.FindGameObjectWithTag("PE");
        AbilityEParticle.GetComponent<ParticleSystem>().Stop();
        slowed = new List<Info>();
    }


    protected override void AbilityActivate()
    {
        base.AbilityActivate();
        AbilityEParticle.GetComponent<Transform>().localPosition = new Vector3(GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>().localPosition.x, 1, GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>().localPosition.z);
        AbilityEParticle.GetComponent<ParticleSystem>().Play();

        if (coll != null)
            coll.DealDamage(Slow);
     }

    void Slow(Info entity)
    {
        if (entity)
            if (entity is MinionInfo)
            {
                if (entity != null)
                    slowed.Add(entity);
                entity.TakeDamage(Edamage);
                entity.gameObject.GetComponent<NavMeshAgent>().speed -= 10;
            }
    }
    protected override void AbilityDeactivate()
    {
        base.AbilityDeactivate();
        if (slowed.Count != 0 && slowed != null)
            for (int i = 0; i < slowed.Count; i++)
            {
                if (slowed[i] != null)
                {
                    slowed[i].gameObject.GetComponent<NavMeshAgent>().speed += 10;
                }
            }
        slowed.Clear();
        AbilityEParticle.GetComponent<Transform>().localPosition -= new Vector3(0, 10);
        AbilityEParticle.GetComponent<ParticleSystem>().Stop();
        AbilityEParticle.GetComponent<ParticleSystem>().Clear();

    }



}