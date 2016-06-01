using UnityEngine;
public class TutMinionStart : MonoBehaviour
{
	[SerializeField]
	private GameObject MininionStart = null;
	private void OnTriggerEnter()
	{
		MininionStart.SetActive( true );
		gameObject.SetActive( false );
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using System.Collections;

public class TutMinionStart : MonoBehaviour {
    [SerializeField]
    GameObject MininionStart = null;
    [SerializeField]
    //GameObject Instruction = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        MininionStart.SetActive(true);
        //Instruction.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

#endif
#endregion //OLD_CODE