using UnityEngine;
using UnityEngine.UI;

public class HeroIcon : MonoBehaviour
{
	[SerializeField]
	Image heroIcon = null;
	HeroInfo info = null;

	void SetIcon()
	{
		if ( CharacterSelectionManager.LegendChoice )
			info = CharacterSelectionManager.heroInfo;
		if ( info )
			heroIcon.sprite = info.heroIcon;
	}
	void Update() { SetIcon(); }
	public void Force() { SetIcon(); }
}