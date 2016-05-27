using UnityEngine;
namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent( typeof( Camera ) )]
	public class PostEffectsBase : MonoBehaviour
	{
		protected bool supportHDRTextures = true;
		protected bool supportDX11 = false;
		protected bool isSupported = true;
		protected Material CheckShaderAndCreateMaterial( Shader s, Material m2Create )
		{
			if ( !s )
			{
				enabled = false;
				return null;
			}
			if ( s.isSupported && m2Create && m2Create.shader == s )
				return m2Create;
			if ( !s.isSupported )
			{
				NotSupported();
				return null;
			}
			else
			{
				m2Create = new Material( s );
				m2Create.hideFlags = HideFlags.DontSave;
				if ( m2Create )
					return m2Create;
				else
					return null;
			}
		}
		protected Material CreateMaterial( Shader s, Material m2Create )
		{
			if ( !s )
				return null;
			if ( m2Create && ( m2Create.shader == s ) && s.isSupported )
				return m2Create;
			if ( !s.isSupported )
				return null;
			else
			{
				m2Create = new Material( s );
				m2Create.hideFlags = HideFlags.DontSave;
				if ( m2Create )
					return m2Create;
				else
					return null;
			}
		}
		void OnEnable()
		{
			isSupported = true;
		}
		protected bool CheckSupport()
		{
			return CheckSupport( false );
		}
		public virtual bool CheckResources()
		{
			return isSupported;
		}
		protected void Start()
		{
			CheckResources();
		}
		protected bool CheckSupport( bool needDepth )
		{
			isSupported = true;
			supportHDRTextures = SystemInfo.SupportsRenderTextureFormat( RenderTextureFormat.
				ARGBHalf );
			supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			if ( !SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures )
			{
				NotSupported();
				return false;
			}
			if ( needDepth && !SystemInfo.SupportsRenderTextureFormat( RenderTextureFormat.Depth ) )
			{
				NotSupported();
				return false;
			}
			if ( needDepth )
				GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			return true;
		}
		protected bool CheckSupport( bool needDepth, bool needHdr )
		{
			if ( !CheckSupport( needDepth ) )
				return false;
			if ( needHdr && !supportHDRTextures )
			{
				NotSupported();
				return false;
			}
			return true;
		}
		public bool Dx11Support()
		{
			return supportDX11;
		}
		bool CheckShader( Shader s )
		{
			if ( !s.isSupported )
				NotSupported();
			return false;
		}
		protected void NotSupported()
		{
			enabled = false;
			isSupported = false;
		}
		protected void DrawBorder( RenderTexture dest, Material material )
		{
			float x2x1y2y1;
			RenderTexture.active = dest;
			GL.PushMatrix();
			GL.LoadOrtho();
			for ( int i = 0; i < material.passCount; ++i )
			{
				material.SetPass( i );
				GL.Begin( GL.QUADS );
				GL.TexCoord2( 0.0f, 1.0f );
				GL.Vertex3( 0.0f, 0.0f, 0.1f );
				GL.TexCoord2( 1.0f, 1.0f );
				GL.Vertex3( x2x1y2y1 = 1.0f / dest.width, 0.0f, 0.1f );
				GL.TexCoord2( 1.0f, 0.0f );
				GL.Vertex3( x2x1y2y1, 1.0f, 0.1f );
				GL.TexCoord2( 0.0f, 0.0f );
				GL.Vertex3( 0.0f, 1.0f, 0.1f );
				GL.TexCoord2( 0.0f, 1.0f );
				GL.Vertex3( x2x1y2y1 = 1.0f - x2x1y2y1, 0.0f, 0.1f );
				GL.TexCoord2( 1.0f, 1.0f );
				GL.Vertex3( 1.0f, 0.0f, 0.1f );
				GL.TexCoord2( 1.0f, 0.0f );
				GL.Vertex3( 1.0f, 1.0f, 0.1f );
				GL.TexCoord2( 0.0f, 0.0f );
				GL.Vertex3( x2x1y2y1, 1.0f, 0.1f );
				GL.TexCoord2( 0.0f, 1.0f );
				GL.Vertex3( 0.0f, 0.0f, 0.1f );
				GL.TexCoord2( 1.0f, 1.0f );
				GL.Vertex3( 1.0f, 0.0f, 0.1f );
				GL.TexCoord2( 1.0f, 0.0f );
				GL.Vertex3( 1.0f, x2x1y2y1 = 1.0f / dest.height, 0.1f );
				GL.TexCoord2( 0.0f, 0.0f );
				GL.Vertex3( 0.0f, x2x1y2y1, 0.1f );
				GL.TexCoord2( 0.0f, 1.0f );
				GL.Vertex3( 0.0f, x2x1y2y1 = 1.0f - x2x1y2y1, 0.1f );
				GL.TexCoord2( 1.0f, 1.0f );
				GL.Vertex3( 1.0f, x2x1y2y1, 0.1f );
				GL.TexCoord2( 1.0f, 0.0f );
				GL.Vertex3( 1.0f, 1.0f, 0.1f );
				GL.TexCoord2( 0.0f, 0.0f );
				GL.Vertex3( 0.0f, 1.0f, 0.1f );
				GL.End();
			}
			GL.PopMatrix();
		}
	}
}