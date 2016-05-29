using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Sprite buttonOut = null, buttonOver = null;
	public Sprite buttonPushed;
	public bool OnOff = false;
	public void OnPointerEnter( PointerEventData even )
	{
		OnOff = true;
		gameObject.GetComponent<Image>().sprite = buttonOver;
	}
	public void OnPointerExit( PointerEventData even )
	{
		OnOff = false;
		gameObject.GetComponent<Image>().sprite = buttonOut;
	}
}