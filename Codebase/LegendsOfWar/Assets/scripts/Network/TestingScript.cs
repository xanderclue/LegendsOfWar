using UnityEngine;
using UnityEngine.Networking;

public class TestingScript : MonoBehaviour
{
	void Start ()
	{

	}

	void Update ()
	{

	}
}

class SomeStringMessage : MessageBase
{
	public override void Deserialize( NetworkReader reader )
	{
		base.Deserialize( reader );
	}
	public override void Serialize( NetworkWriter writer )
	{
		base.Serialize( writer );
	}
}