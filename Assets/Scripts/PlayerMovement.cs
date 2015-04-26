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
	    public Material RedMaterial, BlueMaterial;



	public CharacterClass characterClass;
	public PlayerColor playerColor;
	//player team. red/blue tags.
	public CharacterData(int t, int p, int h, CharacterClass c, PlayerColor s)
	{
		team = t;
		playerNumber = p;
		health = h;
		characterClass = c;
		playerColor = s;
	}
}


public class PlayerMovement : MonoBehaviour{



	//make this more dynamic
	public GameObject HomeHub;


	// Normal Movements Variables
	private float walkSpeed;
	private float curSpeed;
	private float maxSpeed;
	private float sprintSpeed;
	public GameObject GunObject;

	private float horzInput;
	private float vertInput;
	private bool fire = false;

	public CharacterData thisCharacterData = new CharacterData(1,1,1,CharacterClass.Sniper,PlayerColor.red);

	private Quaternion startRotation;
	void Start()
	{
		startRotation = this.transform.rotation;
		SetClass();
	
		if(thisCharacterData.team == 1){
			this.gameObject.tag = "red";
		}
		else
		{
			this.gameObject.tag = "blue";
		}
		SetColor();
		walkSpeed = 0.10f;
		sprintSpeed = walkSpeed + (walkSpeed / 2);

		
	}



	void FixedUpdate()
	{

		if(thisCharacterData.health <=0 && !hasDied){
			DIE();
		}
		else if(thisCharacterData.health <=0 && (hasDied == true)){
			//wait for respwan i guess?
		}
		else{

		curSpeed = walkSpeed; 
		maxSpeed = curSpeed;

		GetInput();


		if(fire){
			FireWeapon();
		}
		// Move 
			this.transform.rotation = startRotation;
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
			fire = Inputmanager.P4_Fire;
			break;

		default :
			horzInput = Inputmanager.P1_Horizontal;
			vertInput = Inputmanager.P1_Vertical;
			fire = Inputmanager.P1_Fire;
			break;
		}



	}

	void SetColor(){
		//really not sure if this is the best way.
		GameObject playerGraphic = transform.Find("RotationCorrection/Player Graphics").gameObject as GameObject;

		switch(thisCharacterData.playerColor){

		case PlayerColor.red:
			this.gameObject.tag = "red";
//			print (this.transform.childCount.ToString()+" ");
			playerGraphic.GetComponent<MeshRenderer>().material = thisCharacterData.RedMaterial;
			break;

		case PlayerColor.blue:
			this.gameObject.tag = "blue";
			playerGraphic.GetComponent<MeshRenderer>().material = thisCharacterData.BlueMaterial;

			//this.transform.Find("RotationCorrection/Player Graphics").GetComponent<MeshRenderer>().material = thisCharacterData.BlueMaterial;

			break;

		case PlayerColor.grey:
			this.gameObject.tag = "grey";
			break;

		default :
			this.gameObject.tag = "grey";//not sure if this makes a good default
			break;
		}

		this.BroadcastMessage("SetTag", this.gameObject.tag);
	}

	void SetClass(){
		//not sure what the point of this was.

		switch(thisCharacterData.characterClass){
		
		case CharacterClass.Sniper:
			//print ("Sniper!");
			break;
		case CharacterClass.DemoMan:
			//print ("Demo!");
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

	bool hasDied = false;

	void DIE(){
		//stick in purgator
		hasDied = true;
		print (this.name+ " is DEAD");
		this.transform.position = HomeHub.transform.position;

		StartCoroutine (Respawn());
		//hide or blink graphics
	}

	IEnumerator Respawn(){

		this.transform.position = HomeHub.transform.position; //to be sure

		yield return new WaitForSeconds(1);

		hasDied = false;

		this.thisCharacterData.health = 1;
	}

	void Damage(int damageAmount){
		thisCharacterData.health -= damageAmount;
	}

}