using UnityEngine;
using System.Collections;

public class FollowingEnemy : Enemy {

	public float distance;
	public float jumpForce;
	private bool jumping;
	private bool grounded;

	void Update () {
		float dis = (transform.position - player.position).x;
		var r = GetComponent <Rigidbody2D> ();
		if (Mathf.Abs (dis) < distance && canMove) {
			if (dis > 0) {
				r.velocity = new Vector2 (-speed, GetComponent <Rigidbody2D> ().velocity.y);
				transform.eulerAngles = new Vector3 (0, 180, 0);
			}
			if (dis < 0) {
				r.velocity = new Vector2 (speed, GetComponent <Rigidbody2D> ().velocity.y);
				transform.eulerAngles = new Vector3 (0, 0, 0);
			}
			if (dis == 0) {
				r.velocity = new Vector2 (0, GetComponent <Rigidbody2D> ().velocity.y);
				jumping = true;
				GetComponent <Animator> ().SetBool ("walking", false);
			}
			GetComponent <Animator> ().SetBool ("walking", true);
		} else {
			GetComponent <Animator> ().SetBool ("walking", false);
		}
		if (jumping && grounded) {
			r.AddForce (Vector2.up * jumpForce);
		}
		if (jumping) {
			GetComponent <Animator> ().SetBool ("jumping", true);
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			grounded = true;
			GetComponent <Animator> ().SetBool ("jumping", false);
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			grounded = false;
		}
	}
}
