using UnityEngine;
using System.Collections;

public class CharIntroManager : MonoBehaviour {
    [SerializeField]
    menuEvents menuEventsObj = null;

    public bool Tutorial = true;

    void Awake()
    {

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        menuEventsObj.ChangeAppState("STATE_INTRODUCTION");

    }
}
