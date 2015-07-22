using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	public float arrowSpeed = 2f;
	

	void FixedUpdate () {
        transform.Translate( -transform.up * arrowSpeed);
		
        }
}
