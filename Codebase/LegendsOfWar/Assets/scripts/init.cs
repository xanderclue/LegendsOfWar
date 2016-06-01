using UnityEngine;
public class init : MonoBehaviour
{
	[SerializeField]
	private bool illegalStart = false;
	[SerializeField]
	private GameObject audioManager = null;
	private static init inst;
	private static bool appInited = false;
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
	private void Awake()
	{
		inst = this;
		InitApp();
	}
}
#region OLD_CODE
#if false
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
#endif
#endregion //OLD_CODE