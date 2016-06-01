using System.Collections.Generic;
using UnityEngine;
public class SupportRange : MonoBehaviour
{
	public static List<Collider> supportedEntities = new List<Collider>();
	private List<Collider> mySupportedEntities, nearbyEnemies;
	private Info iTmp, hInfo;
	public static bool InSupportRange( GameObject entity )
	{
		supportedEntities.RemoveAll( item => !item );
		foreach ( Collider col in supportedEntities )
			if ( entity == col.gameObject )
				return true;
		return false;
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
	private void Awake()
	{
		mySupportedEntities = new List<Collider>();
		nearbyEnemies = new List<Collider>();
		hInfo = GetComponentInParent<HeroInfo>();
	}
	private void OnTriggerEnter( Collider col )
	{
		iTmp = col.gameObject.GetComponent<Info>();
		if ( iTmp )
		{
			if ( iTmp.team == hInfo.team )
			{
				supportedEntities.Add( col );
				mySupportedEntities.Add( col );
			}
			else
				nearbyEnemies.Add( col );
		}
		ClearNullsSelf();
	}
	private void OnTriggerExit( Collider col )
	{
		ClearNullsSelf();
		iTmp = col.gameObject.GetComponent<Info>();
		if ( iTmp )
		{
			if ( iTmp.team == hInfo.team )
			{
				mySupportedEntities.Remove( col );
				supportedEntities.Remove( col );
			}
			else
				nearbyEnemies.Remove( col );
		}
	}
	private void ClearNullsSelf()
	{
		supportedEntities.RemoveAll( item => !item );
		mySupportedEntities.RemoveAll( item => !item );
		nearbyEnemies.RemoveAll( item => !item );
	}
}
#region OLD_CODE
#if false
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
		if ( null != info )
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
			{ supportedEntities.RemoveAt( i ); --i; }
	}

	void ClearNullsSelf()
	{
		ClearNulls();
		for ( int i = 0; i < mySupportedEntities.Count; ++i )
			if ( !mySupportedEntities[ i ] )
			{ mySupportedEntities.RemoveAt( i ); --i; }

		for ( int i = 0; i < nearbyEnemies.Count; ++i )
			if ( !nearbyEnemies[ i ] )
			{ nearbyEnemies.RemoveAt( i ); --i; }
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
#endif
#endregion //OLD_CODE