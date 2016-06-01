using UnityEngine;
public class MarkedEnemyIcon : MonoBehaviour
{
	[SerializeField]
	private string m_name;
	[SerializeField]
	private bool AutoMaticDestroy = true;
	private bool updateScript;
	private void Start()
	{
		updateScript = "AbilityW" != transform.parent.name;
	}
	private void Update()
	{
		if ( updateScript )
		{
			transform.LookAt( 2.0f * transform.position - HeroUIScript.Instance.transform.position,
				HeroUIScript.Instance.transform.up );
			transform.localPosition = transform.localPosition * 0.6f;
			transform.position = Vector3.MoveTowards( transform.position, 3.0f * transform.up +
				transform.position, 25.0f );
			if ( AutoMaticDestroy && !StatusEffectsManager.Instance.CheckSkill( transform.parent.
				gameObject.GetInstanceID().ToString(), m_name ) )
				Destroy( gameObject );
		}
	}
}