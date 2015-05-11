using UnityEditor;
using UnityEngine;
using System.Collections;
[CustomEditor(typeof(TiledMapGeneration))]

public class TileMapEditor : Editor {
	string[] list = new string[3] {"Stone", "MossyStone", "Blank"};
	int selected = 0;
	public override void OnInspectorGUI () {
		//base.OnInspectorGUI ();
		serializedObject.Update ();
		DrawDefaultInspector ();
		

		selected = EditorGUILayout.Popup (selected, list);

		if(GUILayout.Button("Rebuild Tile Map")) {
			TiledMapGeneration map = (TiledMapGeneration)target;
			map.BuildMesh();
		}

		if (GUI.changed)
			EditorUtility.SetDirty (target);
		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI () {
		int controlID = GUIUtility.GetControlID (FocusType.Passive);
		if (Event.current.type == EventType.mouseDown) {
			TiledMapGeneration tiledMapG = (TiledMapGeneration)target;
			Debug.Log ("Collider");
			Event e = Event.current;

			Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
			Vector2 mouseToTile = r.origin - tiledMapG.gameObject.transform.position;
			mouseToTile = new Vector2 (Mathf.FloorToInt (mouseToTile.x/tiledMapG.tileSize), Mathf.FloorToInt (mouseToTile.y/tiledMapG.tileSize));
			if (selected == 0) {
				tiledMapG.tiledmap.
				tiles[(int)mouseToTile.x, (int)mouseToTile.y] = new Tile (TILE.STONE);
			}

			if (selected == 1) {
				tiledMapG.tiledmap.
				tiles[(int)mouseToTile.x, (int)mouseToTile.y] = new Tile (TILE.MOSSYSTONE);
			}

			if (selected == 2) {
				tiledMapG.tiledmap.
				tiles[(int)mouseToTile.x, (int)mouseToTile.y] = new Tile (TILE.BLANK);
			}
			tiledMapG.BuildTile ((int)mouseToTile.x, (int)mouseToTile.y);
			Debug.Log (mouseToTile);
		}
	}
}
