using UnityEngine;
using UnityEngine.UI;

public class GoldGainGlow : MonoBehaviour
{
	Image image;
	Color originalColor;
	Color highlightedColor;
	float activeTimer = 0.0f;
	[SerializeField]
	float activeTime = 1.0f;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	void Start()
	{
		EconomyManager.Instance.OnGainGold += GoldGained;
		originalColor = image.color;
		highlightedColor = originalColor;
		highlightedColor.r = highlightedColor.b = 0.0f;
	}

	void Update()
	{
		activeTimer -= Time.deltaTime;
		if ( activeTimer >= 0.0f )
			image.color = highlightedColor;
		else
			image.color = originalColor;
	}

	void GoldGained()
	{
		activeTimer = activeTime;
	}
}
