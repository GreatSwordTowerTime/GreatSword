using UnityEngine;
using System.Collections;

public class TiledMap {

	public int width;

	public int height;

	public Tile[,] tiles;

	public TiledMap (int width, int height) {
		this.width = width;
		this.height = height;

		tiles = new Tile[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles[x,y] = new Tile (TILE.STONE);
			}
		}
		tiles[0, 0] = new Tile (TILE.STONE);
			
	}

	public int[] getTileTexCoordinatesAt (int x, int y) {
		return tiles[x,y].texCoor;
	}

}
