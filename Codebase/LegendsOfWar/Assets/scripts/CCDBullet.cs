using UnityEngine;
using System.Collections;

public class CCDBullet : MonoBehaviour
{

	public float life;
	Rigidbody body;

	void Start()
	{
		body = GetComponent<Rigidbody>();
		gameObject.layer = 31;
		Physics.IgnoreLayerCollision( 31, 31 );
		CheckCollision();
	}

	void Update()
	{
		CheckCollision();
		life -= Time.deltaTime;
		if ( life <= 0 )
		{ Destroy( this.gameObject ); }
	}

	void CheckCollision()
	{
		Ray ray = new Ray( transform.position, body.velocity );
		RaycastHit hit = new RaycastHit();
		if ( Physics.Raycast( ray, out hit, body.velocity.magnitude * Time.deltaTime ) )
		{
			if ( hit.transform.name != name )
			{
				body.velocity = body.velocity.normalized * ( hit.distance / Time.deltaTime );
				body.useGravity = true;
			}
			Debug.DrawLine( ray.origin, ray.origin + body.velocity * Time.deltaTime, Color.yellow );
		}
		else
		{
			Debug.DrawLine( ray.origin, ray.origin + body.velocity * Time.deltaTime, Color.white );
		}
	}

}
