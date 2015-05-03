using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTeam : MonoBehaviour {
	public enum TeamName {NONE, BLUE, RED}
	public TeamName teamName;
	public Material teamMaterial;
	public List<CharacterBase> characters = new List<CharacterBase>();
	public List<Building> buildings = new List<Building>();
	public List<Tower> towers = new List<Tower>();
	public Hub teamHub;
	public int teamDeaths{
		get{
			int deathCount = 0;
			foreach (CharacterBase c in characters){
				deathCount += c.deaths;
			}
			return deathCount;
		}
	}
}
