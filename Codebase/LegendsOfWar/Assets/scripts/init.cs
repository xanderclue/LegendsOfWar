using UnityEngine;
public class init : MonoBehaviour
{
	private static bool appInited = false;
	[SerializeField]
	private bool illegalStart = false;
	private static init inst;
	[SerializeField]
	private GameObject audioManager = null;
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