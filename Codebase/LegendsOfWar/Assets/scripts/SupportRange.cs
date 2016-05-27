using System.Collections.Generic;
using UnityEngine;
public class SupportRange : MonoBehaviour
{
	public static List<Collider> supportedEntities = new List<Collider>();
	List<Collider> mySupportedEntities;
	List<Collider> nearbyEnemies;
	void Awake()
	{
		mySupportedEntities = new List<Collider>();
		nearbyEnemies = new List<Collider>();
	}
	void OnTriggerEnter( Collider col )
	{
		Info info = col.gameObject.GetComponent<Info>();
		if ( info )
		{
			if ( info.team == GetComponentInParent<HeroInfo>().team )
			{
				supportedEntities.Add( col );
				mySupportedEntities.Add( col );
			}
			else
				nearbyEnemies.Add( col );
		}
		ClearNullsSelf();
	}
	void OnTriggerExit( Collider col )
	{
		ClearNullsSelf();
		Info i = col.gameObject.GetComponent<Info>();
		if ( i )
		{
			if ( i.team == GetComponentInParent<HeroInfo>().team )
			{
				mySupportedEntities.Remove( col );
				supportedEntities.Remove( col );
			}
			else
				nearbyEnemies.Remove( col );
		}
	}
	public static bool InSupportRange( GameObject entity )
	{
		ClearNulls();
		foreach ( Collider col in supportedEntities )
			if ( col.gameObject == entity )
				return true;
		return false;
	}
	static void ClearNulls()
	{
		for ( int i = 0; i < supportedEntities.Count; ++i )
			if ( !supportedEntities[ i ] )
				supportedEntities.RemoveAt( i-- );
	}
	void ClearNullsSelf()
	{
		ClearNulls();
		for ( int i = 0; i < mySupportedEntities.Count; ++i )
			if ( !mySupportedEntities[ i ] )
				mySupportedEntities.RemoveAt( i-- );
		for ( int i = 0; i < nearbyEnemies.Count; ++i )
			if ( !nearbyEnemies[ i ] )
				nearbyEnemies.RemoveAt( i-- );
	}
	public void ApplyToAlliesInRange( System.Action<Info> action )
	{
		ClearNullsSelf();
		foreach ( Collider col in mySupportedEntities )
			action( col.gameObject.GetComponent<Info>() );
	}
	public void ApplyToEnemiesInRange( System.Action<Info> action )
	{
		ClearNullsSelf();
		foreach ( Collider col in mySupportedEntities )
			action( col.gameObject.GetComponent<Info>() );
	}
}