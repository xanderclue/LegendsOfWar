using UnityEngine;
public class MarkedEnemyIcon : MonoBehaviour
{
	[SerializeField]
	private string m_name;
	public bool AutoMaticDestroy = true;

	private void Start()
	{
		if ( transform.parent.name == "AbilityW" )
			if ( GetComponentInParent<AbilityBase>().Effect != null )
				m_name = GetComponentInParent<AbilityBase>().Effect.m_name;
	}
	private void Update()
	{
		if ( transform.parent.name != "AbilityW" )
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