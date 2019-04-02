using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterViewer : MonoBehaviour {
	
	public Transform cameras;

	Transform targetForCamera;
	Vector3 deltaPosition;
	Vector3 lastPosition = Vector3.zero;
	bool rotating = false;

	void Awake () {
		targetForCamera = GameObject.Find ("RigSpine3").transform;
		deltaPosition = cameras.position - targetForCamera.position;
	}
}
