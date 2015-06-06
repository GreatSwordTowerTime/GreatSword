using UnityEditor;
using UnityEngine;
using System.Collections;
[CustomEditor(typeof(TiledMapGeneration))]

public class TileMapEditor : Editor {
	string[] list = new string[7] {"STONE", "MOSSYSTONE", "BLANK","STONEBG","MOSSYBG","BURNEDMOSSY","BURNEDSTONE"};
	int selected = 0;
	public override void OnInspectorGUI () {
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();
		TiledMapGeneration map = (TiledMapGeneration)target;

		selected = EditorGUILayout.Popup (selected, list);

		if(GUILayout.Button("Reset Tile Map")) {
			map.BuildMesh (true);
		}

		if(GUILayout.Button("Build Tile Map")) {
			map.BuildMesh (false);
		}
	}

	void OnSceneGUI () {
		int controlID = GUIUtility.GetControlID (FocusType.Passive);
		Event e = Event.current;
		if (e.type == EventType.mouseDrag || e.type == EventType.mouseDown) {
			if (e.button == 0) {
				TiledMapGeneration tiledmapG = (TiledMapGeneration)target;
				Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
				GUIUtility.hotControl = controlID;
				e.Use ();
				Vector2 mouseToTile = r.origin - tiledmapG.transform.position;
				mouseToTile = new Vector2 (Mathf.FloorToInt (mouseToTile.x/tiledmapG.tileSize), Mathf.FloorToInt (mouseToTile.y/tiledmapG.tileSize));
				switch (selected) {
				case 0 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.STONE);
					break;
				case 1 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.MOSSYSTONE);
					break;
				case 2 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.BLANK);
					break;
				case 3 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.STONEBG);
					break;
				case 4 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.MOSSYBG);
					break;
				case 5 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.BURNEDMOSSY);
					break;
				case 6 :
					tiledmapG.tiledmap.
					tiles[(int)mouseToTile.x, (int)mouseToTile.y].changeType (TILE.BURNEDSTONE);
					break;
				}
				tiledmapG.BuildTile ((int)mouseToTile.x, (int)mouseToTile.y);
			}
		}
	}
}
