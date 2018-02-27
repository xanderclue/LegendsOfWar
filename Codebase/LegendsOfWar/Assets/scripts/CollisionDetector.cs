using UnityEngine;
using System.Collections.Generic;
public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    private List<Collider> targetedEnemies = null;
    public TankAbilityW w = null;
    private HeroInfo heroInfo;
    public void DealDamage(System.Action<Info> action)
    {
        foreach (Collider _target in targetedEnemies)
            if (_target)
                action(_target.GetComponent<Info>());
    }
    private void Awake()
    {
        targetedEnemies = new List<Collider>();
    }
    private void Start()
    {
        heroInfo = GetComponentInParent<HeroInfo>();
    }
    private void OnTriggerEnter(Collider _target)
    {
        if (_target.GetComponent<Info>())
            if (_target.GetComponent<Info>().team != heroInfo.team)
                targetedEnemies.Add(_target);
    }
    private void OnTriggerExit(Collider _target)
    {
        if (_target.GetComponent<Info>())
            targetedEnemies.Remove(_target);
    }
}