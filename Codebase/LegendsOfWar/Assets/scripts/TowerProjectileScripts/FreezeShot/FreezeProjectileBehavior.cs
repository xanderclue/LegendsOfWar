using UnityEngine;
using System.Collections.Generic;

public class FreezeProjectileBehavior : MonoBehaviour
{
    FreezeProjectileInfo info;

    Team team;
    public Transform target = null;
    bool fired = false;

    void Awake()
    {
        info = TowerManager.Instance.freezeInfo;
        victims = new List<Transform>();
        slowTargets = new List<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (fired && target && target.gameObject)
        {
            if (!target.gameObject.activeInHierarchy && Vector3.Distance(target.position, transform.position) < 1.0f)
            {
                CreateAOEZone();
                return;
            }
            transform.LookAt(target);
            transform.Translate(transform.forward * info.ProjectileSpeed * Time.fixedDeltaTime, Space.World);
        }

        if (aoeActive)
            PlayEffect();

        if (!target || !target.gameObject.activeInHierarchy)
            Destroy(gameObject);
    }

    public void Fire(Team theTeam)
    {
        team = theTeam;
        fired = true;
        if (GetComponentInChildren<ParticleSystem>().isPlaying)
            GetComponentInChildren<ParticleSystem>().Stop();
    }

    void OnTriggerEnter(Collider col)
    {
        if (target != null && !aoeActive && col.gameObject == target.gameObject)
        {
            col.gameObject.GetComponent<Info>().TakeDamage(info.Damage);
            CreateAOEZone();
        }
    }

    [SerializeField]
    Detector AreaOfEffect = null;

    List<Transform> victims;
    List<NavMeshAgent> slowTargets;
    float aoeTimer, heroSpeed = 105, minionSpeed = 15;
    bool aoeActive, targetsAreSlowed, skip;
    int repeat;

    void CreateAOEZone()
    {
        aoeActive = targetsAreSlowed = true;
        fired = skip = false;
        aoeTimer = info.aoeTickTime;
        repeat = 0;

        AreaOfEffect.CreateTrigger(info.aoeRadius);
        AreaOfEffect.triggerEnter += AddTarget;
        AreaOfEffect.triggerExit += RemoveTarget;

        GetComponent<MeshRenderer>().enabled = false;
        AudioManager.PlayClipRaw(GetComponent<AudioSource>().clip, transform);
        GetComponentInChildren<ParticleSystem>().Play();
    }

    void SlowTargetsSpeed()
    {
        for (int i = 0; i < slowTargets.Count; ++i)
        {
            if (slowTargets[i].tag == "Hero")
                slowTargets[i].speed -= info.SlowAmount * 3;
            else
                slowTargets[i].speed -= info.SlowAmount;
        }
    }

    void ReturnTargetsSpeed()
    {
        slowTargets.RemoveAll(item => item == null);

        for (int i = 0; i < slowTargets.Count; ++i)
        {
            if (slowTargets[i].tag == "Hero")
                slowTargets[i].speed = heroSpeed;
            else
                slowTargets[i].speed = minionSpeed;
        }
    }

    void PlayEffect()
    {
        victims.RemoveAll(item => item == null);

        if (repeat >= info.aoeTotalTicks)
        {
            ReturnTargetsSpeed();
            Destroy(gameObject);
        }
        else if (aoeTimer <= 0)
        {
            if (targetsAreSlowed)
            {
                targetsAreSlowed = false;
                SlowTargetsSpeed();
            }
            DamageVictims();
            ++repeat;
            aoeTimer = info.aoeTickTime;
        }
        else
            aoeTimer -= Time.deltaTime;
    }

    void DamageVictims()
    {
        foreach (Transform victim in victims)
            victim.gameObject.GetComponent<Info>().TakeDamage(info.aoeDamagePerTick);
    }

    void AddTarget(GameObject obj)
    {
        skip = true;

        if (obj)
        {
            Info targ = obj.GetComponent<Info>();
            if (targ && targ.team != team)
            {
                victims.Add(obj.transform);
                if (obj.gameObject.GetComponent<NavMeshAgent>())
                {
                    //check to see if we already have this agent in the list
                    foreach (NavMeshAgent agent in slowTargets)
                        if (agent == obj.gameObject.GetComponent<NavMeshAgent>())
                            skip = false;

                    if (skip)
                        slowTargets.Add(obj.gameObject.GetComponent<NavMeshAgent>());
                }
            }
        }
    }

    void RemoveTarget(GameObject obj)
    {
        victims.Remove(obj.transform);
    }

    void Update()
    {
        if (!aoeActive)
        {
            if (GameManager.GameEnded) Destroy(gameObject);
            // <BUGFIX: Dev Team #21>
            else if (projectileTimer <= 0.0f) Destroy(gameObject);
            else if (fired) projectileTimer -= Time.deltaTime;
        }
    }

    float projectileTimer;
    public float projectileLifetime = 4.0f;
    void Start() { projectileTimer = projectileLifetime; }
    // </BUGFIX: Dev Team #21>
}
