using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
    protected bool isSupported = true;
    protected virtual bool CheckResources()
    {
        return isSupported;
    }
    protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
    {
        if (s)
            if (s.isSupported)
            {
                if (m2Create && m2Create.shader == s)
                    return m2Create;
                m2Create = new Material(s);
                m2Create.hideFlags = HideFlags.DontSave;
                if (m2Create)
                    return m2Create;
            }
            else
                NotSupported();
        else
            enabled = false;
        return null;
    }
    protected void CheckSupport()
    {
        isSupported = true;
        if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            NotSupported();
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }
    private void Start()
    {
        CheckResources();
    }
    private void OnEnable()
    {
        isSupported = true;
    }
    private void NotSupported()
    {
        enabled = false;
        isSupported = false;
    }
}