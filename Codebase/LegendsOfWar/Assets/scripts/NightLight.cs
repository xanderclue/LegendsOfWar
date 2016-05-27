using UnityEngine;
public class NightLight : MonoBehaviour
{
	[SerializeField]
	Light nightLight = null;
	void Start()
	{
		DayNight.OnDay += OnDay;
		DayNight.OnNight += OnNight;
	}
	void OnDestroy()
	{
		DayNight.OnDay -= OnDay;
		DayNight.OnNight -= OnNight;
	}
	void OnDay()
	{
		nightLight.enabled = false;
	}
	void OnNight()
	{
		nightLight.enabled = true;
	}
}