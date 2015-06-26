using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
[Serializable]
public class TiledMap {
	public int width;
	public int height;
	[SerializeField]
	public Tile[] tiles;

	public TiledMap (int tmWidth, int tmHeight) {
		width = tmWidth;
		height = tmHeight;

		tiles = new Tile[tmWidth * tmHeight];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles[x * height + y] = new Tile (TILE.STONE);
			}
		}
	}

	public int[] getTileTexCoordinatesAt (int x, int y) {
		return tiles[x * height + y].texCoor;
	}

}
