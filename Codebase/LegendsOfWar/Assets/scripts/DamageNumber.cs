using UnityEngine;
using UnityEngine.UI;
public class DamageNumber : MonoBehaviour
{
	[SerializeField]
	private Text textObject = null;
	private Vector3 low, high;
	private float maxDurationInv = 0.0f;
	private float tValue = 0.0f;
	private bool ready = false;
	private Transform orig;
	private bool followingTransform = false;
	private float heightF = 0.0f;

	public void CreateNumber( float number, Vector3 startPos, float height, float duration, Color
		textColor )
	{
		low = startPos;
		high = low + height * Vector3.up;
		textObject.text = Mathf.Round( number ).ToString( "F0" );
		textObject.color = textColor;
		maxDurationInv = 1.0f / duration;
		transform.position = low;
		ready = true;
	}
	public void CreateNumber( float number, Transform startTransform, float height, float duration,
		Color textColor )
	{
		CreateNumber( number, startTransform.position + 10.0f * Vector3.up, height, duration,
			textColor );
		heightF = height;
		orig = startTransform;
		followingTransform = true;
	}
	private void Start()
	{
		transform.LookAt( 2.0f * transform.position - HeroUIScript.Instance.transform.position );
		if ( !ready )
			textObject.text = "";
	}
	private void Update()
	{
		if ( ready )
		{
			tValue += Time.deltaTime * maxDurationInv;
			if ( tValue >= 1.0f )
			{
				Destroy( gameObject );
				return;
			}
			if ( followingTransform )
			{
				if ( orig )
				{
					low = orig.position + Vector3.up * 10.0f;
					high = low + heightF * Vector3.up;
				}
				else
					followingTransform = false;
			}
			transform.position = Vector3.Slerp( low, high, tValue );
		}
		transform.LookAt( 2.0f * transform.position - HeroUIScript.Instance.transform.position );
	}
}