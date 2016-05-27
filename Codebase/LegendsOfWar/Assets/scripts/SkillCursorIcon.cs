using UnityEngine;
public class SkillCursorIcon : MonoBehaviour
{
	void Update()
	{
		if ( gameObject.activeInHierarchy )
			GetComponent<RectTransform>().position = CameraControl.Current.ScreenToWorldPoint( Input
				.mousePosition );
	}
}