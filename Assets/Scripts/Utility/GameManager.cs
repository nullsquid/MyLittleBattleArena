using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public bool matchInProgress = true;	//TODO false until the play button is pressed.
	public static bool isInMatch {
		get{
			return instance.matchInProgress && !MapEditor.instance.inEditMode;
		}
	}
	private void Awake(){
		instance = this;
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}
}
