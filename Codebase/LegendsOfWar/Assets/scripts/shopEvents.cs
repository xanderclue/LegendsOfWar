using UnityEngine;
using System.Collections;

public class shopEvents : MonoBehaviour
{
    [SerializeField]
    Team team = Team.BLUE_TEAM;
    [SerializeField]
    GameObject laneSelectPanel = null;

    string selectedItem;

    void Awake()
    {
        ClosePanel();
        ShopManager.Instance.UpdateValues = true;
    }

    public void OpenPanel(string item)
    {
        laneSelect = true;
        selectedItem = item;
        laneSelectPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        laneSelect = false;
        selectedItem = null;
        laneSelectPanel.SetActive(false);
    }

    public void BuyMinion(int lane)
    {
        switch (selectedItem)
        {
            case "Striker":
                ShopManager.Instance.PurchaseStriker(team, lane);
                break;
            case "Tank":
                ShopManager.Instance.PurchaseTank(team, lane);
                break;
            case "Caster":
                ShopManager.Instance.PurchaseCaster(team, lane);
                break;
            default:
                return;
        }
    }

    public void PurchaseFreezeShot()
    {
        ShopManager.Instance.PurchaseFreezingShot(team);
    }

    public void PurchaseExplosiveShot()
    {
        ShopManager.Instance.PurchaseExplosiveShot(team);
    }

    public void PurchaseRevive()
    {
        ShopManager.Instance.PurchaseInstaRevive(team);
    }

    public void PurchaseStrikerUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.SLvl);
    }

    public void PurchaseTankUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.TLvl);
    }

    public void PurchaseCasterUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.CLvl);
    }

    public void PurchaseQUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.QLvl);
    }

    public void PurchaseWUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.WLvl);
    }

    public void PurchaseEUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.ELvl);
    }

    public void PurchaseRUpgrade()
    {
        ShopManager.Instance.PurchaseUpgrade(team, Items.RLvl);
    }

    //shortcuts
    //currently defaulting to blue team
    bool upgrade;
    bool laneSelect;
    float updateTimer, timer;

    void Start()
    {
        upgrade = false;
        updateTimer = timer = 3.0f;
    }

    void Update()
    {
        //if (!upgrade && Input.anyKeyDown)
            //RecieveInputs();
        if (upgrade)
        {
            if (timer > 0.0f)
            {
                //if (Input.anyKeyDown)
                    //RecieveUpgradeInputs();
                timer -= Time.deltaTime;
            }
            else
            {
                timer = updateTimer;
                upgrade = false;
            }
        }
        if (laneSelect)
            RecieveLaneInputs();
    }

    //void RecieveInputs()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //        OpenPanel("Striker");
    //    else if (Input.GetKeyDown(KeyCode.X))
    //        OpenPanel("Tank");
    //    else if (Input.GetKeyDown(KeyCode.C))
    //        OpenPanel("Caster");
    //    else if (Input.GetKeyDown(KeyCode.A))
    //        PurchaseFreezeShot();
    //    else if (Input.GetKeyDown(KeyCode.S))
    //        PurchaseExplosiveShot();
    //    else if (Input.GetKeyDown(KeyCode.LeftShift))
    //        upgrade = true;
    //}

    //void RecieveUpgradeInputs()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.Striker);
    //    else if (Input.GetKeyDown(KeyCode.X))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.Tank);
    //    else if (Input.GetKeyDown(KeyCode.C))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.Caster);
    //    else if (Input.GetKeyDown(KeyCode.D))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.QLvl);
    //    else if (Input.GetKeyDown(KeyCode.F))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.WLvl);
    //    else if (Input.GetKeyDown(KeyCode.G))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.ELvl);
    //    else if (Input.GetKeyDown(KeyCode.H))
    //        ShopManager.Instance.PurchaseUpgrade(team, Items.RLvl);

    //    upgrade = false;
    //}

    void RecieveLaneInputs()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            BuyMinion(1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            BuyMinion(2);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            BuyMinion(3);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Escape))
            ClosePanel();
    }
}
