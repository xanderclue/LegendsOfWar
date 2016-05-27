using UnityEngine;
public class TowerMovement : MonoBehaviour
{
	void Update()
	{
		if ( EnemyAIManager.towerMovement )
			gameObject.transform.Translate( -Time.fixedDeltaTime, 0.0f, 0.0f );
	}
}