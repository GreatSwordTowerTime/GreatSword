using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health;
	public int speed;
	public bool canTakeDamage;
	public bool takingDamage { get; private set; }
	protected Player player;

	void Start () {
		player = Player.instance;
	}

	public void takeDamage (int damage) {
		if (canTakeDamage) {
			health -= damage;
		}
		takingDamage = true;
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.CompareTag ("Ground") || col.GetComponent <Collider2D> ().CompareTag ("Tile")) {
			takingDamage = false;
		}
	}

}
