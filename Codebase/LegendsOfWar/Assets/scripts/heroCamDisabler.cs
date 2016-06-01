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
// <BUGFIX: Dev Team #16>
using UnityEngine;
using System.Collections.Generic;
public class heroCamDisabler : MonoBehaviour
{
	static List<heroCamDisabler> disablers;
	void Awake() { if ( null == disablers ) disablers = new List<heroCamDisabler>(); }
	void OnEnable() { disablers.Add( this ); }
	void OnDisable() { disablers.Remove( this ); }
	public static bool disabledCameraMovement
	{ get { if ( null != disablers ) return disablers.Count > 0; return false; } }
}
// </BUGFIX: Dev Team #16>
#endif
#endregion //OLD_CODE