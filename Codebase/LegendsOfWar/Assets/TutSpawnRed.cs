using UnityEngine;
using System.Collections;

public class TutSpawnRed : MonoBehaviour {

    [SerializeField]
   GameObject IntroductionManager;

    //[SerializeField]
    //GameObject Intro;
    [SerializeField]
    GameObject Health;
    [SerializeField]
    GameObject Hero;
    // Use this for initialization

    public bool Battle;
    void Start () {
        Battle = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        Hero.SetActive(true);
        Health.SetActive(true);
        //Intro.SetActive(true);
        IntroductionManager.GetComponent<IntroManager>().SpawnRedStrikerMinion();
        IntroductionManager.GetComponent<IntroManager>().SpawnRedStrikerMinion();
        IntroductionManager.GetComponent<IntroManager>().SpawnRedStrikerMinion();
        Battle = true;
        this.gameObject.SetActive(false);

    }
}
