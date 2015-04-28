using UnityEngine;
using System.Collections;
public enum Layer{
	Default = 0,
	TransparentFX = 1,
	IgnoreRaycast = 2,
	Water = 4,
	Player = 8,
	Level = 9,
	Buildings = 10,
	Projectiles = 11,
	Creep = 12,
	TowerProjectiles = 13
}
internal static class LayerExensions{
	public static int ToIndex(this Layer layer){
		return (int) layer;
	}
	public static int ToMask(this Layer layer){
		return 1 << (int) layer;
	}
}
