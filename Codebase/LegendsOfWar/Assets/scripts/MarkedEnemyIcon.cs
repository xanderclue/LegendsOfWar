using UnityEngine;
public class MarkedEnemyIcon : MonoBehaviour
{
	[SerializeField]
	private string m_name;
	[SerializeField]
	private bool AutoMaticDestroy = true;
	private void Start()
	{
		if ( "AbilityW" == transform.parent.name )
			if ( null != GetComponentInParent<AbilityBase>().Effect )
				m_name = GetComponentInParent<AbilityBase>().Effect.m_name;
	}
	private void Update()
	{
		if ( "AbilityW" != transform.parent.name )
		{
			transform.LookAt( 2.0f * transform.position - HeroUIScript.Instance.transform.position,
				HeroUIScript.Instance.transform.up );
			transform.localPosition = transform.localPosition * 0.6f;
			transform.position = Vector3.MoveTowards( transform.position, 3.0f * transform.up +
				transform.position, 25.0f );
			if ( !FindObjectOfType<StatusEffectsManager>().CheckSkill( transform.parent.gameObject.
				GetInstanceID().ToString(), m_name ) && AutoMaticDestroy )
				Destroy( this.gameObject );
		}
	}
}