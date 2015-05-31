﻿using UnityEditor;
using UnityEngine;
using System.Collections;
[CustomEditor(typeof(TiledMapGeneration))]

public class TileMapEditor : Editor {
	string[] list = new string[7] {"Stone", "MossyStone", "Blank","STONEBG","MOSSYBG","BURNEDMOSSY","BURNEDSTONE"};
	int selected = 0;
	public override void OnInspectorGUI () {
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();
		TiledMapGeneration map = (TiledMapGeneration)target;

		selected = EditorGUILayout.Popup (selected, list);

		if(GUILayout.Button("Rebuild Tile Map")) {
			map.BuildMesh();
		}
	}

	void OnSceneGUI () {
		int controlID = GUIUtility.GetControlID (FocusType.Passive);
		Event e = Event.current;
		if (e.type == EventType.mouseDrag || e.type == EventType.mouseDown) {
			if (e.button == 0) {
				TiledMapGeneration tiledMapG = (TiledMapGeneration)target;
				Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
				GUIUtility.hotControl = controlID;
				e.Use ();
				Vector2 mouseToTile = r.origin - tiledMapG.gameObject.transform.position;
				mouseToTile = new Vector2 (Mathf.FloorToInt (mouseToTile.x/tiledMapG.tileSize), Mathf.FloorToInt (mouseToTile.y/tiledMapG.tileSize));
				switch (selected) {
				case 0 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.STONE);
					break;
				case 1 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.MOSSYSTONE);
					break;
				case 2 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.BLANK);
					break;
				case 3 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.STONEBG);
					break;
				case 4 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.MOSSYBG);
					break;
				case 5 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.BURNEDMOSSY);
					break;
				case 6 :
					tiledMapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.BURNEDSTONE);
					break;
				}
				tiledMapG.BuildTile ((int)mouseToTile.x, (int)mouseToTile.y);
			}
			else GUIUtility.hotControl = 0;
		}
	}
}
