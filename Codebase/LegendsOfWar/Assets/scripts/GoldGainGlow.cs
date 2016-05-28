using UnityEngine;
using UnityEngine.UI;
public class GoldGainGlow : MonoBehaviour
{
	[SerializeField]
	private float activeTime = 1.0f;
	private Image image;
	private Color originalColor;
	private Color highlightedColor;
	private float activeTimer = 0.0f;

	private void Awake()
	{
		image = GetComponent<Image>();
	}
	private void Start()
	{
		EconomyManager.Instance.OnGainGold += GoldGained;
		originalColor = image.color;
		highlightedColor = originalColor;
		highlightedColor.r = highlightedColor.b = 0.0f;
	}
	private void Update()
	{
		activeTimer -= Time.deltaTime;
		if ( activeTimer >= 0.0f )
			image.color = highlightedColor;
		else
			image.color = originalColor;
	}
	private void GoldGained()
	{
		activeTimer = activeTime;
	}
}