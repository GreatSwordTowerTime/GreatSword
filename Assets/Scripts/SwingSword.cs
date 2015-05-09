using UnityEngine;
using System.Collections;

public class SwingSword : MonoBehaviour {

	void Update () {
		if(Input.GetButtonDown ("Fire1")) {
			GameObject sword = GameObject.FindWithTag ("Sword");
			sword.GetComponent <Animator> ().SetBool ("isSwinging", true);
		}
	}
}
