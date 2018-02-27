using UnityEngine;
public enum Team { RED_TEAM, BLUE_TEAM }
public class Info : MonoBehaviour
{
    [SerializeField]
    private float MaxHP;
    public Team team = Team.BLUE_TEAM;
    [SerializeField]
    protected float attackSpeed, agroRange, attackRange, damage;
    public delegate void HpChangedEvent();
    public event HpChangedEvent Attacked, Destroyed;
    protected float invMAXHP;
    private float dmgDamp = 0.0f, currHP;
    private bool destroyable = true, isAlive = false;
    public float DmgDamp
    { set { dmgDamp = value; } }
    public bool Alive
    {
        get { return isAlive; }
        protected set
        {
            if (value && !isAlive)
            {
                currHP = MaxHP;
                isAlive = true;
                gameObject.SetActive(true);
            }
            else if (isAlive && !value)
                TakeDamage(currHP + 1.0f);
        }
    }
    public float HP
    {
        get { return currHP; }
        set
        {
            if (value < currHP)
                TakeDamage(currHP - value);
            else
                currHP = Mathf.Min(value, MaxHP);
        }
    }
    public float MAXHP
    {
        get { return MaxHP; }
        set
        {
            MaxHP = value;
            invMAXHP = 1.0f / value;
        }
    }
    public float InvMAXHP
    { get { return invMAXHP; } }
    public virtual void TakeDamage(float damage)
    {
        if (!isAlive || damage <= 0.0f)
            return;
        if (SupportRange.InSupportRange(gameObject))
            damage *= 0.75f;
        HeroUIScript.Damage(damage * (1.0f - dmgDamp * 0.01f), transform.position + Vector3.up *
            10.0f);
        currHP -= damage * (1.0f - dmgDamp * 0.01f);
        Attacked?.Invoke();
        if (currHP <= 0.0f)
        {
            currHP = 0.0f;
            isAlive = false;
            if (!(this is PortalInfo))
            {
                gameObject.SetActive(false);
                if (destroyable)
                    Destroy(gameObject, 1.0f);
            }
            Destroyed?.Invoke();
        }
    }
    protected virtual void Start()
    {
        currHP = MaxHP;
        invMAXHP = 1.0f / MaxHP;
        isAlive = true;
        if (this is HeroInfo)
            destroyable = false;
    }
}