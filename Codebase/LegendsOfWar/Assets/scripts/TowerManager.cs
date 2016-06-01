using UnityEngine;
public class TowerManager : MonoBehaviour
{
	public GameObject normalShotPrefab = null, freezeShotPrefab = null, explosiveShotPrefab = null;
	public NormalProjectileInfo normalInfo = null;
	public FreezeProjectileInfo freezeInfo = null;
	public ExplosiveProjectileInfo explosiveInfo = null;
	[SerializeField]
	private bool redNormalActive = true, blueNormalActive = true, redFreezeActive = false,
		blueFreezeActive = false, redExplosiveActive = false, blueExplosiveActive = false;
	private static TowerManager instance = null;
	private float blueTimer = 0.1f, redTimer = 0.1f;
	private bool blueShotChanged = false, redShotChanged = false, tmpBoolBlue;
	public static TowerManager Instance
	{
		get
		{
			return instance ?? ( instance = FindObjectOfType<TowerManager>() ?? new GameObject(
				"TowerManager" ).AddComponent<TowerManager>() );
		}
	}
	public static bool BlueShotChanged
	{ get { return instance ? instance.blueShotChanged : false; } }
	public static bool RedShotChanged
	{ get { return instance ? instance.redShotChanged : false; } }
	public Items GetActiveShot( Team team )
	{
		switch ( team )
		{
			case Team.RED_TEAM:
				if ( redNormalActive )
					return Items.NormalShot;
				else if ( redFreezeActive )
					return Items.FreezeShot;
				else if ( redExplosiveActive )
					return Items.ExplosiveShot;
				break;
			case Team.BLUE_TEAM:
				if ( blueNormalActive )
					return Items.NormalShot;
				else if ( blueFreezeActive )
					return Items.FreezeShot;
				else if ( blueExplosiveActive )
					return Items.ExplosiveShot;
				break;
			default:
				break;
		}
		return Items.Caster;
	}
	public bool CheckIfShotActive( Team team, Items shotType )
	{
		switch ( shotType )
		{
			case Items.NormalShot:
				if ( Team.BLUE_TEAM == team )
					return blueNormalActive;
				else
					return redNormalActive;
			case Items.FreezeShot:
				if ( Team.BLUE_TEAM == team )
					return blueFreezeActive;
				else
					return redFreezeActive;
			case Items.ExplosiveShot:
				if ( Team.BLUE_TEAM == team )
					return blueExplosiveActive;
				else
					return redExplosiveActive;
			default:
				return false;
		}
	}
	public void ActivateShotType( Team team, Items shotType )
	{
		tmpBoolBlue = Team.BLUE_TEAM == team;
		if ( Items.NormalShot == shotType || Items.FreezeShot == shotType || Items.ExplosiveShot ==
			shotType )
		{
			if ( tmpBoolBlue )
				blueNormalActive = blueFreezeActive = blueExplosiveActive = false;
			else
				redNormalActive = redFreezeActive = redExplosiveActive = false;
			switch ( shotType )
			{
				case Items.NormalShot:
					if ( tmpBoolBlue )
						blueNormalActive = true;
					else
						redNormalActive = true;
					break;
				case Items.FreezeShot:
					if ( tmpBoolBlue )
						blueFreezeActive = true;
					else
						redFreezeActive = true;
					break;
				case Items.ExplosiveShot:
					if ( tmpBoolBlue )
						blueExplosiveActive = true;
					else
						redExplosiveActive = true;
					break;
				default:
					break;
			}
			if ( tmpBoolBlue )
				blueShotChanged = true;
			else
				redShotChanged = true;
		}
	}
	private void Awake()
	{
		instance = this;
	}
	private void Update()
	{
		if ( blueShotChanged )
			if ( 0.0f <= blueTimer )
				blueTimer -= Time.deltaTime;
			else
			{
				blueShotChanged = false;
				blueTimer = 0.1f;
			}
		if ( redShotChanged )
			if ( 0.0f <= redTimer )
				redTimer -= Time.deltaTime;
			else
			{
				redShotChanged = false;
				redTimer = 0.1f;
			}
	}
	private void OnDestroy()
	{
		instance = null;
	}
}