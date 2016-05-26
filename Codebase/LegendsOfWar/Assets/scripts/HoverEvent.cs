using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	//public ParticleSystem button;
	public Sprite buttonOut = null;
	public Sprite buttonOver = null;
	public Sprite buttonPushed = null;

	public bool OnOff = false;
	// Use this for initialization
	//void Awake () {
	//       button = GetComponentInChildren<ParticleSystem>();
	//}

	public void OnPointerEnter( PointerEventData even )
	{
		OnOff = true;
		//   button.Play();
		gameObject.GetComponent<Image>().sprite = buttonOver;
	}

	public void OnPointerExit( PointerEventData even )
	{
		OnOff = false;
		//button.Stop();
		//button.Clear();
		gameObject.GetComponent<Image>().sprite = buttonOut;
	}


}
