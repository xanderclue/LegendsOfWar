using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public Texture2D SelectionHighlight = null;
	public static Rect Selection = new Rect( 0, 0, 0, 0 );

	private Vector3 StartClick = -Vector3.one;

	private Vector3 Origin;
	private Vector3 Difference;

	static CameraControl inst = null;
	public static CameraControl instance { get { return inst; } }
	void Awake()
	{
		inst = this;
	}

	[SerializeField]
	private Camera mainCam = null;
	[SerializeField]
	private Camera vantageCam = null;
	[SerializeField]
	private Camera minimapCam = null;
	[SerializeField]
	private SpriteRenderer camBorder = null;
	[SerializeField]
	private float zoomSpeed = 1.0f, moveSpeed = 1.0f;
	[SerializeField]
	private float worldMaxX = 1200.0f, worldMinX = 0.0f,
	worldMaxZ = 700.0f, worldMinZ = 0.0f;
	private float maxXPos = 1.0f, minXPos = -1.0f,
		maxZPos = 1.0f, minZPos = -1.0f,
		maxZoomSize = 1.0f;
	private float aspectRatio = 0.0f;
	public GameObject player;
	private bool followPlayer = false;
	static Camera main, vantage, current;
	public static Camera Main { get { return main; } }
	public static Camera Vantage { get { return vantage; } }
	public static Camera Current
	{
		get
		{
			return HeroCamScript.onHero ? HeroCamScript.HeroCam : current;
		}
	}

	void Start()
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
	void OnDestroy()
	{
		HeroCamScript.OnOnHero -= OnOnHero;
		GameManager.OnBlueWin -= OnBlueWin;
		GameManager.OnRedWin -= OnRedWin;
	}

	public bool CameraFollowsPlayer
	{
		get
		{
			return followPlayer;
		}
		set
		{
			if ( !playerInfo.Alive || null == playerInfo )
				followPlayer = false;
			else
				followPlayer = value;
		}
	}

	void OnRedWin()
	{
		mainCam.transform.position = new Vector3( 200.0f, 500.0f, 333.0f );
	}
	void OnBlueWin()
	{
		mainCam.transform.position = new Vector3( 1000.0f, 500.0f, 333.0f );
	}

	float zoomTemp;
	RaycastHit hit;
	void Update()
	{

		if ( !GameManager.GameRunning )
			return;
		CheckCamera();

		followPlayer = HeroCamScript.onHero;
		if ( aspectRatio != mainCam.aspect )
			RecalcZoomLimits();
		if ( Input.GetMouseButton( 0 ) )
		{
			if ( Physics.Raycast( minimapCam.ScreenPointToRay( Input.mousePosition ), out hit ) )
			{
				mainCam.transform.position = new Vector3( hit.point.x,
					mainCam.transform.position.y, hit.point.z );
				followPlayer = false;
			}
		}
		if ( Input.GetKeyDown( KeyCode.V ) )
			ToggleCam();
		if ( mainCam.enabled )
		{
			if ( !followPlayer )
				MoveCam( Input.GetAxis( "Horizontal" ) * moveSpeed * mainCam.orthographicSize * 0.01f,
					Input.GetAxis( "Vertical" ) * moveSpeed * mainCam.orthographicSize * 0.01f );
			zoomTemp = Input.GetAxis( "Mouse ScrollWheel" ) + Input.GetAxis( "-=" );
			if ( 0.0f != zoomTemp )
				ZoomCam( -zoomTemp * zoomSpeed );
		}


	}

	private void CheckCamera()
	{
		if ( Input.GetMouseButtonDown( 0 ) && HeroCamScript.onHero == false )
			StartClick = Input.mousePosition;
		if ( Input.GetMouseButtonUp( 0 ) )
		{
			StartClick = -Vector3.one;
		}
		if ( Input.GetMouseButton( 0 ) )
		{
			Selection = new Rect( StartClick.x, Screen.height - StartClick.y,
				Input.mousePosition.x - StartClick.x, StartClick.y - Input.mousePosition.y );

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

	readonly Color guiCol = new Color( 1.0f, 1.0f, 1.0f, 0.5f );
	private void OnGUI()
	{
		if ( StartClick != -Vector3.one )
		{
			GUI.color = guiCol;
			GUI.DrawTexture( Selection, SelectionHighlight );
		}
	}

	float mousePosX;
	float mousePosY;
	const float scrollDistance = 2.5f;
	Vector3 newPos;
	void LateUpdate()
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
				newPos.x = Mathf.Clamp( mainCam.transform.position.x - Origin.x + Difference.x, minXPos, maxXPos );
				newPos.y = mainCam.transform.position.y;
				newPos.z = Mathf.Clamp( mainCam.transform.position.z - Origin.y + Difference.y, minZPos, maxZPos );
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
	Info playerInfo;
	Rect minimapviewport = new Rect( 0.7f, 0.0f, 0.3f, 0.3111f );
	private void RecalcZoomLimits()
	{
		aspectRatio = mainCam.aspect;
		maxZoomSize = Mathf.Min( ( worldMaxX - worldMinX ) / aspectRatio,
			worldMaxZ - worldMinZ ) * 0.3f;
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
		mainCam.transform.position = new Vector3(
				Mathf.Clamp( mainCam.transform.position.x, minXPos, maxXPos ),
				mainCam.transform.position.y,
				Mathf.Clamp( mainCam.transform.position.z, minZPos, maxZPos ) );
	}

	private void MoveCam( float dx, float dz )
	{
		if ( GameManager.GameRunning )
			mainCam.transform.position = new Vector3(
				Mathf.Clamp( mainCam.transform.position.x + dx, minXPos, maxXPos ),
				mainCam.transform.position.y,
				Mathf.Clamp( mainCam.transform.position.z + dz, minZPos, maxZPos ) );
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

	void ResetMinimapCam()
	{
		minimapCam.enabled = false;
		minimapCam.enabled = true;
	}

	public void ToggleCam()
	{
		if ( vantageCam.enabled )
			SwitchToMainCam();
		else
			SwitchToVantageCam();
		ResetMinimapCam();
	}

	private void ZoomCam( float deltaSize )
	{
		if ( HeroCamScript.onHero )
			return;
		mainCam.orthographicSize =
			Mathf.Clamp( mainCam.orthographicSize + deltaSize, 50.0f, maxZoomSize );
		camBorder.transform.localScale = new Vector3( 0.002086875f * mainCam.orthographicSize *
			mainCam.aspect, 0.00371f * mainCam.orthographicSize, 1.0f );
		mainCam.fieldOfView = tworad *
			Mathf.Atan2( mainCam.orthographicSize, mainCam.transform.position.y );
		RecalcBoundaries();
	}

	const float tworad = 2.0f * Mathf.Rad2Deg;
	static readonly float onHeroFov = tworad * Mathf.Atan2( 100.0f, 500.0f );
	void OnOnHero()
	{
		if ( !mainCam )
			return;
		mainCam.orthographicSize = 100.0f;
		camBorder.transform.localScale = new Vector3( 0.2086875f * mainCam.aspect, 0.371f, 1.0f );
		mainCam.fieldOfView = onHeroFov;
		RecalcBoundaries();
	}

	public float ZoomSpeed
	{
		get { return zoomSpeed; }
		set { zoomSpeed = value; }
	}

	public float MoveSpeed
	{
		get { return moveSpeed; }
		set { moveSpeed = value; }
	}

	static public float AudioDistance
	{
		get
		{
			if ( instance && instance.mainCam )
				return instance.mainCam.orthographicSize * instance.mainCam.aspect;
			else
				return 200.0f;
		}
	}

}