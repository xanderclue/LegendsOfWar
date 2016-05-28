using UnityEngine;
using UnityEngine.UI;
public class HeroIcon : MonoBehaviour
{
	[SerializeField]
	private Image heroIcon = null;

	private HeroInfo info = null;
	private void SetIcon()
	{
		if ( CharacterSelectionManager.LegendChoice )
			info = CharacterSelectionManager.heroInfo;
		if ( info )
			heroIcon.sprite = info.heroIcon;
	}
	private void Update()
	{
		SetIcon();
	}
	public void Force()
	{
		SetIcon();
	}
}