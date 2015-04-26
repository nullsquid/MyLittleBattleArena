using UnityEngine;
using System.Collections;

public class PlayerTeamManager : MonoBehaviour {
	public static PlayerTeamManager instance;
	public PlayerTeam blueTeam, redTeam;
	private void Awake(){
		instance = this;
	}
}
