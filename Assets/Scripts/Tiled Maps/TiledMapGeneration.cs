using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;
[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[Serializable]
public class TiledMapGeneration : MonoBehaviour {

	public float tileSize = 1.0f;

	public float individualTileSize = 1.0f;

	public int tileResolution = 16;

	public int width;

	public int height;

	public Texture2D terrainTiles;

	public GameObject[] tileObjects;

	public TiledMap tiledmap;

	public bool displayLines;
	[HideInInspector]
	[SerializeField]
	Mesh mesh;
	[HideInInspector]
	[SerializeField]
	Vector2[] uv;
	[HideInInspector]
	[SerializeField]
	TileVerts[] verts;

	public void BuildMesh (bool resetTileMap) {

		int x, y;

		if (resetTileMap) {
			tiledmap.tiles = new Tile[tiledmap.height * tiledmap.width];
			for (x = 0; x < tiledmap.width; x++) {
				for (y = 0; y < tiledmap.height; y++) {
					tiledmap.tiles[x * tiledmap.height + y] = new Tile (TILE.STONE);
				}
			}

		} else {

			if (tiledmap.width == 0) {
				tiledmap.width = width;
				tiledmap.height = height;
				Debug.Log ("Didn't remember the width and height. Current Width and Height are " + new Vector2 (width, height));
			}

		}

		BuildTileProperties ();

		int tileCount = tiledmap.width * tiledmap.height;

		verts = new TileVerts[tileCount];

		Vector3[] vertices = new Vector3[tileCount * 4];
		Vector3[] normals = new Vector3[tileCount * 4];
		uv = new Vector2[tileCount * 4];
		int[] triangles = new int[tileCount * 4 * 6];

		for (x = 0; x < tiledmap.width; x++) {
			for(y = 0; y < tiledmap.height; y++) {
				verts[x * tiledmap.height + y] = new TileVerts (new Vector3 (x * individualTileSize, y * individualTileSize), tileSize, 
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
				triangles[triOffset] = (squareIndex) * 4 + 2;
				triangles[triOffset + 1] = (squareIndex) * 4 + 3;
				triangles[triOffset + 2] = (squareIndex) * 4 + 0;
				triangles[triOffset + 3] = (squareIndex) * 4 + 3;
				triangles[triOffset + 4] = (squareIndex) * 4 + 1;
				triangles[triOffset + 5] = (squareIndex) * 4 + 0;
			}
		}

		mesh = new Mesh ();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		MeshFilter mesh_filter = GetComponent <MeshFilter> ();
		MeshRenderer mesh_renderer = GetComponent <MeshRenderer> ();
		mesh_filter.mesh = mesh;
		mesh_renderer.sharedMaterial.mainTexture = terrainTiles;

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
		
	public void BuildTile (int x, int y) {
		verts[x * tiledmap.height + y] = new TileVerts (new Vector3 (x * individualTileSize, y * individualTileSize), tileSize,
			tileResolution, terrainTiles.width, tiledmap.getTileTexCoordinatesAt (x, y)[0] * tileResolution, tiledmap.getTileTexCoordinatesAt (x, y)[1] * tileResolution);
		for (int i = 0; i < 4; i++) {
			uv[(x * tiledmap.height + y) * 4 + i] = verts[x * tiledmap.height + y].uv[i];
			mesh.uv = uv;
			GetComponent <MeshFilter> ().mesh = mesh;
			GetComponent <MeshRenderer> ().sharedMaterial.mainTexture = terrainTiles;
		}
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
