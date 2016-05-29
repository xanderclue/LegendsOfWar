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
	private bool blueShotChanged = false, redShotChanged = false;
	public static TowerManager Instance
	{
		get
		{
			if ( !instance )
			{
				instance = FindObjectOfType<TowerManager>();
				if ( !instance )
					instance = new GameObject( "TowerManager" ).AddComponent<TowerManager>();
			}
			return instance;
		}
	}
	public bool BlueShotChanged
	{ get { return blueShotChanged; } }
	public bool RedShotChanged
	{ get { return redShotChanged; } }
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
		if ( Items.NormalShot == shotType || Items.FreezeShot == shotType || Items.ExplosiveShot ==
			shotType )
		{
			DeactivateShots( team );
			switch ( shotType )
			{
				case Items.NormalShot:
					if ( Team.BLUE_TEAM == team )
						blueNormalActive = true;
					else
						redNormalActive = true;
					break;
				case Items.FreezeShot:
					if ( Team.BLUE_TEAM == team )
						blueFreezeActive = true;
					else
						redFreezeActive = true;
					break;
				case Items.ExplosiveShot:
					if ( Team.BLUE_TEAM == team )
						blueExplosiveActive = true;
					else
						redExplosiveActive = true;
					break;
				default:
					break;
			}
			if ( Team.BLUE_TEAM == team )
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
		if ( blueShotChanged && blueTimer < 0.0f )
		{
			blueShotChanged = false;
			blueTimer = 0.1f;
		}
		else if ( blueShotChanged )
			blueTimer -= Time.deltaTime;
		if ( redShotChanged && redTimer < 0.0f )
		{
			redShotChanged = false;
			redTimer = 0.1f;
		}
		else if ( redShotChanged )
			redTimer -= Time.deltaTime;
	}
	private void OnDestroy()
	{
		instance = null;
	}
	private void DeactivateShots( Team team )
	{
		if ( Team.BLUE_TEAM == team )
			blueNormalActive = blueFreezeActive = blueExplosiveActive = false;
		else
			redNormalActive = redFreezeActive = redExplosiveActive = false;
	}
}