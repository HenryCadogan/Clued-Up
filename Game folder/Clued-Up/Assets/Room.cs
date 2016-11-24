using UnityEngine;
using System.Collections;

[System.Serializable]
public class RoomInfo {

    public string name, description, backgroundPath;

    public static RoomInfo GetInfoFromJson(string JsonString)
    {
        return JsonUtility.FromJson<RoomInfo>(JsonString);
    }
    
}
