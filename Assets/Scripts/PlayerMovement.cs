using UnityEngine;
using System.Collections;

/*
 * TODO:  Bullet Collisions knock the player out of Whack.  Really badly.
 * Player doesn't move on my copy--Shep
 * 
 * 
 * 
 * 
 * 
*/

public enum PlayerNumber{one,two,three,four};
public enum PlayerColor{red,blue,grey}

public enum CharacterClass
{
	Sniper,
	DemoMan,
	Melee
}

[System.Serializable]
public class CharacterData {


		public int team;
		public int playerNumber;
		public int health;
		


	public CharacterClass characterClass;
	//player team. red/blue tags.
	public CharacterData(int t, int p, int h, CharacterClass c)
	{
		team = t;
		playerNumber = p;
		health = h;
		characterClass = c;
	}
}


public class PlayerMovement : MonoBehaviour{



	//make this more dynamic
	GameObject HomeHub;


	// Normal Movements Variables
	private float walkSpeed;
	private float curSpeed;
	private float maxSpeed;
	private float sprintSpeed;
	public GameObject GunObject;

	private float horzInput;
	private float vertInput;
	private bool fire = false;

	public CharacterData thisCharacterData = new CharacterData(1,1,1,CharacterClass.Sniper);

	void Start()
	{
		SetClass();
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


	public void DealDamage(int damage){
		thisCharacterData.health -= damage;
	}
	void FixedUpdate()
	{

		if(thisCharacterData.health <=0){
			DIE();
		}
		else{

		curSpeed = walkSpeed; 
		maxSpeed = curSpeed;

		GetInput();


		if(fire){
			FireWeapon();
		}
		// Move 

		Vector2 rawDirection = new Vector2(Mathf.Lerp(0, horzInput * curSpeed, 0.8f), Mathf.Lerp(0, vertInput * curSpeed, 0.8f)); //There are propbably some superflous things here but it works.
		Vector2 directionNormalized = rawDirection.normalized;
		transform.Translate(directionNormalized * maxSpeed);

		float rotateSpeed = 9999.0f;
		//Vector3 moveDirection = gameObject.transform.position; 



		if(vertInput != 0  || horzInput != 0){
			float angle = Mathf.Atan2(vertInput, horzInput) * Mathf.Rad2Deg;
		//print (angle.ToString());

		GunObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotateSpeed * Time.deltaTime); 
			}
		}//alive
	
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

	void SetClass(){
		//not sure what the point of this was.

		switch(thisCharacterData.characterClass){
		
		case CharacterClass.Sniper:
			print ("Sniper!");
			break;
		case CharacterClass.DemoMan:
			print ("Demo!");
			break;
		case CharacterClass.Melee:
			print ("melee!");
			break;
		default:
			print("defaultclass?");
				break;
		}
	}

	void  FireWeapon(){

		switch(thisCharacterData.characterClass){
			
		case CharacterClass.Sniper:
			//print ("Sniper!");
			BroadcastMessage("TryToFire");
			break;
		case CharacterClass.DemoMan:
			//print ("Demo!");
			BroadcastMessage("TryToLayMine");
			break;
		case CharacterClass.Melee:
			print ("melee!");
			break;
		default:
			print("defaultclass?");
			break;
		}


	}

	void DIE(){
		//stick in purgatory
		Debug.Log("Died");
		this.transform.position = HomeHub.transform.position;
		//hide or blink graphics
	}

	void Respawn(){
		this.transform.position = HomeHub.transform.position;
		this.thisCharacterData.health = 1;
	}

}