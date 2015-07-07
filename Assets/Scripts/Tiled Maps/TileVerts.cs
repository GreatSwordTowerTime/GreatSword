using UnityEngine;
using System;
using System.Collections;
[Serializable]
public class TileVerts {
	[SerializeField]
	public readonly Vector3[] verts = new Vector3[4];
	[SerializeField]
	public readonly Vector2[] uv = new Vector2[4];

	public TileVerts (Vector2 position, float tileSize, int tileTexSize, int texSize, int uvx, int uvy) {
		float vertOffset = 0.000f;
		verts[0] = new Vector2 (position.x - vertOffset, position.y - vertOffset);
		verts[1] = new Vector2 (position.x + tileSize + vertOffset, position.y - vertOffset);
		verts[2] = new Vector2 (position.x - vertOffset, position.y + tileSize + vertOffset);
		verts[3] = new Vector2 (position.x + tileSize + vertOffset, position.y + tileSize + vertOffset);

		uv[0] = new Vector2 ((float)uvx / (float)(texSize), (float)uvy / (float)(texSize));
		uv[1] = new Vector2 ((float)(uvx + tileTexSize) / (float)texSize, (float)uvy / (float)texSize);
		uv[2] = new Vector2 ((float)uvx / (float)texSize, (float)(uvy + tileTexSize) / (float)texSize);
		uv[3] = new Vector2 ((float)(uvx + tileTexSize) / (float)texSize, (float)(uvy + tileTexSize) / (float)texSize);
	}
}
