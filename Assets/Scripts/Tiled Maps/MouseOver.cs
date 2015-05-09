using UnityEngine;
using System.Collections;


[RequireComponent (typeof (TiledMapGeneration))]
public class MouseOver : MonoBehaviour {

	TiledMapGeneration map;

	int x;
	int y;

	void Start () {
		map = GetComponent <TiledMapGeneration> ();
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rc;

		if(GetComponent <Collider> ().Raycast (ray, out rc, Mathf.Infinity)) {
			x = Mathf.FloorToInt (rc.point.x / map.tileSize);
			y = Mathf.FloorToInt (rc.point.y / map.tileSize);
		}
	}

	public Vector2 getMouseCoor () {
		return new Vector2 (x, y);
	}
}
