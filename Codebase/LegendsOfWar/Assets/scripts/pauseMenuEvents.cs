using UnityEngine;
using UnityEngine.UI;
public class pauseMenuEvents : MonoBehaviour
{
	[SerializeField]
	private GameObject eventSystem = null;
	[SerializeField]
	private Button[ ] buttons = null;
	private static pauseMenuEvents inst = null;
	public static bool EventSystem
	{
		set
		{
			inst.eventSystem.SetActive( value );
			foreach ( Button button in inst.buttons )
				button.enabled = value;
		}
	}
	public void Unpause()
	{
		if ( StateID.STATE_PAUSED == ApplicationManager.Instance.GetAppState() )
			GameManager.Instance.Unpause();
	}
	private void Awake()
	{
		inst = this;
	}
	private void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Escape ) )
			Unpause();
	}
}
#region OLD_CODE
#if false
using UnityEngine;
using UnityEngine.UI;

public class pauseMenuEvents : MonoBehaviour
{
	[SerializeField]
	GameObject eventSystem = null;

	[SerializeField]
	Button[ ] buttons = null;

	static pauseMenuEvents inst = null;
	public static bool EventSystem
	{
		get { return inst.eventSystem.activeInHierarchy; }
		set {
			inst.eventSystem.SetActive( value );
			foreach ( Button button in inst.buttons )
				button.enabled = value;
		}
	}

	void Awake()
	{
		inst = this;
	}

	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Escape ) )
			Unpause();
	}

	public void Unpause()
	{
		if ( ApplicationManager.Instance.GetAppState() == StateID.STATE_PAUSED )
			GameManager.Instance.Unpause();
	}
}
#endif
#endregion //OLD_CODE