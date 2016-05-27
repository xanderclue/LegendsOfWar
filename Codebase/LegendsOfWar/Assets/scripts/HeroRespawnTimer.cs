using UnityEngine;
using UnityEngine.UI;
public class HeroRespawnTimer : MonoBehaviour
{
	HeroInfo info;
	[SerializeField]
	Text text = null;
	void Start()
	{
		info = GameManager.Instance.Player.GetComponent<HeroInfo>();
		info.Destroyed += ShowTimer;
		gameObject.SetActive( false );
	}
	void Update()
	{
		if ( info.RespawnTimer <= 0.0f )
			gameObject.SetActive( false );
		else
			text.text = ( Options.Japanese ? "生変：" : "Respawn: " ) + info.RespawnTimer.ToString( "F2" );
	}
	void ShowTimer()
	{
		text.text = info.RespawnTimer.ToString( "F2" );
		gameObject.SetActive( true );
	}
}