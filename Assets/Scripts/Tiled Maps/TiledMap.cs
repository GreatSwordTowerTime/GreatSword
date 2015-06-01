using UnityEngine;
using UnityEditor;
using System.Collections;

public class TiledMap : ScriptableObject {

	public int width;

	public int height;

	public Tile[,] tiles;

	public TiledMap (int width, int height) {
		this.width = width;
		this.height = height;


		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles[x,y] = new Tile (TILE.STONE);
			}
		}

	}

	public int[] getTileTexCoordinatesAt (int x, int y) {
		return tiles[x,y].texCoor;
	}

	[MenuItem("Assets/Create/Tile Map")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<TiledMap> ();
	}

}
