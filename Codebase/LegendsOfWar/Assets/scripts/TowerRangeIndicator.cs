using UnityEngine;
public class TowerRangeIndicator : MonoBehaviour
{
	[SerializeField]
	Transform circle = null;
	[SerializeField]
	Team team = Team.RED_TEAM;
	void Awake()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
		UpdateRange();
	}
	void Update()
	{
		circle.Rotate( 0.0f, Time.deltaTime * 1.5f, 0.0f );
		if ( TowerManager.Instance.BlueShotChanged && team == Team.BLUE_TEAM )
			UpdateRange();
		else if ( TowerManager.Instance.RedShotChanged && team == Team.RED_TEAM )
			UpdateRange();
	}
	void UpdateRange()
	{
		switch ( TowerManager.Instance.GetActiveShot( team ) )
		{
			case Items.NormalShot:
				circle.localScale = new Vector3( TowerManager.Instance.normalInfo.AgroRange * 0.27f,
					0.0f, TowerManager.Instance.normalInfo.AgroRange * 0.27f );
				break;
			case Items.FreezeShot:
				circle.localScale = new Vector3( TowerManager.Instance.freezeInfo.AgroRange * 0.27f,
					0.0f, TowerManager.Instance.freezeInfo.AgroRange * 0.27f );
				break;
			case Items.ExplosiveShot:
				circle.localScale = new Vector3( TowerManager.Instance.explosiveInfo.AgroRange *
					0.27f, 0.0f, TowerManager.Instance.explosiveInfo.AgroRange * 0.27f );
				break;
			default:
				break;
		}
	}
}