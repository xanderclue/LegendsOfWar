using UnityEngine;
public class CameraControl : MonoBehaviour
{
	[SerializeField]
	private Texture2D SelectionHighlight = null;
	[SerializeField]
	private Camera mainCam = null, vantageCam = null, minimapCam = null;
	[SerializeField]
	private SpriteRenderer camBorder = null;
	[SerializeField]
	private float zoomSpeed = 1.0f, moveSpeed = 1.0f, worldMaxX = 1200.0f, worldMinX = 0.0f,
		worldMaxZ = 700.0f, worldMinZ = 0.0f;
	[SerializeField]
	private GameObject player;
	private const float scrollDistance = 2.5f;
	private static readonly Color guiCol = new Color( 1.0f, 1.0f, 1.0f, 0.5f );
	private static readonly float onHeroFov = 114.591559026f * Mathf.Atan2( 100.0f, 500.0f );
	public static Rect Selection = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
	private static CameraControl inst = null;
	private static Camera main, vantage, current;
	private Info playerInfo;
	private RaycastHit hit;
	private Rect minimapviewport = new Rect( 0.7f, 0.0f, 0.3f, 0.3111111f );
	private Vector3 StartClick = -Vector3.one, Origin, Difference, newPos = new Vector3();
	private float maxXPos = 1.0f, minXPos = -1.0f, maxZPos = 1.0f, minZPos = -1.0f, maxZoomSize =
		1.0f, aspectRatio = 0.0f, zoomTemp, mousePosX, mousePosY;
	private bool followPlayer = false;
	public static CameraControl instance
	{ get { return inst; } }
	public static Camera Vantage
	{ get { return vantage; } }
	public static Camera Current
	{ get { return HeroCamScript.onHero ? HeroCamScript.HeroCam : current; } }
	public static float AudioDistance
	{
		get
		{
			if ( inst && inst.mainCam )
				return inst.mainCam.orthographicSize * inst.mainCam.aspect;
			else
				return 200.0f;
		}
	}
	public bool CameraFollowsPlayer
	{ get { return followPlayer; } }
	public void ToggleCam()
	{
		if ( vantageCam.enabled )
			SwitchToMainCam();
		else
			SwitchToVantageCam();
		minimapCam.enabled = false;
		minimapCam.enabled = true;
	}
	public void SwitchToMainCam()
	{
		current = mainCam;
		if ( !HeroCamScript.onHero )
			mainCam.enabled = true;
		vantageCam.enabled = false;
		camBorder.enabled = true;
	}
	public void SwitchToVantageCam()
	{
		current = vantageCam;
		vantageCam.enabled = true;
		mainCam.enabled = false;
		camBorder.enabled = false;
	}
	private void Awake()
	{
		inst = this;
	}
	private void Start()
	{
		player = GameManager.Instance.Player;
		playerInfo = player.GetComponent<Info>();
		RecalcZoomLimits();
		current = mainCam;
		main = mainCam;
		vantage = vantageCam;
		HeroCamScript.OnOnHero += OnOnHero;
		GameManager.OnBlueWin += OnBlueWin;
		GameManager.OnRedWin += OnRedWin;
	}
	private void Update()
	{
		if ( !GameManager.GameRunning )
			return;
		CheckCamera();
		followPlayer = HeroCamScript.onHero;
		if ( aspectRatio != mainCam.aspect )
			RecalcZoomLimits();
		if ( Input.GetMouseButton( 0 ) )
			if ( Physics.Raycast( minimapCam.ScreenPointToRay( Input.mousePosition ), out hit ) )
			{
				mainCam.transform.position = new Vector3( hit.point.x, mainCam.transform.position.y,
					hit.point.z );
				followPlayer = false;
			}
		if ( Input.GetKeyDown( KeyCode.V ) )
			ToggleCam();
		if ( mainCam.enabled )
		{
			if ( !followPlayer )
				MoveCam( Input.GetAxis( "Horizontal" ) * moveSpeed * mainCam.orthographicSize *
					0.01f, Input.GetAxis( "Vertical" ) * moveSpeed * mainCam.orthographicSize *
					0.01f );
			zoomTemp = Input.GetAxis( "Mouse ScrollWheel" ) + Input.GetAxis( "-=" );
			if ( 0.0f != zoomTemp )
				ZoomCam( -zoomTemp * zoomSpeed );
		}
	}
	private void LateUpdate()
	{
		if ( !GameManager.GameRunning )
			return;
		if ( mainCam.enabled )
		{
			if ( Input.GetMouseButtonDown( 2 ) )
				Origin = Input.mousePosition * 0.05f;
			if ( Input.GetMouseButton( 2 ) )
			{
				Difference = Input.mousePosition * 0.05f;
				newPos.x = Mathf.Clamp( mainCam.transform.position.x - Origin.x + Difference.x,
					minXPos, maxXPos );
				newPos.y = mainCam.transform.position.y;
				newPos.z = Mathf.Clamp( mainCam.transform.position.z - Origin.y + Difference.y,
					minZPos, maxZPos );
				mainCam.transform.position = newPos;
			}
		}
		if ( mainCam.enabled && !followPlayer )
		{
			mousePosX = Input.mousePosition.x;
			mousePosY = Input.mousePosition.y;
			if ( mousePosX < scrollDistance )
				MoveCam( -moveSpeed * mainCam.orthographicSize * 0.01f, 0.0f );
			if ( mousePosX >= Screen.width - scrollDistance )
				MoveCam( moveSpeed * mainCam.orthographicSize * 0.01f, 0.0f );
			if ( mousePosY < scrollDistance )
				MoveCam( 0.0f, -moveSpeed * mainCam.orthographicSize * 0.01f );
			if ( mousePosY >= Screen.height - scrollDistance )
				MoveCam( 0.0f, moveSpeed * mainCam.orthographicSize * 0.01f );
		}
		if ( followPlayer && HeroCamScript.onHero )
		{
			if ( playerInfo && playerInfo.Alive )
			{
				newPos.x = Mathf.Clamp( player.transform.position.x, minXPos, maxXPos );
				newPos.y = mainCam.transform.position.y;
				newPos.z = Mathf.Clamp( player.transform.position.z, minZPos, maxZPos );
				mainCam.transform.position = newPos;
			}
			else
				followPlayer = false;
		}
	}
	private void OnGUI()
	{
		if ( -Vector3.one != StartClick )
		{
			GUI.color = guiCol;
			GUI.DrawTexture( Selection, SelectionHighlight );
		}
	}
	private void OnDestroy()
	{
		HeroCamScript.OnOnHero -= OnOnHero;
		GameManager.OnBlueWin -= OnBlueWin;
		GameManager.OnRedWin -= OnRedWin;
	}
	private void OnRedWin()
	{
		mainCam.transform.position = new Vector3( 200.0f, 500.0f, 333.0f );
	}
	private void OnBlueWin()
	{
		mainCam.transform.position = new Vector3( 1000.0f, 500.0f, 333.0f );
	}
	private void OnOnHero()
	{
		if ( !mainCam )
			return;
		mainCam.orthographicSize = 100.0f;
		camBorder.transform.localScale = new Vector3( 0.2086875f * mainCam.aspect, 0.371f, 1.0f );
		mainCam.fieldOfView = onHeroFov;
		RecalcBoundaries();
	}
	private void CheckCamera()
	{
		if ( Input.GetMouseButtonDown( 0 ) && !HeroCamScript.onHero )
			StartClick = Input.mousePosition;
		if ( Input.GetMouseButtonUp( 0 ) )
			StartClick = -Vector3.one;
		if ( Input.GetMouseButton( 0 ) )
		{
			Selection = new Rect( StartClick.x, Screen.height - StartClick.y, Input.mousePosition.x
				- StartClick.x, StartClick.y - Input.mousePosition.y );
			if ( Selection.width < 0.0f )
			{
				Selection.x += Selection.width;
				Selection.width = -Selection.width;
			}
			if ( Selection.height < 0.0f )
			{
				Selection.y += Selection.height;
				Selection.height = -Selection.height;
			}
		}
	}
	private void RecalcZoomLimits()
	{
		aspectRatio = mainCam.aspect;
		maxZoomSize = Mathf.Min( ( worldMaxX - worldMinX ) / aspectRatio, worldMaxZ - worldMinZ ) *
			0.3f;
		minimapviewport.height = 0.175f * aspectRatio;
		minimapCam.rect = minimapviewport;
		ZoomCam( 0.0f );
	}
	private void RecalcBoundaries()
	{
		minZPos = worldMinZ + mainCam.orthographicSize;
		maxZPos = worldMaxZ - mainCam.orthographicSize;
		minXPos = worldMinX + aspectRatio * mainCam.orthographicSize;
		maxXPos = worldMaxX - aspectRatio * mainCam.orthographicSize;
		mainCam.transform.position = new Vector3( Mathf.Clamp( mainCam.transform.position.x, minXPos
			, maxXPos ), mainCam.transform.position.y, Mathf.Clamp( mainCam.transform.position.z,
			minZPos, maxZPos ) );
	}
	private void MoveCam( float dx, float dz )
	{
		if ( GameManager.GameRunning )
			mainCam.transform.position = new Vector3( Mathf.Clamp( mainCam.transform.position.x + dx
				, minXPos, maxXPos ), mainCam.transform.position.y, Mathf.Clamp( mainCam.transform.
				position.z + dz, minZPos, maxZPos ) );
	}
	private void ZoomCam( float deltaSize )
	{
		if ( HeroCamScript.onHero )
			return;
		mainCam.orthographicSize = Mathf.Clamp( mainCam.orthographicSize + deltaSize, 50.0f,
			maxZoomSize );
		camBorder.transform.localScale = new Vector3( 0.002086875f * mainCam.orthographicSize *
			mainCam.aspect, 0.00371f * mainCam.orthographicSize, 1.0f );
		mainCam.fieldOfView = 114.591559026f * Mathf.Atan2( mainCam.orthographicSize, mainCam.
			transform.position.y );
		RecalcBoundaries();
	}
}