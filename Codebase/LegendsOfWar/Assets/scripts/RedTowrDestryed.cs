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