﻿using UnityEngine;
using System.Collections;

public class TileVerts {

	public readonly Vector3[] verts = new Vector3[4];

	public readonly Vector2[] uv = new Vector2[4];

	public TileVerts (Vector2 position, float tileSize, int tileTexSize, int textureSize, int x, int y) {
		verts[0] = position;
		verts[1] = new Vector2 (position.x + tileSize, position.y);
		verts[2] = new Vector2 (position.x, position.y + tileSize);
		verts[3] = new Vector2 (position.x + tileSize, position.y + tileSize);

		uv[0] = new Vector2 (x / textureSize, y / textureSize);
		uv[1] = new Vector2 ((x + tileTexSize) / textureSize, y / textureSize);
		uv[2] = new Vector2 (x / tileTexSize, (y + tileTexSize) / textureSize);
		uv[3] = new Vector2 ((x + tileTexSize) / textureSize, (y + tileTexSize) / textureSize);
	}
}
