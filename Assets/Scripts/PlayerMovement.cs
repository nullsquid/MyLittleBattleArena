using UnityEngine;
using System.Collections;



public enum PlayerNumber{one,two,three,four};




public class PlayerMovement : MonoBehaviour{
	// Normal Movements Variables
	private float walkSpeed;
	private float curSpeed;
	private float maxSpeed;
	private float sprintSpeed;
	public GameObject GunObject;

	void Start()
	{

		walkSpeed = 0.10f;
		sprintSpeed = walkSpeed + (walkSpeed / 2);
		
	}



	void FixedUpdate()
	{
		curSpeed = walkSpeed;
		maxSpeed = curSpeed;
		
		// Move 
		transform.Translate (new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* curSpeed, 0.8f),
		                                   Mathf.Lerp(0, Input.GetAxis("Vertical")* curSpeed, 0.8f)));


		float rotateSpeed = 9999.0f;
		//Vector3 moveDirection = gameObject.transform.position; 



		if(Input.GetAxis("Vertical") != 0  || Input.GetAxis("Horizontal") != 0){
		float angle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;
		//print (angle.ToString());

		GunObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotateSpeed * Time.deltaTime); 
		}
	
 }
}