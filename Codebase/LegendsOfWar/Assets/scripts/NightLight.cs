using UnityEngine;
public class NightLight : MonoBehaviour
{
	[SerializeField]
	private Light nightLight = null;
	private void Start()
	{
		DayNight.OnDay += OnDay;
		DayNight.OnNight += OnNight;
	}
	private void OnDestroy()
	{
		DayNight.OnDay -= OnDay;
		DayNight.OnNight -= OnNight;
	}
	private void OnDay()
	{
		nightLight.enabled = false;
	}
	private void OnNight()
	{
		nightLight.enabled = true;
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE