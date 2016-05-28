using UnityEngine;
public class init : MonoBehaviour
{
	[SerializeField]
	private bool illegalStart = false;
	[SerializeField]
	private GameObject audioManager = null;

	private static bool appInited = false;
	private static init inst;
	private void Awake()
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