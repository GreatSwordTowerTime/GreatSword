using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TiledMapGeneration : MonoBehaviour {

	public float tileSize = 1.0f;

	public int tileResolution = 16;

	public int width;

	public int height;

	public Texture2D terrainTiles;

	public GameObject[] tileObjects;

	public TiledMap tiledmap;

	public bool displayLines;

	Mesh mesh;

	MeshFilter mesh_filter;

	TileVerts[] verts;
	Vector2[] uv;

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

		int numTiles = tiledmap.width * tiledmap.height;
		int numTris = numTiles * 2;

		int numVerts = numTiles * 4;

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3];
		verts = new TileVerts[numTiles];



		for (x = 0; x < tiledmap.width; x++) {
			for(y = 0; y < tiledmap.height; y++) {
				verts[x * tiledmap.height + y] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, 
					tileResolution, terrainTiles.width, tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution, tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution); 
			}
		}


		int a = 0;
		for (int i = 0; i <  verts.Length; i++) {
			for (int j = 0; j < 4; j++) {
				vertices[a] = verts[i].verts[j];
				normals[a] = Vector3.back;
				uv[a] = verts[i].uv[j];
				a++;
			}
		}
		
		for (x = 0; x < tiledmap.width; x++) {
			for (y = 0; y < tiledmap.height; y++) {
				int squareIndex = x * tiledmap.height + y;
				int triOffset = squareIndex * 6;
				//int vertOffset = 
				triangles[triOffset] = (squareIndex) * 4 + 0;
				triangles[triOffset + 1] = (squareIndex) * 4 + 3;
				triangles[triOffset + 2] = (squareIndex) * 4 + 2;
				triangles[triOffset + 3] = (squareIndex) * 4 + 0;
				triangles[triOffset + 4] = (squareIndex) * 4 + 1;
				triangles[triOffset + 5] = (squareIndex) * 4 + 3;
			}
		}

		mesh = new Mesh ();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		mesh_filter = GetComponent <MeshFilter> ();
		mesh_filter.mesh = mesh;
		MeshRenderer mesh_renderer = GetComponent <MeshRenderer> ();
		mesh_renderer.sharedMaterial.mainTexture = terrainTiles;
		BuildTileProperties ();

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
							bx2D.offset = new Vector2 (tileSize/2f + x * tileSize, tileSize/2f + y * tileSize) + (Vector2)transform.position;
							bx2D.size = new Vector2 (tileSize, tileSize);
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

	public void BuildTile (int x, int y) {
		verts[x * tiledmap.height + y] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize,
			tileResolution, terrainTiles.width, tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution, tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution);
		for (int i = 0; i < 4; i++) {
			uv[(x * tiledmap.height + y) * 4 + i] = verts[x * tiledmap.height + y].uv[i];
			mesh.uv[(x * tiledmap.height + y) * 4 + i] = verts[x * tiledmap.height + y].uv[i];
		}

		mesh.uv = uv;
		GetComponent <MeshFilter> ().mesh = mesh;

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
				Gizmos.DrawLine (new Vector3 (x * tileSize, 0, 0) + transform.position, new Vector3 (x * tileSize, tiledmap.height * tileSize, 0) + transform.position);
			}

			for (int y = 0; y < tiledmap.height; y += 3) {
				Gizmos.DrawLine (new Vector3 (0, y * tileSize, 0) + transform.position, new Vector3 (tiledmap.width * tileSize, y * tileSize, 0) + transform.position);
			}
		}
	}
}
