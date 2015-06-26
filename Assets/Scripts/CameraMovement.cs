using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject followObject;

	void Update () {
		Vector3 position = followObject.transform.position;
		transform.position = new Vector3 (position.x, position.y, -10f);
	}
}
