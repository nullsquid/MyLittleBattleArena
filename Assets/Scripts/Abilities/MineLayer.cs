using UnityEngine;
using System.Collections;

public class MineLayer : MonoBehaviour {


	public int damagePerShot = 20;
	private float timeBetweenMines = 1.0f;
	public float range = 100f;
	
	public GameObject minePrefab;

	float timer;

	AudioSource MineAudio;

	// Use this for initialization


	void Start () {
	//Set Color

	}

	void SetTag(string teamColor){
		this.tag = teamColor;
	
		
	}

	void Update(){
		timer += Time.deltaTime;

		if(timer >= timeBetweenMines)
		{
			//print("Mine redy");
		}
	}

	void TryToLayMine () {
		if( timer >= timeBetweenMines && Time.timeScale != 0)
		{
			LayMine ();
		}

		//send team color and timer
	}

	void LayMine(){
		timer = 0f;
		GameObject mine = Instantiate(minePrefab,transform.position,transform.rotation) as GameObject;
		mine.GetComponent<MineTimer>().Lifetime = 1.0f;
		mine.GetComponent<MineTimer>().teamColor = this.tag;
	}
}
