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
			if (dis > 0)
				r.velocity = new Vector2 (-speed, GetComponent <Rigidbody2D> ().velocity.y);
			if (dis < 0)
				r.velocity = new Vector2 (speed, GetComponent <Rigidbody2D> ().velocity.y);
			if (dis == 0) {
				r.velocity = new Vector2 (0, GetComponent <Rigidbody2D> ().velocity.y);
				jumping = true;
			}
		}
		if (jumping && grounded) {
			r.AddForce (Vector2.up * jumpForce);
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			grounded = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			grounded = false;
		}
	}
}
