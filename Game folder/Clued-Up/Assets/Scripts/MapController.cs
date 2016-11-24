using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapController : MonoBehaviour {
	//declare list to store room data
	public Dictionary<string, Dictionary<string, string>> data;
	TextAsset text;
	string path = "JSON/Rooms";
	JSONObject obj;


	// Use this for initialization
	void Start() {
		// import list of rooms here
		text = Resources.Load(path) as TextAsset;
		//Debug.Log(text);
		obj = new JSONObject(text.ToString());
		Debug.Log(obj.keys[0]);
	
	}

	JSONObject getItem(string name, JSONObject obj) {
		if(obj.GetField(name).type == JSONObject.Type.OBJECT) {
			return obj.GetField(name);
		} else {
			return new JSONObject();
		}
	}


	void accessData(JSONObject obj) {
		switch(obj.type) {
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++) {
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log(key);
				accessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list) {
				accessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log(obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log(obj.n);
			break;
		case JSONObject.Type.BOOL:
			Debug.Log(obj.b);
			break;
		case JSONObject.Type.NULL:
			Debug.Log("NULL");
			break;

		}
    
	}
}