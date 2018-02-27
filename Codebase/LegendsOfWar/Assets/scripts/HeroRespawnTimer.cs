using UnityEngine;
using UnityEngine.UI;
public class HeroRespawnTimer : MonoBehaviour
{
    [SerializeField]
    private Text text = null;
    private HeroInfo info;
    private void Start()
    {
        info = GameManager.Instance.Player.GetComponent<HeroInfo>();
        info.Destroyed += ShowTimer;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (info.RespawnTimer <= 0.0f)
            gameObject.SetActive(false);
        else
            text.text = (Options.Japanese ? "生変：" : "Respawn: ") + info.RespawnTimer.ToString(
                "F2");
    }
    private void ShowTimer()
    {
        text.text = (Options.Japanese ? "生変：" : "Respawn: ") + info.RespawnTimer.ToString("F2"
            );
        gameObject.SetActive(true);
    }
}