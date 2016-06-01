using UnityEngine;
public class TowerMovement : MonoBehaviour
{
	private void Update()
	{
		if ( EnemyAIManager.towerMovement )
			gameObject.transform.Translate( -Time.fixedDeltaTime, 0.0f, 0.0f );
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class TowerMovement : MonoBehaviour {

	void Update(){
		if (EnemyAIManager.towerMovement)
			gameObject.transform.Translate(-Time.fixedDeltaTime, 0.0f, 0.0f);
	}
}

#endif
#endregion //OLD_CODE