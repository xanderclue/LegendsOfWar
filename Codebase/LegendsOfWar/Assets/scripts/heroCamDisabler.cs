using UnityEngine;
using System.Collections.Generic;
public class heroCamDisabler : MonoBehaviour
{
	private static List<heroCamDisabler> disablers = new List<heroCamDisabler>();
	public static bool disabledCameraMovement
	{ get { return 0 < disablers.Count; } }
	private void OnEnable()
	{
		disablers.Add( this );
	}
	private void OnDisable()
	{
		disablers.Remove( this );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE