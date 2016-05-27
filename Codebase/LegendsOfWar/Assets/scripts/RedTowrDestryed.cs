using UnityEngine;
public class RedTowrDestryed : MonoBehaviour
{
	[SerializeField]
	EnemyAIManager aiManager = null;
	void Start()
	{
		aiManager = FindObjectOfType<EnemyAIManager>();
	}
	void OnDisable()
	{
		if ( aiManager )
			aiManager.Destroyed();
	}
}