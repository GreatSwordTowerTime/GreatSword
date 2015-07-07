using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;

public class TiledMapGeneration : MonoBehaviour {

	public float tileSize = 1.0f;

	public float individualTileSize = 1.0f;

	public int tileResolution = 16;

	public int width;

	public int height;

	public Texture2D terrainTiles;

	public Material material;

	public GameObject[] tileObjects;

	public TiledMap tiledmap;

	public bool displayLines;

	MeshFilter mesh_filter;

	TileVerts[] verts;

	public void BuildMesh (bool resetTileMap) {

		int x, y;

		if (resetTileMap) {

			tiledmap = new TiledMap (width, height);
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Assets/Levels/" + gameObject.name + ".txt"))
			{
				for (x = 0; x < tiledmap.width; x++) {
					for (y = 0; y < tiledmap.height; y++) {
						file.Write ("0.");
					}
				}

			}

		} else {

			if (tiledmap.width == 0) {
				tiledmap.width = width;
				tiledmap.height = height;
				Debug.Log ("Didn't remember the width and height. Current Width and Height are " + new Vector2 (width, height));
			}

			LoadTileMap ();
			SaveTileMap ();
		}

		BuildTileProperties ();

		verts = new TileVerts[tiledmap.width * tiledmap.height];

		GameObject tiles = new GameObject ("tiles");
		tiles.transform.parent = gameObject.transform;

		for (x = 0; x < tiledmap.width; x++) {
			for(y = 0; y < tiledmap.height; y++) {
				GameObject tile = BuildTile (x, y);
				tile.transform.parent = tiles.transform;
			}
		}

	}

	public void BuildTileProperties () {
		Debug.Log ("Building Colliders");
		List <GameObject> children = new List <GameObject> ();
		foreach (Transform child in transform) {
			children.Add (child.gameObject);
		}
		children.ForEach (child => DestroyImmediate (child));

		for (int i = 0; i < tileObjects.Length; i++) {
			GameObject tilesProperties = (GameObject)Instantiate (tileObjects[i]);
			TileProperties t = tileObjects[i].GetComponent <TileProperties> ();
			if(t.hasCollider) {
			
				for (int x = 0; x < tiledmap.width; x++) {
					for (int y = 0; y < tiledmap.height; y++) {
						if (needsCollider (x, y)) {
							BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
							bx2D.offset = new Vector2 (individualTileSize/2f + x * individualTileSize, individualTileSize/2f + y * individualTileSize) + (Vector2)transform.position;
							bx2D.size = new Vector2 (individualTileSize, individualTileSize);
							bx2D.isTrigger = t.isTrigger;
						}
					}
				}
			}
			tilesProperties.transform.parent = transform;
		}
	}

