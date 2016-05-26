using UnityEngine;

public class Detector : MonoBehaviour
{
	public delegate void triggerEvent( GameObject obj );
	public event triggerEvent triggerEnter, triggerExit;
	[SerializeField]
	SphereCollider detectionSphere = null;

	void Start()
	{
		if ( !detectionSphere )
			detectionSphere = GetComponent<SphereCollider>();
	}

	public void CreateTrigger( float _radius )
	{
		if ( !detectionSphere )
			detectionSphere = gameObject.AddComponent<SphereCollider>();
		detectionSphere.isTrigger = true;
		detectionSphere.radius = _radius / transform.parent.lossyScale.x;
	}

	void OnTriggerEnter( Collider col )
	{
		if ( null != triggerEnter )
			triggerEnter( col.gameObject );
	}

	void OnTriggerExit( Collider col )
	{
		if ( null != triggerExit )
			triggerExit( col.gameObject );
	}
}