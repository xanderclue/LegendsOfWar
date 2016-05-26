using UnityEngine;
using System.Collections;

public class DetectorAlt : MonoBehaviour {
    [SerializeField]
    CapsuleCollider detectionSphere = null;
    public delegate void triggerEvent(GameObject obj);
    public event triggerEvent triggerEnter, triggerExit, triggerStay;

    void Start()
    {

        if (null == detectionSphere)
            detectionSphere = GetComponent<CapsuleCollider>();
    }

    public void CreateTrigger(float _radius)
    {
        if (detectionSphere == null)
            detectionSphere = gameObject.AddComponent<CapsuleCollider>();
        detectionSphere.radius = _radius / transform.parent.lossyScale.x;
        detectionSphere.height = 20.0f;
        detectionSphere.direction = 3;
        detectionSphere.isTrigger = true;
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Detector: trigger enter");
        if (null != triggerEnter)
        {
            //Debug.Log("Detector: Triger enter event");
            triggerEnter(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (null != triggerExit)
            triggerExit(col.gameObject);
    }

    void OnTriggerStay(Collider _col)
    {
        if (null != triggerStay)
            triggerStay(_col.gameObject);
    }
}
