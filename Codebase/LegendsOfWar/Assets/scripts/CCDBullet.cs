using UnityEngine;
public class CCDBullet : MonoBehaviour
{
	public float life;
	private Rigidbody body;
	private Ray ray;
	private RaycastHit hit;
	private void Start()
	{
		body = GetComponent<Rigidbody>();
		gameObject.layer = 31;
		Physics.IgnoreLayerCollision( 31, 31 );
		CheckCollision();
	}
	private void Update()
	{
		CheckCollision();
		life -= Time.deltaTime;
		if ( life <= 0.0f )
			Destroy( gameObject );
	}
	private void CheckCollision()
	{
		if ( Physics.Raycast( ray, out hit, body.velocity.magnitude * Time.deltaTime ) )
			if ( hit.transform.name != name )
			{
				body.velocity = body.velocity.normalized * ( hit.distance / Time.deltaTime );
				body.useGravity = true;
			}
	}
}