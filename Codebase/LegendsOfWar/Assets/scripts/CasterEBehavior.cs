using UnityEngine;
using System.Collections;

public class CasterEBehavior : MonoBehaviour
{
	public bool Activate = false;

	void Update()
	{
		if ( Activate )
		{
			if ( GetComponent<ParticleSystem>().isStopped )
			{
				Destroy( this.gameObject );
			}
		}
	}
}
