using UnityEngine;
using System.Collections;

public class FollowingEnemy : Enemy {

	public float distance;
	public float disFromPlayer { get {return (transform.position - player.position).x;}}
	public float jumpForce;
	public bool jumping { get; private set; }
	private bool grounded;

	void Update () {
		var r = GetComponent <Rigidbody2D> ();
		if (Mathf.Abs (disFromPlayer) < distance && !takingDamage) {
			if (disFromPlayer > 0) {
				r.velocity = new Vector2 (-speed, GetComponent <Rigidbody2D> ().velocity.y);
				transform.eulerAngles = new Vector3 (0, 180, 0);
			}
			if (disFromPlayer < 0) {
				r.velocity = new Vector2 (speed, GetComponent <Rigidbody2D> ().velocity.y);
				transform.eulerAngles = new Vector3 (0, 0, 0);
			}
			if (Mathf.Abs (disFromPlayer) < 0.1f) {
				r.velocity = new Vector2 (0, GetComponent <Rigidbody2D> ().velocity.y);
				jumping = true;
			}
		}

		if (jumping && grounded) {
			r.AddForce (Vector2.up * jumpForce);
		}
		GetComponent <Animator> ().SetBool ("walking", Mathf.Abs (disFromPlayer) < distance && !takingDamage);
		GetComponent <Animator> ().SetBool ("jumping", jumping);
		if (health <= 0) {
			GetComponent <Animator> ().SetBool ("dead", true);
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			grounded = true;
			jumping = false;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			grounded = false;
		}
	}
}
