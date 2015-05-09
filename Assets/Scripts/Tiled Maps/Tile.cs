using UnityEngine;
using System.Collections;

public enum TILE : int {
	STONE,
	MOSSYSTONE,
	BLANK
}

public class Tile {


	public TILE type;

	public int[] texCoor;

	public Tile (TILE type) {
		this.type = type;
		if(type == TILE.STONE)
			texCoor = new int[2] {0,0};

		if(type == TILE.MOSSYSTONE)
			texCoor = new int[2] {1,0};

		if (type == TILE.BLANK)
			texCoor = new int[2] {63, 63};

	}
}
