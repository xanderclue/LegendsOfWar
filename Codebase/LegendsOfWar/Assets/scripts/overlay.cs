using UnityEngine;
using UnityEngine.UI;
public class overlay : MonoBehaviour
{
	private static overlay inst = null;
	private Image image;
	private Color empty = new Color( 0.0f, 0.0f, 0.0f, 0.0f );
	private float invDuration = 0.0f, time = 0.0f, level = 0.0f;
	public static void Flash( float health, float INVmaxHealth )
	{
		inst.time = 0.75f;
		inst.invDuration = 1.333333333f;
		inst.level = health * INVmaxHealth;
	}
	private void Awake()
	{
		inst = this;
	}
	private void Start()
	{
		image = GetComponent<Image>();
	}
	private void Update()
	{
		time -= Time.deltaTime;
		if ( time < 0.0f )
			image.color = empty;
		else
			image.color = new Color( 1.0f, level, 0.0f, time * invDuration );
	}
}