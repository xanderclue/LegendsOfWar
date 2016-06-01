using UnityEngine;
using UnityEngine.UI;
public class GoldGainGlow : MonoBehaviour
{
	[SerializeField]
	private float activeTime = 1.0f;
	private Image image;
	private Color originalColor, highlightedColor;
	private float activeTimer = 0.0f;
	private void Start()
	{
		EconomyManager.Instance.OnGainGold += GoldGained;
		image = GetComponent<Image>();
		originalColor = image.color;
		highlightedColor = originalColor;
		highlightedColor.r = highlightedColor.b = 0.0f;
	}
	private void Update()
	{
		activeTimer -= Time.deltaTime;
		if ( activeTimer < 0.0f )
			image.color = originalColor;
		else
			image.color = highlightedColor;
	}
	private void GoldGained()
	{
		activeTimer = activeTime;
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE