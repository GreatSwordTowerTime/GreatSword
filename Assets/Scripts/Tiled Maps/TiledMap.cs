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
		if (!PlayerPrefs.HasKey ("TilesSaved")) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					tiles[x,y] = new Tile (TILE.STONE);
					PlayerPrefs.SetInt ("tile" + x.ToString () + y.ToString (), (int)TILE.STONE);
				}
			}
		} else {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					tiles[x,y] = new Tile ((TILE)PlayerPrefs.GetInt ("tile" + x.ToString () + y.ToString ()));
				}
			}
		}
			
	}

	public int[] getTileTexCoordinatesAt (int x, int y) {
		return tiles[x,y].texturecoordinates;
	}

}
