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
	private bool blueShotChanged = false;
	private bool redShotChanged = false;
	private float blueTimer = 0.1f, redTimer = 0.1f;
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
				if ( team == Team.BLUE_TEAM )
					return blueNormalActive;
				else
					return redNormalActive;
			case Items.FreezeShot:
				if ( team == Team.BLUE_TEAM )
					return blueFreezeActive;
				else
					return redFreezeActive;
			case Items.ExplosiveShot:
				if ( team == Team.BLUE_TEAM )
					return blueExplosiveActive;
				else
					return redExplosiveActive;
			default:
				return false;
		}
	}
	public void ActivateShotType( Team team, Items shotType )
	{
		if ( shotType == Items.NormalShot || shotType == Items.FreezeShot || shotType == Items.
			ExplosiveShot )
		{
			DeactivateShots( team );
			switch ( shotType )
			{
				case Items.NormalShot:
					if ( team == Team.BLUE_TEAM )
						blueNormalActive = true;
					else
						redNormalActive = true;
					break;
				case Items.FreezeShot:
					if ( team == Team.BLUE_TEAM )
						blueFreezeActive = true;
					else
						redFreezeActive = true;
					break;
				case Items.ExplosiveShot:
					if ( team == Team.BLUE_TEAM )
						blueExplosiveActive = true;
					else
						redExplosiveActive = true;
					break;
				default:
					break;
			}
			if ( team == Team.BLUE_TEAM )
				blueShotChanged = true;
			else
				redShotChanged = true;
		}
	}
	private void DeactivateShots( Team team )
	{
		if ( team == Team.BLUE_TEAM )
			blueNormalActive = blueFreezeActive = blueExplosiveActive = false;
		else
			redNormalActive = redFreezeActive = redExplosiveActive = false;
	}
	private void Awake()
	{
		instance = this;
	}
	private void OnDestroy()
	{
		instance = null;
	}
}