using UnityEngine;
using System.Collections;

public class SwingSword : MonoBehaviour {

	public GameObject sword;

	public float speed;

	private bool rotating;

	void FixedUpdate () {
		if(Input.GetButtonDown ("Fire1")) {
			rotating = true;
			sword.SetActive (true);
		}
		if (rotating) {
			sword.transform.Rotate (new Vector3 (0, 0, -1 * speed));
			if (sword.transform.localEulerAngles.z > 200f && sword.transform.localEulerAngles.z < 205f) {
				rotating = false;
				sword.SetActive (false);
				sword.transform.eulerAngles = new Vector3 (0, 0, 50);
			}
		}

	}
}