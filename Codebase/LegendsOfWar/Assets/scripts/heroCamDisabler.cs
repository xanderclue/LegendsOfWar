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
