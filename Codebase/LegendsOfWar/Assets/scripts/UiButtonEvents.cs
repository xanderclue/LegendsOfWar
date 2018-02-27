using UnityEngine;
using UnityEngine.EventSystems;
public class UiButtonEvents : MonoBehaviour
{
    public void SetSelectedThis()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}