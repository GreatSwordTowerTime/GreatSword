using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	//how fast the player can go
	public float speed;
	//how much the player increases speed when running / pressing shift
	public float multiplier;
	//how high the player jumps
	public float jumpForce;
	//how long the jump lasts
	public float jumpTime;
	//in what direction the player moves
	private bool isGrounded;
	private bool jumping;
	private float timer = 0;
	private bool letGo;

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Run") == 0) {
			if (Input.GetAxis ("Horizontal") > 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, GetComponent<Rigidbody2D>().velocity.y);
			}
			if (Input.GetAxis ("Horizontal") < 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D>().velocity.y);
			}
			if (Input.GetAxis ("Horizontal") == 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x/1.5f, GetComponent<Rigidbody2D>().velocity.y);
			}
		}
		if(Input.GetAxis ("Run") > 0) {
			if (Input.GetAxis ("Horizontal") > 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (speed * multiplier, GetComponent<Rigidbody2D>().velocity.y);
			}
			if (Input.GetAxis ("Horizontal") < 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (-speed * multiplier, GetComponent<Rigidbody2D>().velocity.y);
			}
			if (Input.GetAxis ("Horizontal") == 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x/2, GetComponent<Rigidbody2D>().velocity.y);
			}
		}
		if (Input.GetAxis ("Jump") > 0 & timer <= jumpTime &isGrounded  &!letGo) {
			jumping = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, jumpForce);
			timer += Time.deltaTime;
		}
		else if (Input.GetAxis ("Jump") == 0 & jumping) {
			letGo = true;
		}

	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			//reset all variables
			jumping = false;
			isGrounded = true;
			//doubleJump = false;
			letGo = false;
			//dash = false;
			//dashTimer = 0;
			timer = 0;
			//doubleJumpTimer = 0;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") & !jumping) {
			isGrounded = false;
			letGo = true;

		}
	}
}
