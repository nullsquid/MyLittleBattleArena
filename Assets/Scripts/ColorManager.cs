using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour {

	public Material red, blue;
	public static Material RedMaterial, BlueMaterial;


	// Use this for initialization
	void Start () {
		RedMaterial = red;
		BlueMaterial = blue;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void setColorByTag(string tag,GameObject g){
		if(tag == "red"){
			g.GetComponent<MeshRenderer>().material = ColorManager.RedMaterial;
		}
		if(tag == "blue"){
			g.GetComponent<MeshRenderer>().material = ColorManager.BlueMaterial;
		}
		else{
			//non-denominational?
		}
	}
}
