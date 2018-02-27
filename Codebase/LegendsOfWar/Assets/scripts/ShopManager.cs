using UnityEngine;
public enum Items
{
    Striker, Tank, Caster, Revive, SLvl, TLvl, CLvl, QLvl, WLvl, ELvl, RLvl, FreezeShot,
    ExplosiveShot, NormalShot
}
public class ShopManager : MonoBehaviour
{
    public int StrikerPurchaseCost = 20, TankPurchaseCost = 30, CasterPurchaseCost = 40,
        MinionUG1Cost = 20, MinionUG2Cost = 30, MinionUG3Cost = 40, AbilityUG1Cost = 10,
        AbilityUG2Cost = 20, AbilityUG3Cost = 30, InstaReviveCost = 50, FreezingShotCost = 50,
        ExplosiveShotCost = 60;
    public float strikerHpUpgrade = 10.0f, strikerDamageUpgrade = 5.0f, strikerAttackspeedUpgrade =
        10.0f, tankHpUpgrade = 15.0f, tankDamageUpgrade = 5.0f, tankAttackspeedUpgrade = 5.0f,
        casterHpUpgrade = 5.0f, casterDamageUpgrade = 10.0f, casterAttackspeedUpgrade = 5.0f,
        casterRangeUpgrade = 5.0f;
    private static ShopManager instance = null;
    private int[] purchases = new int[13], MinionUGPrices = new int[4], AbilityUGPrices = new
        int[3];
    private bool updateValues = false;
    public static ShopManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<ShopManager>() ?? new GameObject("ShopManager").
                    AddComponent<ShopManager>();
            return instance;
        }
    }
    public int[] Purchases
    { get { return purchases; } }
    public int[] minionUGPrices
    { get { return MinionUGPrices; } }
    public bool UpdateValues
    {
        get { return updateValues; }
        set { updateValues = value; }
    }
    public void PurchaseFreezingShot(Team team)
    {
        if (EconomyManager.Instance.TakeGold(team, FreezingShotCost))
        {
            TowerManager.Instance.ActivateShotType(team, Items.FreezeShot);
            ++purchases[(int)Items.FreezeShot];
        }
    }
    public void PurchaseExplosiveShot(Team team)
    {
        if (EconomyManager.Instance.TakeGold(team, ExplosiveShotCost))
        {
            TowerManager.Instance.ActivateShotType(team, Items.ExplosiveShot);
            ++purchases[(int)Items.ExplosiveShot];
        }
    }
    public void PurchaseStriker(Team team, int lane)
    {
        if (EconomyManager.Instance.TakeGold(team, StrikerPurchaseCost))
        {
            GameManager.Instance.SpawnStrikerMinion(team, lane);
            ++purchases[(int)Items.Striker];
        }
    }
    public void PurchaseTank(Team team, int lane)
    {
        if (EconomyManager.Instance.TakeGold(team, TankPurchaseCost))
        {
            GameManager.Instance.SpawnTankMinion(team, lane);
            ++purchases[(int)Items.Tank];
        }
    }
    public void PurchaseCaster(Team team, int lane)
    {
        if (EconomyManager.Instance.TakeGold(team, CasterPurchaseCost))
        {
            GameManager.Instance.SpawnCasterMinion(team, lane);
            ++purchases[(int)Items.Caster];
        }
    }
    public void PurchaseInstaRevive(Team team)
    {
        foreach (HeroInfo hero in GameManager.Instance.Heros)
            if (!hero.Alive && hero.team == team && EconomyManager.Instance.TakeGold(team,
                InstaReviveCost))
            {
                ++purchases[(int)Items.Revive];
                GameManager.Instance.InstaRespawn(team, hero);
                break;
            }
    }
    public void PurchaseUpgrade(Team team, Items item)
    {
        switch (item)
        {
            case Items.SLvl:
                if (purchases[(int)Items.SLvl] < 3 && EconomyManager.Instance.TakeGold(team,
                    MinionUGPrices[purchases[(int)Items.SLvl]]))
                {
                    GameManager.Instance.UpgradeStrikerMinions(team);
                    ++purchases[(int)Items.SLvl];
                }
                break;
            case Items.TLvl:
                if (purchases[(int)Items.TLvl] < 3 && EconomyManager.Instance.TakeGold(team,
                    MinionUGPrices[purchases[(int)Items.TLvl]]))
                {
                    GameManager.Instance.UpgradeTankMinions(team);
                    ++purchases[(int)Items.TLvl];
                }
                break;
            case Items.CLvl:
                if (purchases[(int)Items.CLvl] < 3 && EconomyManager.Instance.TakeGold(team,
                    MinionUGPrices[purchases[(int)Items.CLvl]]))
                {
                    GameManager.Instance.UpgradeCasterMinions(team);
                    ++purchases[(int)Items.CLvl];
                }
                break;
            case Items.QLvl:
                if (purchases[(int)Items.QLvl] < 3)
                    EconomyManager.Instance.TakeGold(team, AbilityUGPrices[purchases[(int)Items
                        .QLvl]]);
                break;
            case Items.WLvl:
                if (purchases[(int)Items.WLvl] < 3)
                    EconomyManager.Instance.TakeGold(team, AbilityUGPrices[purchases[(int)Items
                        .WLvl]]);
                break;
            case Items.ELvl:
                if (purchases[(int)Items.ELvl] < 3)
                    EconomyManager.Instance.TakeGold(team, AbilityUGPrices[purchases[(int)Items
                        .ELvl]]);
                break;
            case Items.RLvl:
                if (purchases[(int)Items.RLvl] < 3)
                    EconomyManager.Instance.TakeGold(team, AbilityUGPrices[purchases[(int)Items
                        .RLvl]]);
                break;
            default:
                break;
        }
        UpdateValues = true;
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        MinionUGPrices[0] = MinionUG1Cost;
        MinionUGPrices[1] = MinionUG2Cost;
        MinionUGPrices[2] = MinionUG3Cost;
        MinionUGPrices[3] = MinionUG3Cost;
        AbilityUGPrices[0] = AbilityUG1Cost;
        AbilityUGPrices[1] = AbilityUG2Cost;
        AbilityUGPrices[2] = AbilityUG3Cost;
    }
    private void OnDestroy()
    {
        instance = null;
    }
}