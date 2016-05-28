using UnityEngine;
public class CasterEBehavior : MonoBehaviour
{
	public bool Activate = false;
	private void Update()
	{
		if ( Activate )
			if ( GetComponent<ParticleSystem>().isStopped )
				Destroy( this.gameObject );
	}
}