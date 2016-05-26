using UnityEngine;

public class init : MonoBehaviour
{
	static bool appInited = false;
	[SerializeField]
	bool illegalStart = false;
	static init inst;
	[SerializeField]
	GameObject audioManager = null;
	void Awake()
	{
		inst = this;
		InitApp();
	}
	public static void InitApp()
	{
		if ( !appInited )
		{
			Profiler.maxNumberOfSamplesPerFrame = -1;
			Application.targetFrameRate = 90;
			Options.Init();
			appInited = true;
			if ( inst )
				if ( inst.illegalStart )
					UnityEngine.SceneManagement.SceneManager.LoadScene( "MainMenu" );
			Instantiate( inst.audioManager );
		}
	}
	public void Init()
	{
		InitApp();
	}
}