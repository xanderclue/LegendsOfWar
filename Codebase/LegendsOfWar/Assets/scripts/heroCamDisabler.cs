using UnityEngine;
using System.Collections.Generic;
public class heroCamDisabler : MonoBehaviour
{
	private static List<heroCamDisabler> disablers;
	public static bool disabledCameraMovement
	{
		get
		{
			if ( null != disablers )
				return disablers.Count > 0;
			return false;
		}
	}
	private void Awake()
	{
		if ( null == disablers )
			disablers = new List<heroCamDisabler>();
	}
	private void OnEnable()
	{
		disablers.Add( this );
	}
	private void OnDisable()
	{
		disablers.Remove( this );
	}
}