	private bool needsCollider (int x, int y) {
		bool needsCollider = false;
		if (!tiledmap.tiles[x * tiledmap.height + y].hasCollider) {
			return needsCollider;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + Mathf.Clamp (y, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + Mathf.Clamp (y + 1, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x, 0, width - 1) * tiledmap.height + Mathf.Clamp (y + 1, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x - 1, 0, width - 1) * tiledmap.height + Mathf.Clamp (y + 1, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x - 1, 0, width - 1) * tiledmap.height + Mathf.Clamp (y, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x - 1, 0, width - 1) * tiledmap.height + Mathf.Clamp (y - 1, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x, 0, width - 1) * tiledmap.height + Mathf.Clamp (y - 1, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		if (!tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + Mathf.Clamp (y - 1, 0, height - 1)].hasCollider || tiledmap.tiles[Mathf.Clamp (x + 1, 0, width - 1) * tiledmap.height + y].isTrigger) {
			needsCollider = true;
		}
		return needsCollider;
	}

	public GameObject BuildTile (int x, int y) {
		if (!(GameObject.Find ("tile" + x.ToString () + y.ToString ()) == null)) {
			DestroyImmediate (GameObject.Find ("tile" + x.ToString () + "." + y.ToString ()));
		}

		GameObject tile = new GameObject ("tile" + x.ToString () + "." + y.ToString ());
		tile.transform.position = (new Vector3 (x, y, -(x + y)/100f) * individualTileSize) + transform.position;
		Mesh mesh = new Mesh ();
		Vector3[] vertices = new Vector3[4];
		Vector3[] normals = new Vector3[4];
		Vector2[] uv = new Vector2[4];
		int[] triangles = new int[6];

		vertices[0] = new Vector3 (0, 0, 0);
		vertices[1] = new Vector3 (tileSize, 0, 0);
		vertices[2] = new Vector3 (0, tileSize, 0);
		vertices[3] = new Vector3 (tileSize, tileSize, 0);

		uv[0] = new Vector2 ((float)tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution / (float)(terrainTiles.width), (float)tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution / (float)(terrainTiles.width));
		uv[1] = new Vector2 ((float)(tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution + tileResolution) / (float)terrainTiles.width, (float)tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution / (float)terrainTiles.width);
		uv[2] = new Vector2 ((float)tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution / (float)terrainTiles.width, (float)(tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution + tileResolution) / (float)terrainTiles.width);
		uv[3] = new Vector2 ((float)(tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution + tileResolution) / (float)terrainTiles.width, (float)(tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution + tileResolution) / (float)terrainTiles.width);

		for (int i = 0; i < 4; i++) 
			normals[i] = Vector3.back;

		triangles[0] = 0;
		triangles[1] = 3;
		triangles[2] = 2;
		triangles[3] = 0;
		triangles[4] = 1;
		triangles[5] = 3;

		tile.AddComponent <MeshFilter> ();
		tile.AddComponent <MeshRenderer> ();

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.triangles = triangles;
		tile.GetComponent <MeshFilter> ().mesh = mesh;
		tile.GetComponent <MeshRenderer> ().sharedMaterial = material;
		tile.GetComponent <MeshRenderer> ().sharedMaterial.mainTexture = terrainTiles;

		return tile;

	}

	public void SaveTileMap () {
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Assets/Levels/" + gameObject.name + ".txt"))
		{
			for (int x = 0; x < tiledmap.width; x++) {
				for (int y = 0; y < tiledmap.height; y++) {
					file.Write (((int)tiledmap.tiles [x * tiledmap.height + y].type).ToString () + ".");
				}
			}

		}
	}

	public void LoadTileMap () {
		Tile[] tempTiles = new Tile[width * height];
		int x, y;
		string text = System.IO.File.ReadAllText(@"Assets/Levels/" + gameObject.name + ".txt");
		string[] tileTexts = text.Split (new char[1] {'.'});
		for (x = 0; x < tiledmap.width; x++) {
			for (y = 0; y < tiledmap.height; y++) {
				tempTiles[x * tiledmap.height + y] = new Tile ((TILE)Int32.Parse (tileTexts[x * tiledmap.height + y]));
			}
		}

		for (x = 0; x < width; x++) {
			for (y = 0; y < height; y++) {
				if (tempTiles[x * height + y] == null) {
					tempTiles[x * height + y] = new Tile (TILE.STONE);
				}
			}
		}

		tiledmap.tiles = tempTiles;
		tiledmap.width = width;
		tiledmap.height = height;
	}

	void OnDrawGizmos () {
		if (displayLines) {
			Gizmos.color = Color.green;
	
			for (int x = 0; x < tiledmap.width; x += 3) {
				Gizmos.DrawLine (new Vector3 (x * individualTileSize, 0, 0) + transform.position, new Vector3 (x * individualTileSize, tiledmap.height * individualTileSize, 0) + transform.position);
			}

			for (int y = 0; y < tiledmap.height; y += 3) {
				Gizmos.DrawLine (new Vector3 (0, y * individualTileSize, 0) + transform.position, new Vector3 (tiledmap.width * individualTileSize, y * individualTileSize, 0) + transform.position);
			}
		}
	}
}
