using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public class TiledMapGeneration : MonoBehaviour {

	public int size_x = 100;
	public int size_y = 50;
	public float tileSize = 1.0f;

	public Texture2D terrainTiles;
	public int tileResolution = 16;
	public int numTilesPerRow;
	public int numRows;

	public TiledMap tiledmap;

	Mesh mesh;

	MeshFilter mesh_filter;

	TileVerts[] verts;
	Vector2[] uv;
	Vector3[] normals;
	Vector3[] vertices;

	Color[][] ChopUpTiles () {


		Color[][] tiles = new Color[numTilesPerRow * numRows][];

		for (int y = 0; y < numRows; y++) {
			for (int x = 0; x < numTilesPerRow; x++) {
				tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}

		}

		return tiles;
	}

	void BuildTexture () {
	
		numTilesPerRow = terrainTiles.width / tileResolution;
		numRows = terrainTiles.height / tileResolution;

		int texWidth = size_x * tileResolution;
		int textHeight = size_y * tileResolution;

		Texture2D texture = new Texture2D (texWidth, textHeight);

		Color[][] tiles = ChopUpTiles ();
		
		for (int y = 0; y < size_y; y++) {
			for (int x = 0; x < size_x; x++) {
				Color[] p = tiles[tiledmap.getTileTexCoordinatesAt (x, y)[1] * numTilesPerRow + tiledmap.getTileTexCoordinatesAt (x, y)[0]];
				texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.Apply ();

		MeshRenderer mesh_renderer = GetComponent <MeshRenderer> ();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
	}

	public void BuildMesh () {

		tiledmap = new TiledMap (size_x, size_y);

		int numTiles = size_x * size_y;
		int numTris = numTiles * 2;

		//int vsize_x = size_x * 2;
		//int vsize_y = size_y * 2;

		int numVerts = numTiles * 4;

		vertices = new Vector3[numVerts];
		normals = new Vector3[numVerts];
		uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3];
		verts = new TileVerts[numTiles];

		int x, y;

		//TileVerts[] verts= new TileVerts[numTiles]; 

		for (y = 0; y < size_y; y++) {
			for(x = 0; x < size_x; x++) {
				verts[y * size_x + x] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, size_x, size_y); 
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



		/*for (y = 0; y < vsize_y; y++) {
			for(x = 0; x < vsize_x; x++) {
				vertices[y * vsize_x + x] = new Vector3 (x * tileSize, y * tileSize);
				normals[y * vsize_x + x] = Vector3.back;
				uv[y * vsize_x + x] = new Vector2 ((float) x / size_x, (float) y / size_y);
			}
		}*/
		
		for (y = 0; y < size_y; y++) {
			for (x = 0; x < size_x; x++) {
				int squareIndex = y * size_x + x;
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
		BuildTexture ();

	}

	public void BuildTile (int x, int y) {
		/*Debug.Log (tiledmap.tiles[x, y].type);
		verts[y * size_x + x] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, size_x, size_y);
		for (int i = 0; i < 4; i++)
			uv[(y * size_x + x) * 4 + i] = verts[y * size_x + x].uv[i];
		mesh.uv = uv;
		mesh_filter.mesh = mesh;*/
		BuildTexture ();
		PlayerPrefs.SetInt ("tile" + x.ToString () + y.ToString (), (int)tiledmap.tiles[x, y].type);
		PlayerPrefs.SetInt ("TilesSaved", 1);
		PlayerPrefs.Save ();
	}

}
