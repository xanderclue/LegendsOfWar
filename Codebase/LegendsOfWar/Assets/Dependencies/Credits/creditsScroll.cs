using UnityEngine;

public class creditsScroll : MonoBehaviour
{
	[SerializeField]
	float speed = 0.0f, initialY = 0.0f, finalY = 0.0f;
	Vector3 position;
	void Start()
	{
		position = transform.localPosition;
		position.y = initialY;
		transform.localPosition = position;
	}
	void Update()
	{
		position.y += Time.deltaTime * speed;
		if ( position.y > finalY )
			position.y = initialY;
		transform.localPosition = position;
		if ( Input.GetKeyDown( KeyCode.Escape ) )
			ApplicationManager.Instance.ChangeAppState( StateID.STATE_MAIN_MENU );
	}
}