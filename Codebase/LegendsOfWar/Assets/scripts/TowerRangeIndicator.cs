using UnityEngine;
public class TowerRangeIndicator : MonoBehaviour
{
	[SerializeField]
	private Transform circle = null;
	[SerializeField]
	private Team team = Team.RED_TEAM;
	private void Awake()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
		UpdateRange();
	}
	private void Update()
	{
		circle.Rotate( 0.0f, Time.deltaTime * 1.5f, 0.0f );
		if ( ( TowerManager.Instance.BlueShotChanged && Team.BLUE_TEAM == team ) || ( TowerManager.
			Instance.RedShotChanged && Team.RED_TEAM == team ) )
			UpdateRange();
	}
	private void UpdateRange()
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