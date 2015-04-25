using UnityEngine;
using System.Collections;



public class PlayerMovement : MonoBehaviour{
	// Normal Movements Variables
	private float walkSpeed;
	private float curSpeed;
	private float maxSpeed;
	private float sprintSpeed;
	

	void Start()
	{

		walkSpeed = 10.0f;
		sprintSpeed = walkSpeed + (walkSpeed / 2);
		
	}
	
	void FixedUpdate()
	{
		curSpeed = walkSpeed;
		maxSpeed = curSpeed;
		
		// Move 
		GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* curSpeed, 0.8f),
		                                   Mathf.Lerp(0, Input.GetAxis("Vertical")* curSpeed, 0.8f));
		float rotateSpeed = 9999.0f;
		//Vector3 moveDirection = gameObject.transform.position; 

		Vector2 dir = GetComponent<Rigidbody2D>().velocity;


		if(dir.magnitude > 0.0f){  //Replace this with input?
		
		//	print(dir.magnitude.ToString());
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//print (angle.ToString());
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotateSpeed * Time.deltaTime); 
	}
	}
}