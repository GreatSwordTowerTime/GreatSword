using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TiledMapGeneration : MonoBehaviour {

	public int size_x = 100;
	public int size_y = 50;
	public float tileSize = 1.0f;
	//for uv mapping
	public int tileTexSize;
	//Size of texture for mapping uv's
	public int textureSize;

	//public Texture2D terrainTiles;
	//public int tileResolution = 16;

	public TiledMap tiledmap;

	/*Color[][] ChopUpTiles () {


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
	}*/

	public void BuildMesh () {

		tiledmap = new TiledMap (size_x, size_y);

		int numTiles = size_x * size_y;
		int numTris = numTiles * 2;

		int numVerts = numTiles * 4;

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3];

		int x, y;

		TileVerts[] verts= new TileVerts[numTiles]; 

		for (y = 0; y < size_y; y++) {
			for(x = 0; x < size_x; x++) {
				verts[y * size_x + x] = new TileVerts (new Vector3 (x * tileSize, y * tileSize), tileSize, tileTexSize, textureSize,
					tiledmap.tiles[x,y].texCoor[0] * tileTexSize, tiledmap.tiles[x,y].texCoor[1] * tileTexSize); 
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


		Mesh mesh = new Mesh ();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		MeshFilter mesh_filter = GetComponent <MeshFilter> ();
		mesh_filter.mesh = mesh;

		//BuildTexture ();

	}

}
