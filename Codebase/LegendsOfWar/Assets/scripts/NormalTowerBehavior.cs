using UnityEngine;
using System.Collections.Generic;
public class NormalTowerBehavior : MonoBehaviour
{
    [SerializeField]
    private Team team = Team.BLUE_TEAM;
    [SerializeField]
    private Transform projectileSpawnPoint = null;
    [SerializeField]
    private Detector detector = null;
    private List<Transform> targets;
    private NormalProjectileInfo info;
    private Info targ;
    private float fireTimer;
    private void Awake()
    {
        targets = new List<Transform>();
        info = TowerManager.Instance.normalInfo;
        detector.CreateTrigger(info.AgroRange);
        detector.triggerEnter += AddTarget;
        detector.triggerExit += RemoveTarget;
    }
    private void Update()
    {
        targets.RemoveAll(item => !item);
        if (TowerManager.Instance.CheckIfShotActive(team, Items.NormalShot))
            if (0.0f < fireTimer)
                fireTimer -= Time.deltaTime;
            else if (0 < targets.Count)
                if (targets[0].gameObject.activeInHierarchy)
                    FireAtTarget();
                else
                    RemoveTarget(targets[0].gameObject);
    }
    private void AddTarget(GameObject obj)
    {
        if (obj)
        {
            targ = obj.GetComponent<Info>();
            if (targ && targ.team != team)
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
        NormalProjectileBehavior p = (Instantiate(TowerManager.Instance.normalShotPrefab,
            projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject).
            GetComponent<NormalProjectileBehavior>();
        p.target = targets[0];
        p.Fire();
        fireTimer = info.AttackSpeed;
    }
}