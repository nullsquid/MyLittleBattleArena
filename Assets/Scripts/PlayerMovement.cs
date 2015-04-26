using UnityEngine;
using System.Collections;



public enum PlayerNumber{one,two,three,four};



[System.Serializable]
public class CharacterData {


		public int team;
		public int playerNumber;
		public int health;
	//player team. red/blue tags.
	public CharacterData(int t, int p, int h)
	{
		team = t;
		playerNumber = p;
		health = h;
	}
}


public class PlayerMovement : MonoBehaviour{





	// Normal Movements Variables
	private float walkSpeed;
	private float curSpeed;
	private float maxSpeed;
	private float sprintSpeed;
	public GameObject GunObject;

	private float horzInput;
	private float vertInput;
	private bool fire = false;

	public CharacterData thisCharacterData = new CharacterData(1,1,1);
	void Start()
	{

		if(thisCharacterData.team == 1){
			this.gameObject.tag = "red";
		}
		else
		{
			this.gameObject.tag = "blue";
		}

		walkSpeed = 0.10f;
		sprintSpeed = walkSpeed + (walkSpeed / 2);
		
	}



	void FixedUpdate()
	{
		GetInput();
		//print(Inputmanager.P1_Horizontal);
		curSpeed = walkSpeed;
		maxSpeed = curSpeed;
		
		// Move 
		transform.Translate (new Vector2(Mathf.Lerp(0, horzInput * curSpeed, 0.8f), Mathf.Lerp(0, vertInput * curSpeed, 0.8f)));


		float rotateSpeed = 9999.0f;
		//Vector3 moveDirection = gameObject.transform.position; 



		if(vertInput != 0  || horzInput != 0){
			float angle = Mathf.Atan2(vertInput, horzInput) * Mathf.Rad2Deg;
		//print (angle.ToString());

		GunObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotateSpeed * Time.deltaTime); 
		}
	
 }
	void GetInput(){

		switch(thisCharacterData.playerNumber){
		case 1:
			horzInput = Inputmanager.P1_Horizontal;
			vertInput = Inputmanager.P1_Vertical;
			fire = Inputmanager.P1_Fire;
			break;
		case 2:
			horzInput = Inputmanager.P2_Horizontal;
			vertInput = Inputmanager.P2_Vertical;
			fire = Inputmanager.P2_Fire;
			break;
		case 3:
			horzInput = Inputmanager.P3_Horizontal;
			vertInput = Inputmanager.P3_Vertical;
			fire = Inputmanager.P3_Fire;
			break;
		case 4:
			horzInput = Inputmanager.P4_Horizontal;
			vertInput = Inputmanager.P4_Vertical;
			fire = Inputmanager.P1_Fire;
			break;

		default :
			horzInput = Inputmanager.P1_Horizontal;
			vertInput = Inputmanager.P1_Vertical;
			fire = Inputmanager.P1_Fire;
			break;
		}



	}
}