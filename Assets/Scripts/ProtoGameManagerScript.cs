using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProtoGameManagerScript : MonoBehaviour {



	static public int P1Deaths, P2Deaths, P3Deaths, P4Deaths;  

	//not sure to use an array here....

	public int[] Deaths;

	private int RedDeaths, BlueDeaths; //dire stats, for score?

	public int RedTowersRemaining, BlueTowersRemaning; //important stats

	private int BombsExploded; //fun stats......for each player?

	public Transform RedHub, BlueHub;

	public List<GameObject> Players = new List<GameObject>();    // a real-world example of declaring a List of 'GameObjects'



	// Use this for initialization
	void Start () {
	//find all players
	}




	void BodyCounter(int playerNum){

		Deaths[playerNum] += 1; //too simple?


	}

	public int  totalDeaths(){
		int tempTotalDeaths = 0; //clear!
		foreach (int death in Deaths){
			tempTotalDeaths += death;
		}
		return tempTotalDeaths;
	}
}
