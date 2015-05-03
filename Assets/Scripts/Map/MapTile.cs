using UnityEngine;
using System.Collections;
public class MapTile : MonoBehaviour {
	public int typeIndex {get;set;}
	public MapTile mirrorEquivalent {get;set;}
	public bool isTheMirrorVersion {get;set;}
	public float zDepth;
}
