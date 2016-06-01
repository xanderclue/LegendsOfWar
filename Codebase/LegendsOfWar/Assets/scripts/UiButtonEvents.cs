using UnityEngine;
using UnityEngine.EventSystems;
public class UiButtonEvents : MonoBehaviour
{
	public void SetSelectedThis()
	{
		EventSystem.current.SetSelectedGameObject( gameObject );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE