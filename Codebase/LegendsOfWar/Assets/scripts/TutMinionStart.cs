using UnityEngine;
public class TutMinionStart : MonoBehaviour
{
    [SerializeField]
    private GameObject MininionStart = null;
    private void OnTriggerEnter()
    {
        MininionStart.SetActive(true);
        gameObject.SetActive(false);
    }
}