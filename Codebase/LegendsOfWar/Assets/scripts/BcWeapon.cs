using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BcWeapon : MonoBehaviour
{
	[Header( "Weapon Options" )]
	public bool autofire = true;
	[Tooltip( "Add here any axis from the Input Settings to fire when using that axis" )]
	public string triggerAxis = "Fire1";
	public bool isSemiAutomatic = true;
	public float horizontalSpread = 0.0f, verticalSpread = 0.0f, rateOfFire = 1.0f;
	public int clipSize = 10;
	public float reloadTime = 1.0f;
	[Header( "Bullet Options" )]
	public GameObject bulletPrefab;
	public int bulletsPerShot = 1;
	public float bulletLife = 5.0f;
	public Vector3 bulletAcceleration, bulletGlobalAcceleration;
	public float bulletSpeed = 5.0f, bulletSpeedDelta = 0.0f, accelerationScale;
	[Header( "Gizmo Options" )]
	public bool showDisplayHandles = false;
	public float displayScale = 1.0f;
	public Vector3 displayOffset = -10.0f * Vector3.one + 20.0f * Vector3.right;
	public float currentAmmo;
	[Header( "Sound Options" )]
	public AudioClip shootSound, reloadingSound, reloadedSound, emptyClickSound;
	private SiegeProjectile siegeProjectile;
	private float bulletMass = 1.0f, shootTimer, reloadTimer, lastTriggerValue;
	private bool isReloading, triggerPushed;
	public Vector3 DisplayPosition
	{ get { return DisplayOffsetF + transform.position; } }
	public Vector3 DisplayOffsetF
	{
		get
		{
			return displayOffset.z * transform.forward * transform.lossyScale.z + displayOffset.y *
				transform.up * transform.lossyScale.y + displayOffset.x * transform.right *
				transform.lossyScale.x;
		}
	}
	public float siegeProjectileDamage
	{ set { siegeProjectile.damage = value; } }
	private static bool RoughlyEqual( float a, float b )
	{
		return ( Mathf.Abs( a - b ) < 0.01f );
	}
	public void Reload()
	{
		if ( !isReloading )
		{
			AudioManager.PlaySoundEffect( reloadingSound, transform.position );
			isReloading = true;
		}
		shootTimer = 0.0f;
		reloadTimer += Time.deltaTime;
		currentAmmo = -reloadTimer / reloadTime * clipSize;
		if ( currentAmmo <= -clipSize )
		{
			currentAmmo = clipSize;
			isReloading = false;
			reloadTimer = 0.0f;
			shootTimer = 0.0f;
			AudioManager.PlaySoundEffect( reloadedSound, transform.position );
		}
	}
	public void Shoot()
	{
		if ( !isReloading && 0.0f < currentAmmo && shootTimer <= 0.0f )
		{
			--currentAmmo;
			shootTimer += 1.0f / rateOfFire;
			SpawnBullet();
			AudioManager.PlaySoundEffect( shootSound, transform.position );
		}
		if ( isReloading && triggerPushed && !isSemiAutomatic )
			AudioManager.PlaySoundEffect( emptyClickSound, transform.position );
	}
	public void SpawnBullet()
	{
		for ( int i = 0; i < bulletsPerShot; ++i )
		{
			GameObject newBullet = Instantiate( bulletPrefab, transform.position, transform.rotation
				) as GameObject;
			Rigidbody rigidBody = newBullet.AddComponent<Rigidbody>();
			rigidBody.mass = bulletMass;
			rigidBody.drag = 0.0f;
			rigidBody.useGravity = false;
			rigidBody.velocity = transform.forward * ( bulletSpeed + bulletSpeedDelta * ( Random.
				value - 0.5f ) * 2.0f ) + transform.up * ( verticalSpread * ( Random.value - 0.5f )
				* 2.0f ) + transform.right * ( horizontalSpread * ( Random.value - 0.5f ) * 2.0f );
			ConstantForce force = newBullet.AddComponent<ConstantForce>();
			force.force = ( bulletAcceleration.x * transform.forward + bulletAcceleration.y *
				transform.up + bulletAcceleration.z * transform.right ) * accelerationScale;
			force.force += accelerationScale * bulletGlobalAcceleration;
			force.force *= 50.0f;
			newBullet.AddComponent<CCDBullet>().life = bulletLife;
		}
	}
	public float IsOverCannon()
	{
		if ( displayOffset.y > 0.0f )
			return 1.0f;
		if ( displayOffset.y < 0.0f )
			return -1.0f;
		return 0.0f;
	}
	public Vector3 ShotEnd( Vector3 initialSpeed, float time, Color color )
	{
		return DrawTrajectory( transform.position, initialSpeed, time, color, false );
	}
	private void Start()
	{
		isReloading = false;
		currentAmmo = clipSize;
		shootTimer = 0.0f;
		siegeProjectile = bulletPrefab.GetComponent<SiegeProjectile>();
	}
	private void Update()
	{
		triggerPushed = Input.GetAxis( triggerAxis ) > lastTriggerValue;
		lastTriggerValue = Input.GetAxis( triggerAxis );
		if ( autofire || Input.GetAxis( triggerAxis ) > 0.0f )
			if ( isSemiAutomatic || triggerPushed )
				Shoot();
		if ( !isReloading && shootTimer > 0.0f )
			shootTimer -= Time.deltaTime;
		if ( currentAmmo <= 0.0f && shootTimer <= 0.0f )
			Reload();
	}
	private void OnDrawGizmosSelected()
	{
		if ( this.enabled )
		{
			Mesh hull = new Mesh();
			hull.Clear();
			Vector3[ ] vertexMatrix = new Vector3[ 8 ];
			Vector3[ ] dots1, dots2;
			dots1 = DrawShot( transform.forward * ( bulletSpeed + bulletSpeedDelta ), bulletLife );
			dots2 = DrawShot( transform.forward * ( bulletSpeed - bulletSpeedDelta ), bulletLife );
			System.Array.Copy( dots1, 0, vertexMatrix, 0, dots1.Length );
			System.Array.Copy( dots2, 0, vertexMatrix, dots1.Length, dots2.Length );
			hull.vertices = vertexMatrix;
			Vector3 s, d;
			foreach ( Vector3 vert in hull.vertices )
			{
				Gizmos.color = Color.white;
				foreach ( Vector3 check in hull.vertices )
				{
					s = transform.worldToLocalMatrix.MultiplyVector( vert );
					d = transform.worldToLocalMatrix.MultiplyVector( check );
					if ( RoughlyEqual( s.x, d.x ) ^ RoughlyEqual( s.y, d.y ) ^ RoughlyEqual( s.z, d.
						z ) )
						continue;
					if ( RoughlyEqual( s.x, d.x ) || RoughlyEqual( s.y, d.y ) || RoughlyEqual( s.z,
						d.z ) )
						Gizmos.DrawLine( vert, check );
				}
			}
			Gizmos.DrawWireSphere( DrawTrajectory( transform.position, transform.forward * (
				bulletSpeed ), bulletLife, Color.yellow ), 0.3f );
			float width = clipSize * displayScale / rateOfFire;
			float height = displayScale;
			Vector3 dispPos = DisplayPosition;
			for ( float i = 0.0f; i < clipSize / rateOfFire; i += 1.0f )
			{
				float size = 0.25f * height;
				if ( 0.0f == i % 5.0f )
					size = 0.5f * height;
				if ( 0.0f == i % 10.0f )
					size = height;
				Gizmos.color = Color.white;
				Gizmos.DrawLine( dispPos + transform.forward * ( i * displayScale ), dispPos +
					transform.up * size + transform.forward * ( i * displayScale ) );
			}
			Gizmos.color = Color.white;
			Gizmos.DrawLine( dispPos + transform.up * height, dispPos + transform.up * height +
				transform.forward * width );
			Gizmos.DrawLine( dispPos, dispPos + transform.forward * width );
			Gizmos.DrawLine( dispPos + transform.up * height + transform.forward * width, dispPos +
				transform.forward * width );
			Gizmos.color = Color.green;
			Gizmos.DrawLine( dispPos - transform.up * ( height * 0.5f ), dispPos + transform.up *
				( height * 0.5f ) + transform.forward * ( Mathf.Max( 0.0f, currentAmmo + shootTimer
				* rateOfFire ) / clipSize * width ) );
			for ( float i = 0.0f; i < clipSize; i += 1.0f )
			{
				float size = 0.15f * height;
				if ( 0.0f == i % 5.0f )
					size = 0.5f * height;
				if ( 0.0f == i % 10.0f )
					size = height;
				Gizmos.color = Color.white;
				Gizmos.DrawLine( dispPos - transform.up * height + transform.forward * ( i * width /
					clipSize ), dispPos - transform.up * ( height - size ) + transform.forward * ( i
					* width / clipSize ) );
			}
			Gizmos.color = Color.white;
			Gizmos.DrawLine( dispPos, dispPos + transform.forward * width );
			Gizmos.DrawLine( dispPos - transform.up * height, dispPos - transform.up * height +
				transform.forward * width );
			Gizmos.DrawLine( dispPos + transform.forward * width, dispPos - transform.up * height +
				transform.forward * width );
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine( dispPos - transform.up * ( height * 0.5f ), dispPos - transform.up * (
				height * 0.5f ) + transform.forward * ( Mathf.Max( 0.0f, currentAmmo ) / clipSize *
				width ) );
			for ( float i = 0.0f; i < reloadTime; i += 1.0f )
			{
				float size = 0.15f * height;
				if ( 0.0f == i % 5.0f )
					size = 0.5f * height;
				if ( 0.0f == i % 10.0f )
					size = height;
				Gizmos.color = Color.white;
				Gizmos.DrawLine( dispPos - transform.forward * ( i * displayScale ), dispPos +
					transform.up * size - transform.forward * ( i * displayScale ) );
			}
			Gizmos.color = Color.white;
			Gizmos.DrawLine( dispPos + transform.up * height, dispPos + transform.up * height -
				transform.forward * ( reloadTime * displayScale ) );
			Gizmos.DrawLine( dispPos, dispPos - transform.forward * ( reloadTime * displayScale ) );
			Gizmos.DrawLine( dispPos + transform.up * height - transform.forward * ( reloadTime *
				displayScale ), dispPos - transform.forward * ( reloadTime * displayScale ) );
			Gizmos.color = Color.red;
			Gizmos.DrawLine( dispPos + transform.up * ( height * 0.5f ), dispPos + transform.up * (
				height * 0.5f ) - transform.forward * ( reloadTimer * displayScale ) );
			Gizmos.color = Color.white;
			Gizmos.DrawLine( transform.position, transform.position + transform.up * ( height * 2.0f
				* IsOverCannon() ) );
			Gizmos.DrawLine( transform.position + transform.up * ( height * 2.0f * IsOverCannon() ),
				dispPos + transform.up * ( height * -2.0f * IsOverCannon() ) );
			Gizmos.DrawLine( dispPos + transform.up * ( height * -2.0f * IsOverCannon() ), dispPos )
				;
			if ( autofire )
				Gizmos.color = new Color( 0.75f, 0.5f, 0.5f );
			else
				Gizmos.color = Color.white;
			Gizmos.DrawLine( transform.position + displayScale * transform.up, transform.position -
				displayScale * transform.up );
			Gizmos.DrawLine( transform.position + displayScale * transform.right, transform.position
				- displayScale * transform.right );
			Gizmos.DrawLine( transform.position + displayScale * 1.5f * transform.forward, transform
				.position - displayScale * transform.forward );
			Gizmos.DrawLine( transform.position + displayScale * 1.5f * transform.forward, transform
				.position + transform.up * ( -0.5f * displayScale ) );
			Gizmos.DrawLine( transform.position + displayScale * 1.5f * transform.forward, transform
				.position + transform.up * ( 0.5f * displayScale ) );
			Gizmos.DrawLine( transform.position + displayScale * 1.5f * transform.forward, transform
				.position + transform.right * ( -0.5f * displayScale ) );
			Gizmos.DrawLine( transform.position + displayScale * 1.5f * transform.forward, transform
				.position + transform.right * ( 0.5f * displayScale ) );
			Gizmos.DrawWireSphere( transform.position, 0.475f * displayScale );
			Gizmos.color = Color.cyan;
			for ( int i = 0; i < bulletsPerShot; ++i )
			{
				float size = 0.5f;
				if ( 0 == i % 5 )
					size = 1.0f;
				if ( 0 == i % 10 )
					size = 2.0f;
				Gizmos.DrawLine( transform.position + transform.up * ( i * displayScale * 2.0f /
					bulletsPerShot ), transform.position + transform.forward * ( size * displayScale
					) + transform.up * ( i * displayScale * 2.0f / bulletsPerShot ) );
			}
			DestroyImmediate( hull );
		}
	}
	private Vector3[ ] DrawShot( Vector3 initialSpeed, float time )
	{
		Vector3[ ] res = new Vector3[ 4 ];
		res[ 0 ] = DrawTrajectory( transform.position, initialSpeed - transform.up * verticalSpread
			- transform.right * horizontalSpread, time, Color.gray );
		res[ 1 ] = DrawTrajectory( transform.position, initialSpeed + transform.up * verticalSpread
			- transform.right * horizontalSpread, time, Color.gray );
		res[ 2 ] = DrawTrajectory( transform.position, initialSpeed + transform.up * verticalSpread
			+ transform.right * horizontalSpread, time, Color.gray );
		res[ 3 ] = DrawTrajectory( transform.position, initialSpeed - transform.up * verticalSpread
			+ transform.right * horizontalSpread, time, Color.gray );
		Gizmos.color = Color.white;
		return res;
	}
	private Vector3 DrawTrajectory( Vector3 pos, Vector3 vel, float time, Color color, bool draw =
		true )
	{
		Gizmos.color = color;
		Vector3 nextPos;
		for ( float i = 0.0f; i < time; i += Time.fixedDeltaTime )
		{
			nextPos = pos + Time.fixedDeltaTime * vel;
			if ( draw )
				Gizmos.DrawLine( pos, nextPos );
			pos = nextPos;
			Vector3 accel = bulletAcceleration.x * transform.forward + bulletAcceleration.y *
				transform.up + bulletAcceleration.z * transform.right + bulletGlobalAcceleration;
			vel += accelerationScale * accel;
		}
		return pos;
	}
}
#if UNITY_EDITOR
[CustomEditor( typeof( BcWeapon ) )]
public class BcWeaponEditor : Editor
{
	public SerializedProperty test, bulletPrefab, bulletSpeed, bulletSpeedDelta, bulletLife,
		bulletAcceleration, Offset, displayScale, displayOffset, gizmosFlag, clipSize,
		horizontalSpread, verticalSpread, bulletsPerShot, showDisplayHandles,
		bulletGlobalAcceleration, rateOfFire, reloadTime, isSemiAutomatic, shootSound,
		reloadingSound, reloadedSound, emptyClickSound;
	public bool speedEditMode, lifeEditMode;
	private void OnSceneGUI()
	{
		Vector3 offset, vec;
		BcWeapon tW = ( BcWeapon )target;
		if ( tW.enabled )
		{
			test = this.serializedObject.FindProperty( "test" );
			bulletPrefab = this.serializedObject.FindProperty( "bulletPrefab" );
			horizontalSpread = this.serializedObject.FindProperty( "horizontalSpread" );
			verticalSpread = this.serializedObject.FindProperty( "verticalSpread" );
			displayOffset = this.serializedObject.FindProperty( "displayOffset" );
			bulletAcceleration = this.serializedObject.FindProperty( "bulletAcceleration" );
			bulletSpeed = this.serializedObject.FindProperty( "bulletSpeed" );
			bulletSpeedDelta = this.serializedObject.FindProperty( "bulletSpeedDelta" );
			bulletLife = this.serializedObject.FindProperty( "bulletLife" );
			rateOfFire = this.serializedObject.FindProperty( "rateOfFire" );
			clipSize = this.serializedObject.FindProperty( "clipSize" );
			displayScale = this.serializedObject.FindProperty( "displayScale" );
			bulletsPerShot = this.serializedObject.FindProperty( "bulletsPerShot" );
			showDisplayHandles = this.serializedObject.FindProperty( "showDisplayHandles" );
			bulletGlobalAcceleration = this.serializedObject.FindProperty(
				"bulletGlobalAcceleration" );
			reloadTime = this.serializedObject.FindProperty( "reloadTime" );
			gizmosFlag = this.serializedObject.FindProperty( "gizmosFlag" );
			isSemiAutomatic = this.serializedObject.FindProperty( "isSemiAutomatic" );
			shootSound = this.serializedObject.FindProperty( "shootSound" );
			reloadingSound = this.serializedObject.FindProperty( "reloadingSound" );
			reloadedSound = this.serializedObject.FindProperty( "reloadedSound" );
			emptyClickSound = this.serializedObject.FindProperty( "emptyClickSound" );
			if ( showDisplayHandles.boolValue )
			{
				offset = tW.transform.position;
				vec = -offset + Handles.PositionHandle( offset + tW.DisplayOffsetF, tW.transform.
					rotation );
				displayOffset.vector3Value = tW.transform.worldToLocalMatrix.MultiplyVector( vec );
				displayScale.floatValue = Mathf.Max( 0.1f, Handles.ScaleSlider( displayScale.
					floatValue, offset + tW.DisplayOffsetF, -tW.transform.up, tW.transform.rotation,
					0.75f * HandleUtility.GetHandleSize( offset + tW.DisplayOffsetF ), 0.0f ) );
			}
			Handles.color = Color.white;
			offset = tW.DisplayPosition + tW.transform.up * ( displayScale.floatValue * 0.5f ) + tW.
				transform.forward * ( 0.5f * displayScale.floatValue );
			vec = Handles.Slider( offset + tW.transform.forward * ( tW.clipSize * displayScale.
				floatValue / rateOfFire.floatValue ), tW.transform.forward, 0.75f * displayScale.
				floatValue, Handles.ConeCap, 0.0f ) - offset;
			rateOfFire.floatValue = tW.clipSize * ( displayScale.floatValue / vec.magnitude );
			Handles.color = Color.white;
			offset = tW.DisplayPosition - tW.transform.up * ( displayScale.floatValue * 0.5f ) + tW.
				transform.forward * ( 0.75f * displayScale.floatValue );
			vec = Handles.Slider( offset + tW.transform.forward * ( tW.clipSize * displayScale.
				floatValue / tW.rateOfFire ), tW.transform.forward, 0.75f * displayScale.floatValue,
				Handles.SphereCap, 0.0f ) - offset;
			offset = tW.DisplayPosition - tW.transform.up * ( displayScale.floatValue * 0.5f ) + tW.
				transform.forward * ( 0.5f * displayScale.floatValue );
			vec = Handles.Slider( offset + tW.transform.forward * ( tW.clipSize * displayScale.
				floatValue / tW.rateOfFire ), tW.transform.forward, 0.75f * displayScale.floatValue,
				Handles.CylinderCap, 0.0f ) - offset;
			clipSize.intValue = Mathf.RoundToInt( tW.rateOfFire * vec.magnitude / displayScale.
				floatValue );
			Handles.color = Color.gray;
			offset = tW.DisplayPosition + tW.transform.up * ( displayScale.floatValue * 0.5f ) - tW.
				transform.forward * ( 0.5f * displayScale.floatValue );
			vec = Handles.Slider( offset - tW.transform.forward * ( tW.reloadTime * displayScale.
				floatValue ), -tW.transform.forward, 0.75f * displayScale.floatValue, Handles.
				ConeCap, 0.0f ) - offset;
			reloadTime.floatValue = vec.magnitude / displayScale.floatValue;
			this.serializedObject.ApplyModifiedProperties();
			Handles.color = Color.white;
			Vector3 position = tW.ShotEnd( tW.bulletSpeed * tW.transform.forward, tW.bulletLife,
				Color.white );
			Handles.Label( position, Mathf.Round( 100.0f * tW.bulletLife ) * 0.01f + " secs" + " | "
				+ Mathf.Round( 100.0f * Vector3.Distance( tW.transform.position, position ) ) *
				0.01f + " m" );
		}
	}
}
#endif
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE