using UnityEngine;
using UnityEngine.UI;
public class sub : MonoBehaviour
{
	static sub inst = null;
	static Text text = null;
	static float timer = -0.0f;
	void Awake()
	{
		inst = this;
	}
	void Start()
	{
		text = GetComponentInChildren<Text>();
		gameObject.SetActive( false );
	}
	void Update()
	{
		if ( timer <= 0.0f )
			gameObject.SetActive( false );
		timer -= Time.deltaTime;
	}
	void OnDestroy()
	{
		inst = null;
		text = null;
		timer = -0.0f;
	}
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
}