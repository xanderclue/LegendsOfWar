using UnityEngine;
using System.Collections.Generic;
public class FreezeTowerBehavior : MonoBehaviour
{
    [SerializeField]
    private Team team = Team.BLUE_TEAM;
    [SerializeField]
    private Transform projectileSpawnPoint = null;
    [SerializeField]
    private Detector detector = null;
    private List<Transform> targets;
    private Info targ;
    private FreezeProjectileInfo info;
    private float fireTimer;
    private void Awake()
    {
        targets = new List<Transform>();
        info = TowerManager.Instance.freezeInfo;
        detector.CreateTrigger(info.AgroRange);
        detector.triggerEnter += AddTarget;
        detector.triggerExit += RemoveTarget;
    }
    private void Update()
    {
        targets.RemoveAll(item => !item);
        if (TowerManager.Instance.CheckIfShotActive(team, Items.FreezeShot) && fireTimer <= 0.0f
            && 0 < targets.Count)
            if (!targets[0].gameObject.activeInHierarchy)
                RemoveTarget(targets[0].gameObject);
            else
            {
                FireAtTarget();
                fireTimer = info.AttackSpeed;
            }
        else
            fireTimer -= Time.deltaTime;
    }
    private void AddTarget(GameObject obj)
    {
        if (obj)
        {
            targ = obj.GetComponent<Info>();
            if (targ)
                if (targ.team != team)
                    targets.Add(obj.transform);
        }
    }
    private void RemoveTarget(GameObject obj)
    {
        targets.Remove(obj.transform);
    }
    private void FireAtTarget()
    {
        if (GameManager.GameEnded)
            return;
        projectileSpawnPoint.LookAt(targets[0]);
        FreezeProjectileBehavior p = (Instantiate(TowerManager.Instance.freezeShotPrefab,
            projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject).
            GetComponent<FreezeProjectileBehavior>();
        p.target = targets[0];
        p.Fire(team);
    }
}