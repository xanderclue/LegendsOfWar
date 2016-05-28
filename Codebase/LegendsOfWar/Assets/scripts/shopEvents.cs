using UnityEngine;
public class shopEvents : MonoBehaviour
{
	[SerializeField]
	private Team team = Team.BLUE_TEAM;
	[SerializeField]
	private GameObject laneSelectPanel = null;

	private string selectedItem;
	private void Awake()
	{
		ClosePanel();
		ShopManager.Instance.UpdateValues = true;
	}
	public void OpenPanel( string item )
	{
		laneSelect = true;
		selectedItem = item;
		laneSelectPanel.SetActive( true );
	}
	public void ClosePanel()
	{
		laneSelect = false;
		selectedItem = null;
		laneSelectPanel.SetActive( false );
	}
	public void BuyMinion( int lane )
	{
		switch ( selectedItem )
		{
			case "Striker":
				ShopManager.Instance.PurchaseStriker( team, lane );
				break;
			case "Tank":
				ShopManager.Instance.PurchaseTank( team, lane );
				break;
			case "Caster":
				ShopManager.Instance.PurchaseCaster( team, lane );
				break;
			default:
				return;
		}
	}
	public void PurchaseFreezeShot()
	{
		ShopManager.Instance.PurchaseFreezingShot( team );
	}
	public void PurchaseExplosiveShot()
	{
		ShopManager.Instance.PurchaseExplosiveShot( team );
	}
	public void PurchaseRevive()
	{
		ShopManager.Instance.PurchaseInstaRevive( team );
	}
	public void PurchaseStrikerUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.SLvl );
	}
	public void PurchaseTankUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.TLvl );
	}
	public void PurchaseCasterUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.CLvl );
	}
	public void PurchaseQUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.QLvl );
	}
	public void PurchaseWUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.WLvl );
	}
	public void PurchaseEUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.ELvl );
	}
	public void PurchaseRUpgrade()
	{
		ShopManager.Instance.PurchaseUpgrade( team, Items.RLvl );
	}
	private bool upgrade;
	private bool laneSelect;
	private float updateTimer, timer;
	private void Start()
	{
		upgrade = false;
		updateTimer = timer = 3.0f;
	}
	private void Update()
	{
		if ( upgrade )
		{
			if ( timer > 0.0f )
				timer -= Time.deltaTime;
			else
			{
				timer = updateTimer;
				upgrade = false;
			}
		}
		if ( laneSelect )
			RecieveLaneInputs();
	}
	private void RecieveLaneInputs()
	{
		if ( Input.GetKeyDown( KeyCode.UpArrow ) )
			BuyMinion( 1 );
		else if ( Input.GetKeyDown( KeyCode.RightArrow ) )
			BuyMinion( 2 );
		else if ( Input.GetKeyDown( KeyCode.DownArrow ) )
			BuyMinion( 3 );
		else if ( Input.GetKeyDown( KeyCode.LeftArrow ) || Input.GetKeyDown( KeyCode.Escape ) )
			ClosePanel();
	}
}