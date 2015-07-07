using UnityEditor;
using UnityEngine;
using System.Collections;
[CustomEditor(typeof(TiledMapGeneration))]

public class TileMapEditor : Editor {
	string[] list = new string[7] {"STONE", "MOSSYSTONE", "BLANK","STONEBG","MOSSYBG","BURNEDMOSSY","BURNEDSTONE"};
	int selected = 0;
	int size = 1;
	public override void OnInspectorGUI () {
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();
		TiledMapGeneration map = (TiledMapGeneration)target;
		list = new string[map.tileObjects.Length];
		for (int i = 0; i < list.Length; i++)
			list[i] = map.tileObjects[i].name;
		selected = EditorGUILayout.Popup (selected, list);
		size = EditorGUILayout.IntField (size);

		if(GUILayout.Button("Reset Tile Map")) {
			map.BuildMesh (true);
		}

		if(GUILayout.Button("Build Tile Map")) {
			map.BuildMesh (false);
		}
	}

	void OnSceneGUI () {
		TiledMapGeneration tiledmapG = (TiledMapGeneration)target;
		TiledMap tiledmap = tiledmapG.tiledmap;
		Event e = Event.current;
		if (e.type == EventType.mouseDrag || e.type == EventType.mouseDown) {
			if (e.button == 0) {

				Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
				e.Use ();
				Vector2 mouseToTile = r.origin - tiledmapG.transform.position;
				mouseToTile = new Vector2 (Mathf.Clamp (Mathf.FloorToInt (mouseToTile.x/tiledmapG.tileSize), 0, tiledmapG.tiledmap.width - 1), 
					Mathf.Clamp (Mathf.FloorToInt (mouseToTile.y/tiledmapG.tileSize), 0, tiledmapG.tiledmap.height - 1));
				tiledmap.
				tiles[(int)mouseToTile.x * tiledmap.height + (int)mouseToTile.y].changeType ((TILE)tiledmapG.tileObjects[selected].GetComponent <TileProperties> ().tile);
				for (int x = 0; x < size; x++) {
					for (int y = 0; y < size; y++) {
						tiledmap.
						tiles[((int)mouseToTile.x + x) * tiledmap.height + (int)mouseToTile.y + y].changeType ((TILE)tiledmapG.tileObjects[selected].GetComponent <TileProperties> ().tile);
						tiledmapG.BuildTile ((int)mouseToTile.x + x, (int)mouseToTile.y + y);
					}
				}
			}
		} else if (e.type == EventType.mouseUp || e.type == EventType.used) {
			tiledmapG.SaveTileMap ();
		} else if (e.Equals (Event.KeyboardEvent ("tab"))) {
			selected = (selected + 1)%list.Length;
			EditorUtility.SetDirty (target);
		}
	}

}
