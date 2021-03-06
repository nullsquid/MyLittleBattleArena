﻿using UnityEngine;
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

public enum CharacterClass
{
	Sniper,
	DemoMan,
	Melee
}

[System.Serializable]
public class CharacterData {
	public PlayerTeam team;
	public int playerNumber {get; set;}
	public int health;
	public CharacterClass characterClass;
	//public PlayerColor playerColor;
	//player team. red/blue tags.
	public CharacterData(int h, CharacterClass c)
	{
		health = h;
		characterClass = c;
	}
}


public class PlayerMovement : MonoBehaviour{  //this should probably be renamed


	//make this more dynamic

	public GameObject HomeHub;
	// Normal Movements Variables
	private float walkSpeed;
	private float curSpeed;
	private float maxSpeed;
	//private float sprintSpeed;
	public GameObject GunObject;
	private float horzInput;
	private float vertInput;
	private bool fire = false;
	private bool  canMove = true;
	public Vector3 spawnPosition;
	public CharacterData thisCharacterData = new CharacterData(100,CharacterClass.Sniper);
	private  Vector3  startRotation;
	void Start()
	{

		spawnPosition = transform.position;
		GetPlayerNumber();

		transform.eulerAngles = new Vector3(0,0,0);
		SetClass();
		walkSpeed = 0.10f;
		//sprintSpeed = walkSpeed + (walkSpeed / 2);
	}
	
	
	public void DealDamage(int damage){
		thisCharacterData.health -= damage;
	}


	void GetPlayerNumber(){
		if (thisCharacterData.playerNumber == 0){
			Inputmanager.instance.AddPlayer(this);
		}

	}

	void FixedUpdate()
	{
		
		if(thisCharacterData.health <=0 && !hasDied){
			DIE();
		}
		else if(thisCharacterData.health <=0 && (hasDied == true)){
			//wait for respwan i guess?
		}
		else if(!canMove){
			//wait for stuff to happen

		}

		else{
			
			curSpeed = walkSpeed; 
			maxSpeed = curSpeed;
			
			GetInput();
			
			if(fire){
				FireWeapon();
			}
			// Move 
			transform.eulerAngles = new Vector3(0,0,0);
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
		//	print("defaultclass?");
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
		//	print ("melee!");
			break;
		default:
			print("defaultclass?");
			break;
		}
		
		
	}
	
	bool hasDied = false;
	
	void DIE(){
		//stick in purgatory
		Debug.Log("Died");
		
		//stick in purgator
		hasDied = true;
		print (this.name+ " is DEAD");
		this.transform.position = spawnPosition;
		//this.transform.position = team.teamHub.transform.position;


		//print (this.name+ " is DEAD");
		//this.transform.position = HomeHub.transform.position;
		
		StartCoroutine (Respawn());
		//hide or blink graphics
	}
	
	IEnumerator Respawn(){
		
		//this.transform.position = spawnPosition; //to be sure
		
		yield return new WaitForSeconds(1);
		hasDied = false;
		this.thisCharacterData.health = 1;
	}
	
	void Damage(int damageAmount){
		thisCharacterData.health -= damageAmount;
	}
	
}