using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Sprite buttonOut = null, buttonOver = null;
	public Sprite buttonPushed;
	public bool OnOff = false;
	private Image image;
	private void Start()
	{
		image = GetComponent<Image>();
	}
	public void OnPointerEnter( PointerEventData even )
	{
		OnOff = true;
		image.sprite = buttonOver;
	}
	public void OnPointerExit( PointerEventData even )
	{
		OnOff = false;
		image.sprite = buttonOut;
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE