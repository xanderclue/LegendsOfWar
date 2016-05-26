using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
	Vector3 mouseDelta;
	Vector3 lastPosition;

	void Update () {
		
		if (Input.GetMouseButton(0)) {
			if (lastPosition==Vector3.zero) {
				lastPosition=Input.mousePosition;
			}
			mouseDelta=Input.mousePosition-lastPosition;
			lastPosition=Input.mousePosition;
			transform.LookAt(transform.position+transform.forward*100+mouseDelta.x*transform.right+mouseDelta.y*transform.up, Vector3.up);
		} else {
			lastPosition=Vector3.zero;
		}
	}
}