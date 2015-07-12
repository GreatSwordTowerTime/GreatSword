using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player instance;
	public Vector3 position {
		get {return transform.position;}
	}
	//how fast the player can go
	public float speed;
	//how much the player increases speed when running / pressing shift
	public float multiplier;
	//how high the player jumps
	public float jumpForce;
	//how long the jump lasts
	public float jumpTime;
	public bool right { get; private set; }
	//in what direction the player moves
	private bool isGrounded;
	private bool jumping;
	private float timer = 0;
	private bool letGo =false;

	void Awake () {
		instance = this;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Run") == 0) {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (speed * Input.GetAxis ("Horizontal"), GetComponent<Rigidbody2D>().velocity.y);
		}
		if(Input.GetAxis ("Run") > 0) {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (speed * Input.GetAxis ("Horizontal") * multiplier, GetComponent<Rigidbody2D>().velocity.y);
		}
		if (Input.GetAxis ("Jump") > 0 && isGrounded && !letGo) {
			jumping = true;
		}
		if (jumping && timer <= jumpTime && !letGo) {
			timer += Time.deltaTime;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, jumpForce);
		}


		if (Input.GetAxis ("Jump") == 0 && !isGrounded && !letGo) {
			letGo = true;

			if (timer <= jumpTime) {
				GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y/1.5f);
				timer = jumpTime + 1;
			}
		}

	}

	void Update () {
		if (Input.GetAxis ("Horizontal") > 0)
			right = true;
		if (Input.GetAxis ("Horizontal") < 0)
			right = false;

		if (right) {
			transform.eulerAngles = new Vector3 (0, 0, 0);

		} else {
			transform.eulerAngles = new Vector3 (0, 180, 0);
		}

		if (!(Input.GetAxis ("Horizontal") == 0)) {
			GetComponent <Animator> ().SetBool ("running", true);
		} else {
			GetComponent <Animator> ().SetBool ("running", false);
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
		if (col.GetComponent<Collider2D>().CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			isGrounded = false;
			//letGo = true;

		}
	}
}
