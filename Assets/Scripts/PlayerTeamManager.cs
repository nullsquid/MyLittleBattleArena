using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTeamManager : MonoBehaviour {
	public static PlayerTeamManager instance;
	public PlayerTeam blueTeam, redTeam;
	public Text winText;
	public Image winBG;
	private bool allowReset;
	private bool gameOver;
	private void Awake(){
		instance = this;
		winText.text = string.Empty;
		winText.enabled = false;
		winBG.enabled = false;
	}
	void Update(){
		if (!gameOver){
			if (blueTeam.teamHub != null && !blueTeam.teamHub.isActiveAndEnabled){
				winBG.color = redTeam.teamMaterial.color;
				winText.text = "RED TEAM WINS";
				gameOver = true;
				StartCoroutine(ResetTimer());
			}else if (redTeam.teamHub != null && !redTeam.teamHub.isActiveAndEnabled){
				winBG.color = blueTeam.teamMaterial.color;
				winText.text = "BLUE TEAM WINS";
				gameOver = true;
				StartCoroutine(ResetTimer());
			}
		}else if (allowReset){
			if (Input.anyKey){
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
	IEnumerator ResetTimer(){
		yield return new WaitForSeconds(3f);
		allowReset = true;
	}
}
