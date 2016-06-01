using UnityEngine;
public class Detector : MonoBehaviour
{
	[SerializeField]
	private SphereCollider detectionSphere = null;
	public delegate void triggerEvent( GameObject obj );
	public event triggerEvent triggerEnter, triggerExit;
	public void CreateTrigger( float _radius )
	{
		if ( !detectionSphere )
			detectionSphere = gameObject.AddComponent<SphereCollider>();
		detectionSphere.isTrigger = true;
		detectionSphere.radius = _radius / transform.parent.lossyScale.x;
	}
	private void Start()
	{
		if ( !detectionSphere )
			detectionSphere = GetComponent<SphereCollider>();
	}
	private void OnTriggerEnter( Collider col )
	{
		if ( null != triggerEnter )
			triggerEnter( col.gameObject );
	}
	private void OnTriggerExit( Collider col )
	{
		if ( null != triggerExit )
			triggerExit( col.gameObject );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE