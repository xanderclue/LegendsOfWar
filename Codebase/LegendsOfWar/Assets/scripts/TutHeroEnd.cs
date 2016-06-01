using UnityEngine;
public class TutHeroEnd : MonoBehaviour
{
	[SerializeField]
	private GameObject HeroOnly = null;
	private void OnTriggerEnter()
	{
		HeroOnly.SetActive( false );
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class TutHeroEnd : MonoBehaviour {

    [SerializeField]
    GameObject HeroOnly;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        HeroOnly.SetActive(false);
    }
}

#endif
#endregion //OLD_CODE