﻿using UnityEngine;
using System.Collections;

public enum TILE : int {
	STONE,
	MOSSYSTONE,
	BLANK,
	STONEBG,
	MOSSYBG,
	BURNEDMOSSY,
	BURNEDSTONE
}

public class Tile {


	public TILE type;

	public int[] texCoor;
	public bool hasCollider;

	public Tile (TILE type) {
		this.type = type;

		if (type == TILE.STONE) {
			texCoor = new int[] {1, 0};
			hasCollider = true;
		}
		if (type == TILE.MOSSYSTONE) {
			texCoor = new int[] {0, 0};
			hasCollider = true;
		}
		if (type == TILE.BLANK) {
			texCoor = new int[] {63, 63};
			hasCollider = false;
		}
		if (type == TILE.STONEBG) {
			texCoor = new int[] {1, 1};
			hasCollider = false;
		}
		if (type == TILE.MOSSYBG) {
			texCoor = new int[] {0, 1};
			hasCollider = false;
		}
		if (type == TILE.BURNEDMOSSY) {
			texCoor = new int[] {0, 2};
			hasCollider = true;
		}
		if (type == TILE.BURNEDSTONE) {
			texCoor = new int[] {1, 2};
			hasCollider = true;
		}
	}

	public void changeType (TILE type) {
		this.type = type;

		if (type == TILE.STONE) {
			texCoor = new int[] {1, 0};
			hasCollider = true;
		}
		if (type == TILE.MOSSYSTONE) {
			texCoor = new int[] {0, 0};
			hasCollider = true;
		}
		if (type == TILE.BLANK) {
			texCoor = new int[] {63, 63};
			hasCollider = false;
		}
		if (type == TILE.STONEBG) {
			texCoor = new int[] {1, 1};
			hasCollider = false;
		}
		if (type == TILE.MOSSYBG) {
			texCoor = new int[] {0, 1};
			hasCollider = false;
		}
		if (type == TILE.BURNEDMOSSY) {
			texCoor = new int[] {0, 2};
			hasCollider = true;
		}
		if (type == TILE.BURNEDSTONE) {
			texCoor = new int[] {1, 2};
			hasCollider = true;
		}
	}
}
