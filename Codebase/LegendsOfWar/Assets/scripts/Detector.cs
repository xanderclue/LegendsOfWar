using UnityEngine;
public class Detector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider detectionSphere = null;
    public delegate void triggerEvent(GameObject obj);
    public event triggerEvent TriggerEnter, TriggerExit;
    public void CreateTrigger(float _radius)
    {
        if (!detectionSphere)
            detectionSphere = gameObject.AddComponent<SphereCollider>();
        detectionSphere.isTrigger = true;
        detectionSphere.radius = _radius / transform.parent.lossyScale.x;
    }
    private void Start()
    {
        if (!detectionSphere)
            detectionSphere = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider col)
    {
        TriggerEnter?.Invoke(col.gameObject);
    }
    private void OnTriggerExit(Collider col)
    {
        TriggerExit?.Invoke(col.gameObject);
    }
}