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
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class CasterEBehavior : MonoBehaviour {
    public bool Activate = false;

    void Update()
    {
        if (Activate)
        {
            if (GetComponent<ParticleSystem>().isStopped)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

#endif
#endregion //OLD_CODE