using UnityEngine;
public class init : MonoBehaviour
{
    [SerializeField]
    private bool illegalStart = false;
    [SerializeField]
    private GameObject audioManager = null;
    private static init inst;
    private static bool appInited = false;
    public static void InitApp()
    {
        if (!appInited)
        {
            Application.targetFrameRate = 90;
            Options.Init();
            appInited = true;
            if (inst)
                if (inst.illegalStart)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            Instantiate(inst.audioManager);
        }
    }
    public void Init()
    {
        InitApp();
    }
    private void Awake()
    {
        inst = this;
        InitApp();
    }
}