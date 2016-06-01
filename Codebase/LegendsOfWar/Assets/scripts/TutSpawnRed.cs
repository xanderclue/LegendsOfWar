using UnityEngine;
public class TutSpawnRed : MonoBehaviour
{
	[SerializeField]
	private GameObject IntroductionManager = null, Health = null, Hero = null;
	public bool Battle;
	private IntroManager introManager;
	private void Start()
	{
		Battle = false;
		introManager = IntroductionManager.GetComponent<IntroManager>();
	}
	private void OnTriggerEnter()
	{
		Hero.SetActive( true );
		Health.SetActive( true );
		introManager.SpawnRedStrikerMinion();
		introManager.SpawnRedStrikerMinion();
		introManager.SpawnRedStrikerMinion();
		Battle = true;
		this.gameObject.SetActive( false );
	}
}
#region OLD_CODE
#if false
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

#endif
#endregion //OLD_CODE