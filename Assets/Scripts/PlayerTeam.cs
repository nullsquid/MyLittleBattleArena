using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTeam : MonoBehaviour {
	public enum TeamName {NONE, BLUE, RED}
	public TeamName teamName;
	public Material teamMaterial;
	public List<PlayerMovement> characters = new List<PlayerMovement>();
	public List<Building> buildings = new List<Building>();
	public List<Tower> towers = new List<Tower>();
	public Hub teamHub;
}
