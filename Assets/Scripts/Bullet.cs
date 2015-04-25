using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float speed = 10;
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}
}
