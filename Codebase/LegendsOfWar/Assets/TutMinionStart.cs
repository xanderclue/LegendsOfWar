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
