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
		set
		{
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