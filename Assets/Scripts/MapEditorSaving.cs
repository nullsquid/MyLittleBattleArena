//Here are all the namespaces that this script will use.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
public static class MapEditorSaving {
	#region Fields
	private const string fileExtension = ".xml";	//Should always be .xml, so we make it const
	public const string defaultFileName = "XMLExample";	//When we do not give
	private static string filePath{
		get{
			Debug.Log( Application.dataPath + "/../GameData/");
			return Application.dataPath + "/../GameData/";	//Saves to the resources folder.
		}
	}
	#endregion
	[XmlRoot("Map")]	//This is the root tag of the xml file as a whole. Everything in the file will be contained within these xml tags: <map></map>. The second /map is a closing tag.
	[System.Serializable]	//This exposes the data in the inspector for manual editing.
	public class MapInfo{
		public int id = 0;	//The actual data for the id goes here.
		public string mapName = string.Empty;	//The map name, of course
		public int mapWidth = 0;
		public int mapheight = 0;
		[XmlElement("Tiles")]	//XMLElements are additional tags, and can have their own attributes. E.g. <map id=128 mapName="The Fragile"><PlayList></PlayList></map>
		public List<TileInfo> tiles = new List<TileInfo>();	//Creates a playlists for this map.
		#region Class Methods
		public void Save(string path){	//Saves this current class to the given file path.
			Debug.Log("Saving XML file to " + path);
			var serializer = new XmlSerializer(typeof(MapInfo));	//This object encodes all the data that is passed to it through the FileStream.
			using(var stream = new FileStream(path, FileMode.Create)){
				serializer.Serialize(stream, this);	//The file is created!
			}
		}
		public static MapInfo Load(string path){	//Loads the current class from the given path.
			Debug.Log("Loading XML file from " + path);
			var serializer = new XmlSerializer(typeof(MapInfo));	//This serializer decoded the xml data.
			using(var stream = new FileStream(path, FileMode.Open)){
				return serializer.Deserialize(stream) as MapInfo;	//The file is loaded and returned.
			}
		}
		public static MapInfo LoadFromText(string text){	//Instead of loading from a file, what if we just gave the serializer the raw data from a string?
			var serializer = new XmlSerializer(typeof(MapInfo));	//Creates the xml serialzer to decode the data.
			return serializer.Deserialize(new StringReader(text)) as MapInfo;	//Loads the data and returns an object of the class type.
		}
		#endregion
	}
	[System.Serializable]	//This exposes the data in the inspector for manual editing.
	public class TileInfo{
		public int xPos = 0;
		public int yPos = 0;
		public float rotation = 0;
		public int type = 0;
	}
	#region Save and Load Methods
	/// <summary>
	/// Saves the file to the specified path.
	/// </summary>
	public static void SaveFile(MapInfo data, string fileName){
		string fullPath = filePath + fileName + fileExtension;
		data.Save(fullPath);
	}
	/// <summary>
	/// Loads the file from the specified path.
	/// </summary>
	public static MapInfo LoadFile (string fileName) {
		string fullPath = filePath + fileName + fileExtension;
		return MapInfo.Load(fullPath);
	}
	#endregion
}