using UnityEngine;
using UnityEngine.UI;
public class HeroIcon : MonoBehaviour
{
	[SerializeField]
	private Image heroIcon = null;
	private HeroInfo info = null;
	public void Force()
	{
		SetIcon();
	}
	private void Update()
	{
		SetIcon();
	}
	private void SetIcon()
	{
		if ( CharacterSelectionManager.LegendChoice )
			info = CharacterSelectionManager.heroInfo;
		if ( info )
			heroIcon.sprite = info.heroIcon;
	}
}
#region OLD_CODE
#if false
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
#endif
#endregion //OLD_CODE