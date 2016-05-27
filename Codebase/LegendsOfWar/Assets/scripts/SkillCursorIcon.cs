using UnityEngine;
public class SkillCursorIcon : MonoBehaviour
{
	private void Update()
	{
		if ( gameObject.activeInHierarchy )
			GetComponent<RectTransform>().position = CameraControl.Current.ScreenToWorldPoint( Input
				.mousePosition );
	}
}