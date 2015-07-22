using UnityEngine;
using System.Collections;

public class ArrowTrap : MonoBehaviour {
    public GameObject Astroid;
    public float arrowSpawnRate = .5f;
    public float MinX = -3;
    public float MaxX = 3;
    public float MinY = 5;
    public float MaxY = 6;

    //for 3d you have z position
    public float MinZ = 0;
    public float MaxZ = 10;

    //turn off or on 3D placement
    public bool is3D = false;
    
    
    
    void Start() {

        InvokeRepeating("Spawn", 2, arrowSpawnRate);
    }

    void Spawn() {
        float x = Random.Range(MinX, MaxX);
        GameObject arrow = (GameObject)Instantiate(Astroid, new Vector3(x, 0, 0) + transform.position, Quaternion.identity);
		arrow.transform.parent = transform;
		Destroy(arrow,3f);
        
    }
    
 
    
}
