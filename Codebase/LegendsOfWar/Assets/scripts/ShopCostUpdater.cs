using UnityEngine;
using UnityEngine.UI;
public class ShopCostUpdater : MonoBehaviour
{
	[SerializeField]
	private Text freezeShotCost = null, explosiveShotCost = null, strikerCost = null, tankCost =
		null, casterCost = null, reviveCost = null, strikerUpgradeCost = null, tankUpgradeCost =
		null, casterUpgradeCost = null;
	[SerializeField]
	private GameObject strikerButton = null, tankButton = null, casterButton = null;
	private static readonly Color bronze = new Color( 0.424f, 0.329f, 0.118f ), silver = new Color(
		0.753f, 0.753f, 0.753f ), gold = new Color( 1.0f, 0.843f, 0.0f );
	private void Start()
	{
		SetValues();
	}
	private void Update()
	{
		if ( ShopManager.Instance.UpdateValues )
		{
			UpdateColors();
			UpdatedValues();
			ShopManager.Instance.UpdateValues = false;
		}
	}
	private void SetValues()
	{
		freezeShotCost.text = ShopManager.Instance.FreezingShotCost.ToString();
		explosiveShotCost.text = ShopManager.Instance.ExplosiveShotCost.ToString();
		strikerCost.text = ShopManager.Instance.StrikerPurchaseCost.ToString();
		tankCost.text = ShopManager.Instance.TankPurchaseCost.ToString();
		casterCost.text = ShopManager.Instance.CasterPurchaseCost.ToString();
		reviveCost.text = ShopManager.Instance.InstaReviveCost.ToString();
		strikerUpgradeCost.text = tankUpgradeCost.text = casterUpgradeCost.text = ShopManager.
			Instance.MinionUG1Cost.ToString();
	}
	private void UpdatedValues()
	{
		strikerUpgradeCost.text = ShopManager.Instance.minionUGPrices[ ShopManager.Instance.
			Purchases[ ( int )Items.SLvl ] ].ToString();
		tankUpgradeCost.text = ShopManager.Instance.minionUGPrices[ ShopManager.Instance.Purchases[
			( int )Items.TLvl ] ].ToString();
		casterUpgradeCost.text = ShopManager.Instance.minionUGPrices[ ShopManager.Instance.Purchases
			[ ( int )Items.CLvl ] ].ToString();
		if ( 3 == ShopManager.Instance.Purchases[ ( int )Items.SLvl ] )
			strikerUpgradeCost.text = "MAX";
		if ( 3 == ShopManager.Instance.Purchases[ ( int )Items.TLvl ] )
			tankUpgradeCost.text = "MAX";
		if ( 3 == ShopManager.Instance.Purchases[ ( int )Items.CLvl ] )
			casterUpgradeCost.text = "MAX";
	}
	private void UpdateColors()
	{
		switch ( ShopManager.Instance.Purchases[ ( int )Items.SLvl ] )
		{
			case 1:
				strikerButton.GetComponent<Image>().color = bronze;
				break;
			case 2:
				strikerButton.GetComponent<Image>().color = silver;
				break;
			case 3:
				strikerButton.GetComponent<Image>().color = gold;
				break;
			default:
				break;
		}
		switch ( ShopManager.Instance.Purchases[ ( int )Items.TLvl ] )
		{
			case 1:
				tankButton.GetComponent<Image>().color = bronze;
				break;
			case 2:
				tankButton.GetComponent<Image>().color = silver;
				break;
			case 3:
				tankButton.GetComponent<Image>().color = gold;
				break;
			default:
				break;
		}
		switch ( ShopManager.Instance.Purchases[ ( int )Items.CLvl ] )
		{
			case 1:
				casterButton.GetComponent<Image>().color = bronze;
				break;
			case 2:
				casterButton.GetComponent<Image>().color = silver;
				break;
			case 3:
				casterButton.GetComponent<Image>().color = gold;
				break;
			default:
				break;
		}
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using UnityEngine.UI;

public class ShopCostUpdater : MonoBehaviour
{
    [SerializeField]
    Text freezeShotCost = null, explosiveShotCost = null, strikerCost = null,
         tankCost = null, casterCost = null, reviveCost = null,
        strikerUpgradeCost = null, tankUpgradeCost = null, casterUpgradeCost = null;
    [SerializeField]
    GameObject strikerButton = null, tankButton = null, casterButton = null;

    Color bronze = new Color(0.424f, 0.329f, 0.118f);
    Color silver = new Color(0.753f, 0.753f, 0.753f);
    Color gold = new Color(1.0f, 0.843f, 0.0f);

    void Start()
    {
        SetValues();
    }

    void Update()
    {
        if (ShopManager.Instance.UpdateValues)
        {
            UpdateColors();
            UpdatedValues();           
            ShopManager.Instance.UpdateValues = false;
        }
    }

    void SetValues()
    {
        freezeShotCost.text = ShopManager.Instance.FreezingShotCost.ToString();
        explosiveShotCost.text = ShopManager.Instance.ExplosiveShotCost.ToString();
        strikerCost.text = ShopManager.Instance.StrikerPurchaseCost.ToString();
        tankCost.text = ShopManager.Instance.TankPurchaseCost.ToString();
        casterCost.text = ShopManager.Instance.CasterPurchaseCost.ToString();
        reviveCost.text = ShopManager.Instance.InstaReviveCost.ToString();
        strikerUpgradeCost.text = tankUpgradeCost.text = casterUpgradeCost.text = ShopManager.Instance.MinionUG1Cost.ToString();
    }

    void UpdatedValues()
    {
        strikerUpgradeCost.text = ShopManager.Instance.minionUGPrices[ShopManager.Instance.Purchases[(int)Items.SLvl]].ToString();
        tankUpgradeCost.text = ShopManager.Instance.minionUGPrices[ShopManager.Instance.Purchases[(int)Items.TLvl]].ToString();
        casterUpgradeCost.text = ShopManager.Instance.minionUGPrices[ShopManager.Instance.Purchases[(int)Items.CLvl]].ToString();

        if (ShopManager.Instance.Purchases[(int)Items.SLvl] == 3)
            strikerUpgradeCost.text = "MAX";
        if (ShopManager.Instance.Purchases[(int)Items.TLvl] == 3)
            tankUpgradeCost.text = "MAX";
        if (ShopManager.Instance.Purchases[(int)Items.CLvl] == 3)
            casterUpgradeCost.text = "MAX";
    }

    void UpdateColors()
    {
        switch (ShopManager.Instance.Purchases[(int)Items.SLvl])
        {
            case 0:
                break;
            case 1:
                strikerButton.GetComponent<Image>().color = bronze;
                break;
            case 2:
                strikerButton.GetComponent<Image>().color = silver;
                break;
            case 3:
                strikerButton.GetComponent<Image>().color = gold;
                break;
        }

        switch (ShopManager.Instance.Purchases[(int)Items.TLvl])
        {
            case 0:
                break;
            case 1:
                tankButton.GetComponent<Image>().color = bronze;
                break;
            case 2:
                tankButton.GetComponent<Image>().color = silver;
                break;
            case 3:
                tankButton.GetComponent<Image>().color = gold;
                break;
        }

        switch (ShopManager.Instance.Purchases[(int)Items.CLvl])
        {
            case 0:
                break;
            case 1:
                casterButton.GetComponent<Image>().color = bronze;
                break;
            case 2:
                casterButton.GetComponent<Image>().color = silver;
                break;
            case 3:
                casterButton.GetComponent<Image>().color = gold;
                break;
        }
    }
}

#endif
#endregion //OLD_CODE