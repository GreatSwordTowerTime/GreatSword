using UnityEngine;
using System.Collections;

public class Sword : Item {

	public float speed;

	private bool rotating;

	public float hitForce;

	void FixedUpdate () {
		if(Input.GetButtonDown ("Fire1")) {
			rotating = true;
			GetComponent <SpriteRenderer> ().enabled = true;
			GetComponent <BoxCollider2D> ().enabled = true;
		}
		if (rotating) {
			transform.Rotate (new Vector3 (0, 0, -1 * speed));
			if (transform.localEulerAngles.z > 200f && transform.localEulerAngles.z < 205f) {
				rotating = false;
				GetComponent <SpriteRenderer> ().enabled = false;
				GetComponent <BoxCollider2D> ().enabled = false;
				transform.localEulerAngles = new Vector3 (0, 0, 50);
			}
		}

	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Enemy")) {
			col.GetComponent <Enemy> ().takeDamage (damage);
			col.GetComponent <Rigidbody2D> ().velocity = Vector2.zero;
			col.GetComponent <Rigidbody2D> ().AddForce (Player.instance.right ? Vector2.right * hitForce + Vector2.up * hitForce : -Vector2.right * hitForce + Vector2.up * hitForce);
		}
	}

}