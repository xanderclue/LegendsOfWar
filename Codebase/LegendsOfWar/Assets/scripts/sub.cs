using UnityEngine;
using UnityEngine.UI;
public class sub : MonoBehaviour
{
	private static sub inst = null;
	private static Text text = null;
	private static float timer = -0.0f;
	public static void SetSub( string txt, float time )
	{
		if ( time <= 0.0f && inst )
			inst.gameObject.SetActive( false );
		else if ( txt.Length <= 0 )
			return;
		else if ( inst )
		{
			if ( !text )
				text = inst.GetComponentInChildren<Text>();
			if ( text )
			{
				text.text = txt;
				timer = time;
				inst.gameObject.SetActive( true );
			}
		}
	}
	private void Awake()
	{
		inst = this;
	}
	private void Start()
	{
		text = GetComponentInChildren<Text>();
		gameObject.SetActive( false );
	}
	private void Update()
	{
		if ( timer <= 0.0f )
			gameObject.SetActive( false );
		timer -= Time.deltaTime;
	}
	private void OnDestroy()
	{
		inst = null;
	}
}