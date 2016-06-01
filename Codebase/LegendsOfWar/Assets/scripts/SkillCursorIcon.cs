using UnityEngine;
public class SkillCursorIcon : MonoBehaviour
{
	private RectTransform rTransform;
	private void Start()
	{
		rTransform = GetComponent<RectTransform>();
	}
	private void Update()
	{
		rTransform.position = CameraControl.Current.ScreenToWorldPoint( Input.mousePosition );
	}
}