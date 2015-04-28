using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraControl : MonoBehaviour {
	[SerializeField] private List<Transform> targets = new List<Transform>();
	private Vector2 offset = new Vector2(0f,0f);
	[SerializeField] private float speed = 2f;
	[SerializeField] private float zoomSpeed = 2f;
	public float minZoom = 1f;
	public float maxZoom = 6f;
	Bounds newBounds;
	private Camera myCamera;
	private void Start () {
		CharacterBase[] characters = FindObjectsOfType<CharacterBase>();
		for (int i = 0; i < characters.Length; i++){
			targets.Add(characters[i].geometryRoot.transform);
		}
		Hub[] hubs = FindObjectsOfType<Hub>();
		for (int i = 0; i < hubs.Length; i++){
			targets.Add(hubs[i].transform);
		}
		myCamera = GetComponent<Camera>();
	}
	private void Update () {
		if (targets.Count > 0){
			newBounds = targets[0].GetComponent<MeshRenderer>().bounds;
			for (int i = 1; i < targets.Count; i++){
				newBounds.Encapsulate(targets[i].position);
			}
			transform.position = Vector3.Lerp(transform.position, new Vector3(newBounds.center.x + offset.x, newBounds.center.y + offset.y, transform.position.z), Time.deltaTime * speed);
			float newZoom = Mathf.Lerp(myCamera.orthographicSize, newBounds.extents.x, Time.deltaTime * zoomSpeed);
			newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
			myCamera.orthographicSize = newZoom;
		}
	}
	private void OnDrawGizmos(){
		Gizmos.DrawWireCube(newBounds.center, newBounds.size);
	}
}
