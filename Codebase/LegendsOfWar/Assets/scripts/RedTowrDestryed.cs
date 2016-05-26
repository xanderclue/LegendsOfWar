using UnityEngine;
using System.Collections;

public class RedTowrDestryed : MonoBehaviour
{
	[SerializeField]
	EnemyAIManager aiManager = null;
	void Start()
	{
		aiManager = GameObject.FindObjectOfType<EnemyAIManager>();
	}


	void OnDisable()
	{
		if ( aiManager != null )
			aiManager.Destroyed();
	}
}
