using UnityEngine;
using System.Collections;

public class MeleeParticleEffect : MonoBehaviour {
    [SerializeField]
    ParticleSystem effect = null;
    [SerializeField]
    GameObject Weapon = null;
    Mesh Mesh = null;
	// Use this for initialization
	void Awake () {
        if (Weapon == null)
            Weapon = gameObject.transform.parent.gameObject;
        if (Mesh == null)
            Mesh = Weapon.GetComponent<MeshFilter>().mesh;
        if(effect == null)
            effect = gameObject.GetComponent<ParticleSystem>();
        effect.gameObject.GetComponent<ParticleSystemRenderer>().mesh = Weapon.GetComponent<MeshFilter>().mesh;
        effect.gameObject.GetComponent<ParticleSystemRenderer>().material = Weapon.GetComponent<MeshRenderer>().material;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        effect.startRotation3D = Weapon.transform.rotation.eulerAngles * 0.0174533f;
	}
}
