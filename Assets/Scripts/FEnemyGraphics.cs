using UnityEngine;
using System.Collections;

public class FEnemyGraphics : MonoBehaviour {

	private bool flashing = false;
	private Shader defaultShader;
	private Shader whiteFlashShader;

	void Start () {
		defaultShader = GetComponent <SpriteRenderer> ().sharedMaterial.shader;
		whiteFlashShader = Shader.Find ("GUI/Text Shader");
	}
	
	// Update is called once per frame
	void Update () {
		FollowingEnemy fe = GetComponent <FollowingEnemy> ();

		if (fe.takingDamage) {
			InvokeRepeating ("flash", 0, 0.5f);
		} else {
			CancelInvoke ("flash");
			if (flashing) {
				GetComponent <SpriteRenderer> ().sharedMaterial.shader = defaultShader;
				GetComponent <SpriteRenderer> ().sharedMaterial.color = Color.white;
				flashing = false;
			}
		}
	}

	void flash () {
		if (!flashing) {
			GetComponent <SpriteRenderer> ().sharedMaterial.shader = whiteFlashShader;
			GetComponent <SpriteRenderer> ().sharedMaterial.color = Color.white;
			flashing = true;
		}

		if (flashing) {
			GetComponent <SpriteRenderer> ().sharedMaterial.shader = defaultShader;
			GetComponent <SpriteRenderer> ().sharedMaterial.color = Color.white;
			flashing = false;
		}
	}
}
