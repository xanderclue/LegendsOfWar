using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Sprite buttonOut = null;
	public Sprite buttonOver = null;
	public Sprite buttonPushed = null;

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
