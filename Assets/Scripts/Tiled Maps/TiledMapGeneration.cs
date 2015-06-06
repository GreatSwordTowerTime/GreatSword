using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class TiledMapGeneration : MonoBehaviour {

	public float tileSize = 1.0f;

	public int tileResolution = 16;

	private int width;

	private int height;

	public Texture2D terrainTiles;

	public GameObject[] tileObjects;

	public TiledMap tiledmap;

	Mesh mesh;

	MeshFilter mesh_filter;

	TileVerts[] verts;
	Vector2[] uv;

	public void BuildMesh (bool resetTileMap) {

		int x, y;

		if (resetTileMap) {
			//tiledmap = ScriptableObject.CreateInstance <TiledMap> ();
			tiledmap.tiles = new Tile[tiledmap.width, tiledmap.height];
				
			for (x = 0; x < tiledmap.width; x++) {
				for (y = 0; y < tiledmap.height; y++) {
					tiledmap.tiles[x,y] = new Tile (TILE.STONE);
				}
			}
			width = tiledmap.width;
			height = tiledmap.height;
		} else {
			Tile[,] tempTiles = new Tile[tiledmap.width, tiledmap.height];
			for (x = 0; x < width; x++) {
				for (y = 0; y < height; y++) {
						tempTiles[x, y] = tiledmap.tiles[x, y];
				}
			}

			for (x = 0; x < tiledmap.width; x++) {
				for (y = 0; y < tiledmap.height; y++) {
					if (tempTiles[x, y] == null)
						tempTiles[x, y] = new Tile (TILE.STONE);
				}
			}
			tiledmap.tiles = tempTiles;
			width = tiledmap.width;
			height = tiledmap.height;

		}

		int numTiles = tiledmap.width * tiledmap.height;
		int numTris = numTiles * 2;

		int numVerts = numTiles * 4;

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3];
		verts = new TileVerts[numTiles];



		for (y = 0; y < tiledmap.height; y++) {
			for(x = 0; x < tiledmap.width; x++) {
				verts[y * tiledmap.width + x] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, 
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
		
		for (y = 0; y < tiledmap.height; y++) {
			for (x = 0; x < tiledmap.width; x++) {
				int squareIndex = y * tiledmap.width + x;
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
		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Tile"))
			DestroyImmediate (g);
		for (int i = 0; i < tileObjects.Length; i++) {
			GameObject tilesProperties = (GameObject)Instantiate (tileObjects[i]);
			TileProperties t = tileObjects[i].GetComponent <TileProperties> ();
			if(t.hasCollider) {
			
				for (int x = 0; x < tiledmap.width; x++) {
					for (int y = 0; y < tiledmap.height; y++) {
						if ((int)tiledmap.tiles[x, y].type != t.tile && tiledmap.tiles[x, y].hasCollider == false) {
							if ((int)tiledmap.tiles[x + 1, y].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x + tileSize, tileSize/2f + y);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x, y + 1].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x, tileSize/2f + y + tileSize);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x + 1, y + 1].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x + tileSize, tileSize/2f + y + tileSize);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x - 1, y].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x - tileSize, tileSize/2f + y);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x, y - 1].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x, tileSize/2f + y - tileSize);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x - 1, y - 1].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x - tileSize, tileSize/2f + y - tileSize);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x + 1, y - 1].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x + tileSize, tileSize/2f + y - tileSize);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
							if ((int)tiledmap.tiles[x - 1, y + 1].type == t.tile) {
								BoxCollider2D bx2D = tilesProperties.AddComponent <BoxCollider2D> ();
								bx2D.offset = new Vector2 (tileSize/2f + x - tileSize, tileSize/2f + y + tileSize);
								bx2D.size = new Vector2 (tileSize, tileSize);
								bx2D.isTrigger = t.isTrigger;
							}
						}
					}
				}
			}
			tilesProperties.transform.parent = transform;
		}
	}

	public void BuildTile (int x, int y) {
		verts[y * tiledmap.width + x] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize,
			tileResolution, terrainTiles.width, tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution, tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution);
		for (int i = 0; i < 4; i++) {
			uv[(y * tiledmap.width + x) * 4 + i] = verts[y * tiledmap.width + x].uv[i];
			mesh.uv[(y * tiledmap.width + x) * 4 + i] = verts[y * tiledmap.width + x].uv[i];
		}
		mesh.uv = uv;
		GetComponent <MeshFilter> ().mesh = mesh;
	}
}
