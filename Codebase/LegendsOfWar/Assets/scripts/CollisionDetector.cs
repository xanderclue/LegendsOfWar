using UnityEngine;
using System.Collections.Generic;
public class CollisionDetector : MonoBehaviour
{
	public List<Collider> targetedEnemies;
	public TankAbilityW w = null;
	public void DealDamage( System.Action<Info> action )
	{
		foreach ( Collider _target in targetedEnemies )
			if ( _target )
				action( _target.gameObject.GetComponent<Info>() );
	}
	private void Awake()
	{
		targetedEnemies = new List<Collider>();
	}
	private void OnTriggerEnter( Collider _target )
	{
		if ( _target.GetComponent<Info>() != null )
			if ( _target.gameObject.GetComponent<Info>().team != GetComponentInParent<HeroInfo>().
				team )
				targetedEnemies.Add( _target );
	}
	private void OnTriggerExit( Collider _target )
	{
		if ( _target.GetComponent<Info>() != null )
			targetedEnemies.Remove( _target );
	}
}