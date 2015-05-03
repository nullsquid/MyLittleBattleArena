using UnityEngine;
using System.Collections;
public class CharacterBase : MonoBehaviour{  //this should probably be renamed
	public int id = -1;
	public CharacterClass characterClass;
	public GameObject geometryRoot;
	public InputScheme inputScheme;
	public PlayerTeam team;
	private float health = 10f;
	public float maxHealth {get {return (characterClass != null) ? characterClass.maxHealth : 10f;}}
	public bool isDead {get; private set;}
	private float moveSpeed {get {return (characterClass != null) ? characterClass.moveSpeed : 4f;}}
	[SerializeField]private float horizontalInput;
	[SerializeField]private float verticalInput;
	private bool ability1Pressed;
	private bool ability2Pressed;
	private bool ability3Pressed;
	private bool canMove = true;
	public Vector3 spawnPosition {get; set;}
	public Quaternion spawnRotation {get; set;}
    [SerializeField] private float respawnDuration = 2f;	//TODO visible timer
	private bool isRespawning = false;
	private const int respawnImmunityFrameLength = 30;
	private const int respawnFramesBetweenFlashes = 3;
	private bool canUseAbilities = true;
	public int deaths {get; private set;}
	//private const float holdInputTime = 0.6f;
	private void Start(){
		InputManager.instance.AssignPlayerInputs(this);
	}
	private void Update(){
		if(GameManager.isInMatch && !isDead){
			if (health < 1f){
				Death();
			}else{
				ProcessInput();
				HandleAbilities();
				HandleMovement();
			}
		}
	}
	private void ProcessInput(){
		horizontalInput = 0f;
		verticalInput = 0f;
		if (inputScheme != null){
			if (Input.GetKey(inputScheme.positiveHorizontalButton)){
				horizontalInput += 1;
			}
			if (Input.GetKey(inputScheme.negativeHorizontalButton)){
				horizontalInput -= 1;
            }
			if (Input.GetKey(inputScheme.positiveVerticalButton)){
				verticalInput += 1;
			}
			if (Input.GetKey(inputScheme.negativeVerticalButton)){
				verticalInput -= 1;
			}
			bool ability1Down = Input.GetKeyDown(inputScheme.abilityButton1);
			bool ability2Down = Input.GetKeyDown(inputScheme.abilityButton2);
			ability1Pressed = (ability1Down && !ability2Down);
			ability2Pressed = (!ability1Down && ability2Down);
			ability3Pressed = (ability1Down && ability2Down);
        }
    }
	private void HandleAbilities(){
		if (canUseAbilities && characterClass != null){
			if (ability1Pressed){
				UseAbility1();
			}else if (ability2Pressed){
				UseAbility2();
			}else if (ability3Pressed){
				UseAbility3();
			}
		}
	}
	private void  UseAbility1(){
		if (characterClass.ability1 != null){
			characterClass.ability1.OnActivate();
		}
	}
	private void  UseAbility2(){
		if (characterClass.ability2 != null){
			characterClass.ability2.OnActivate();
		}
    }
	private void UseAbility3(){
		if (characterClass.ability3 != null){
			characterClass.ability3.OnActivate();
		}
	}
	/*private void  UseHeldAbility1(){
		if (characterClass.heldAbility1 != null){
			characterClass.heldAbility1.OnActivate();
		}
	}
	private void  UseHeldAbility2(){
		if (characterClass.heldAbility2 != null){
			characterClass.heldAbility2.OnActivate();
		}
    }
	private void UseHeldAbility3(){
		if (characterClass.ability3 != null){
			characterClass.heldAbility3.OnActivate();
		}
	}*/
	private void HandleMovement(){
		if(canMove && (verticalInput != 0f || horizontalInput != 0f)){
			Vector2 movement = new Vector2(horizontalInput, verticalInput) * Time.deltaTime * moveSpeed;
			//Vector2 movement = Vector2.Lerp(Vector2.zero, direction, moveSpeed * Time.deltaTime).normalized;
			transform.Translate(movement, Space.World);
			float angle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0f, 0f, angle);
		}
	}
	private void Death(){
		//TODO Turn off, return to base
		isDead = true;
		deaths++;
		transform.position = spawnPosition;
		transform.rotation = spawnRotation;
		StartCoroutine (Respawn());
		//TODO death blinking
	}
	IEnumerator Respawn(){
		isRespawning = true;
		float respawnTime = 0f;
		while (respawnTime < respawnDuration){
			respawnTime += Time.deltaTime;
			yield return null;
		}
		OnRespawn();
	}
	private void OnRespawn(){
		isDead = false;
		health = maxHealth;
		StartCoroutine(RespawnEffects());
	}
	private IEnumerator RespawnEffects(){
		int frame = 0;
		int nextFrameToFlash = 0;
		while (frame < respawnImmunityFrameLength){
			if (frame == nextFrameToFlash){
				geometryRoot.SetActive(!geometryRoot.activeSelf);
				nextFrameToFlash += respawnFramesBetweenFlashes;
			}
			frame++;
			yield return null;
		}
		geometryRoot.SetActive(true);
		isRespawning = false;
	}
	public void Damage(int damageAmount){
		if (!isRespawning){
			health -= damageAmount;
		}
	}
}