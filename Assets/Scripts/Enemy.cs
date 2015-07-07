using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health;
	public int speed;
	public bool canTakeDamage;
	protected bool canMove = true;
	protected Player player;

	void Start () {
		player = Player.instance;
	}

	public void takeDamage (int damage) {
		if (canTakeDamage) {
			health -= damage;
		}
		canMove = false;
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Ground") || col.GetComponent<Collider2D>().CompareTag ("Tile")) {
			canMove = true;
		}
	}

}
