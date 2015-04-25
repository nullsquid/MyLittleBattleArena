using UnityEngine;
using System.Collections;
public class MapEditor : MonoBehaviour {
	//[SerializeField] private string fileName;
	[SerializeField] private string mapName;
	[SerializeField] private int mapID;
	[SerializeField] private BoxCollider2D backgroundPlane;
	[SerializeField] private Transform backgroundGeometry;
	[SerializeField] private Transform root, mirrorRoot;
	[SerializeField] private GameObject[] levelTiles;
	public bool inEditMode = true;
	private int currentTileIndex;
	private const float zDistance = 0f;
	[SerializeField] private Transform tileBrushTr;
	[SerializeField] private Vector2 mapDimensions;
	private MapTile[,] mapData, mirrorMapData;
	private bool changedDataThisFrame = false;
	private Vector3 lastPlacementPosition, lastDeletePosition;
	readonly Vector3 resetCursorPosition = Vector3.one * -1f;
	private void Awake () {
		ResizeMap();
		LoadMap();
	}
	private void ResizeMap () {
		foreach (Transform t in root){
			Destroy(t.gameObject);
		}
		foreach (Transform t in mirrorRoot){
			Destroy(t.gameObject);
		}
		mapData = new MapTile[(int)mapDimensions.x, (int)mapDimensions.y];
		backgroundPlane.offset = mapDimensions * 0.5f;
		backgroundPlane.size = mapDimensions;
		backgroundPlane.transform.position = Vector3.zero;
		backgroundGeometry.position = mapDimensions * 0.5f;
		backgroundGeometry.position = new Vector3(backgroundGeometry.position.x - 0.5f, backgroundGeometry.position.y - 0.5f, 10f);
		backgroundGeometry.localScale = mapDimensions;
		Camera.main.orthographicSize = mapDimensions.y * 0.5f;
		Camera.main.transform.position = mapDimensions * 0.5f;
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - 0.5f, Camera.main.transform.position.y - 0.5f, -10f);
		mirrorRoot.position = new Vector3(mapDimensions.x - 1, mapDimensions.y - 1, mirrorRoot.position.z);
		mirrorRoot.localEulerAngles = new Vector3(0f,0f,180f);
	}
	private void HandleSavingAndLoading(){
		if (Input.GetKey(KeyCode.LeftShift)){
			if (Input.GetKeyDown(KeyCode.Alpha1)){
				mapID = 1;
			} else if (Input.GetKeyDown(KeyCode.Alpha2)){
				mapID = 2;
            } else if (Input.GetKeyDown(KeyCode.Alpha3)){
				mapID = 3;
            } else if (Input.GetKeyDown(KeyCode.Alpha4)){
				mapID = 4;
            } else if (Input.GetKeyDown(KeyCode.Alpha5)){
				mapID = 5;
            } else if (Input.GetKeyDown(KeyCode.Alpha6)){
				mapID = 6;
            } else if (Input.GetKeyDown(KeyCode.Alpha7)){
				mapID = 7;
			} else if (Input.GetKeyDown(KeyCode.Alpha8)){
				mapID = 8;
			} else if (Input.GetKeyDown(KeyCode.Alpha9)){
				mapID = 9;
			} else if (Input.GetKeyDown(KeyCode.Alpha0)){
				mapID = 0;
            }
        }else if (Input.GetKeyDown(KeyCode.F5)){
			SaveMap();
		}else if (Input.GetKeyDown(KeyCode.F6)){
			LoadMap();
		}
	}
	private void SaveMap(){
		MapEditorSaving.MapInfo saveMapData = new MapEditorSaving.MapInfo();
		saveMapData.id = mapID;
		saveMapData.mapName = mapName;
		saveMapData.mapWidth = Mathf.RoundToInt(mapDimensions.x);
		saveMapData.mapheight = Mathf.RoundToInt(mapDimensions.y);
		for (int y = 0; y < mapData.GetLength(1); y++){
			for (int x = 0; x < mapData.GetLength(0); x++){
				MapTile targetTile = mapData[x,y];
				if (targetTile != null && !targetTile.isTheMirrorVersion){
					MapEditorSaving.TileInfo newTile = new MapEditorSaving.TileInfo();
					newTile.xPos = x;
					newTile.yPos = y;
					newTile.rotation = Mathf.RoundToInt(targetTile.transform.eulerAngles.z);
					newTile.type = targetTile.typeIndex;
					saveMapData.tiles.Add(newTile);
				}
			}
		}
		MapEditorSaving.SaveFile(saveMapData, mapID.ToString());
	}
	private void LoadMap(){
		MapEditorSaving.MapInfo loadedMap = MapEditorSaving.LoadFile(mapID.ToString());
		if (loadedMap != null){
			mapDimensions = new Vector2(loadedMap.mapWidth, loadedMap.mapheight);
			ResizeMap();
			mapName = loadedMap.mapName;
			mapID = loadedMap.id;
			foreach (MapEditorSaving.TileInfo tile in loadedMap.tiles){
				currentTileIndex = tile.type;
				Quaternion newRotation = Quaternion.Euler(new Vector3(0f,0f,tile.rotation));
				CreateAtPosition(tile.xPos, tile.yPos, newRotation);
			}
		}
	}
	private void Update () {
		if (inEditMode){
			HandlePlacingTile();
			HandleChangingTile();
			HandleSavingAndLoading();
		}
		if (tileBrushTr.gameObject.activeSelf != inEditMode){
			tileBrushTr.gameObject.SetActive(inEditMode);
		}
		changedDataThisFrame = false;
    }
	private void HandlePlacingTile(){
		bool lmbDown = Input.GetMouseButton(0);
		bool rmbDown = Input.GetMouseButton(1);
		bool rButtonDown = Input.GetKeyDown(KeyCode.R);
		Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		int xPos = Mathf.RoundToInt(Mathf.Clamp(mousePoint.x, 0, mapDimensions.x));
		int yPos = Mathf.RoundToInt(Mathf.Clamp(mousePoint.y, 0, mapDimensions.y));
		tileBrushTr.position = new Vector3(xPos, yPos, -1f);
        if (lmbDown || rmbDown || rButtonDown){
			Collider2D hit = Physics2D.OverlapPoint(mousePoint, Layer.Level.ToMask());
			if (hit != null){
				if (lmbDown && lastPlacementPosition != tileBrushTr.position){
                    DeleteAtPosition(xPos, yPos);
                    CreateAtPosition(xPos, yPos);
					lastPlacementPosition = tileBrushTr.position;
					lastDeletePosition = resetCursorPosition;
				} else if (rmbDown && lastDeletePosition != tileBrushTr.position){
					DeleteAtPosition(xPos, yPos);
					lastPlacementPosition = resetCursorPosition;
					lastDeletePosition = tileBrushTr.position;
				}else if (rButtonDown){
					RotateAtPosition(xPos, yPos);
                }
				changedDataThisFrame = true;
            }
		}
    }
	private void CreateAtPosition(int xPos, int yPos){
		CreateAtPosition(xPos, yPos, tileBrushTr.rotation);
	}
	private void CreateAtPosition(int xPos, int yPos, Quaternion targetRotation){
		MapTile originalTile = CreateAtPosition(xPos, yPos, root, targetRotation);
		Vector2 mirrorPosition = mirrorRoot.TransformPoint(originalTile.transform.localPosition);
		//Debug.Log(mirrorPosition + " " + originalTile.transform.localPosition);
		MapTile mirrorTile = CreateAtPosition(Mathf.RoundToInt(mirrorPosition.x), Mathf.RoundToInt(mirrorPosition.y), mirrorRoot, originalTile.transform.localRotation);
		originalTile.mirrorEquivalent = mirrorTile;
		mirrorTile.mirrorEquivalent = originalTile;
		mirrorTile.isTheMirrorVersion = true;
	}
	private MapTile CreateAtPosition(int xPos, int yPos, Transform targetRoot, Quaternion targetRotation){
		MapTile newMapTile = null;
		GameObject newTileGo = Instantiate(levelTiles[currentTileIndex]) as GameObject;
		Transform newTileTr = newTileGo.transform;
		if (targetRoot != null){
			newTileTr.SetParent(targetRoot);
		}
		newTileTr.position = new Vector3(xPos,yPos,zDistance);
		newTileTr.localRotation = targetRotation;
		newMapTile = newTileGo.GetComponent<MapTile>();
		if (newMapTile == null){
			newMapTile = newTileGo.AddComponent<MapTile>();
		}
		newMapTile.typeIndex = currentTileIndex;
        if (mapData[xPos, yPos] == null){
            mapData[xPos, yPos] = newMapTile;
        }
		//Debug.Log(newTileTr.localPosition + "  " + newTileTr.name);
		return newMapTile;
	}
	private void RotateAtPosition(int xPos, int yPos){
		if (!changedDataThisFrame){
			if (mapData[xPos, yPos] != null){
				Vector3 currentTargetRotation = tileBrushTr.eulerAngles;
				mapData[xPos, yPos].transform.eulerAngles = new Vector3(currentTargetRotation.x, currentTargetRotation.y, currentTargetRotation.z + 90f);
				if (mapData[xPos, yPos].mirrorEquivalent != null){
					mapData[xPos, yPos].mirrorEquivalent.transform.localRotation = mapData[xPos, yPos].transform.localRotation;
				}
			}else{
				float currentPlacementRotation = tileBrushTr.eulerAngles.z;
				currentPlacementRotation += 90f;
				if (currentPlacementRotation >= 360f){
					currentPlacementRotation -= 360f;
				}
				if (currentPlacementRotation < 0f){
	                currentPlacementRotation += 360f;
	            }
				tileBrushTr.eulerAngles = new Vector3(0f, 0f, currentPlacementRotation);
	        }
        }
    }
    private void DeleteAtPosition(int xPos, int yPos){
		if (mapData[xPos, yPos] != null){
			if (mapData[xPos, yPos].mirrorEquivalent != null){
				Destroy(mapData[xPos, yPos].mirrorEquivalent.gameObject);
			}
            Destroy(mapData[xPos, yPos].gameObject);
			mapData[xPos, yPos] = null;
		}
    }
    private void HandleChangingTile(){
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if(mouseWheel != 0){
			if (mouseWheel > 0){
				ChangeTileIndex(true);
			}else if (mouseWheel < 0){
				ChangeTileIndex(false);
			}
            SetTileCursorMeshToCurrentPrefab();
        }
    }
    private void ChangeTileIndex(bool nextTile){
		if (levelTiles.Length > 0){
			if (nextTile){
				currentTileIndex++;
				if (currentTileIndex >= levelTiles.Length){
					currentTileIndex = 0;
				}
			}else{
				currentTileIndex--;
				if (currentTileIndex < 0){
					currentTileIndex = levelTiles.Length - 1;
				}
			}
			if (currentTileIndex >= 0 && currentTileIndex < levelTiles.Length && levelTiles[currentTileIndex] == null){
				ChangeTileIndex(nextTile);
	        }
        }
    }
	void SetTileCursorMeshToCurrentPrefab(){
		MeshFilter prefabMeshFilter = levelTiles[currentTileIndex].GetComponent<MeshFilter>();
		if (prefabMeshFilter != null){
			tileBrushTr.GetComponent<MeshFilter>().sharedMesh = prefabMeshFilter.sharedMesh;
		}
    }
}
