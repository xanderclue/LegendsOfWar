using UnityEngine;
public class creditsScroll : MonoBehaviour
{
	[SerializeField]
	private float speed = 0.0f, initialY = 0.0f, finalY = 0.0f;
	private Vector3 position;
	private void Start()
	{
		position = transform.localPosition;
		position.y = initialY;
		transform.localPosition = position;
	}
	private void Update()
	{
		position.y += Time.deltaTime * speed;
		if ( position.y > finalY )
			position.y = initialY;
		transform.localPosition = position;
		if ( Input.GetKeyDown( KeyCode.Escape ) )
			ApplicationManager.Instance.ChangeAppState( StateID.STATE_MAIN_MENU );
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE