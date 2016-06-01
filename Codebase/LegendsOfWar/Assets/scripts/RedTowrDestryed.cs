using UnityEngine;
public class RedTowrDestryed : MonoBehaviour
{
	[SerializeField]
	private EnemyAIManager aiManager = null;
	private void Start()
	{
		aiManager = FindObjectOfType<EnemyAIManager>();
	}
	private void OnDisable()
	{
		if ( aiManager )
			aiManager.Destroyed();
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class RedTowrDestryed : MonoBehaviour {
	[SerializeField]
	EnemyAIManager aiManager = null;
	void Start(){
		aiManager = GameObject.FindObjectOfType<EnemyAIManager>();
	}


	void OnDisable(){
		if(aiManager!= null)
			aiManager.Destroyed();
	}
}

#endif
#endregion //OLD_CODE