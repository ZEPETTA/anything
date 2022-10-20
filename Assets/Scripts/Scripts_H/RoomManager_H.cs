using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class MapInfo
{

    public byte[] backGroundImage;
    public int mapWidth;
    public int mapHeight;

    //public Vector3 scale;

    //public Vector3 angle;
}

[System.Serializable]
public class TileInfo
{
    public Vector3 position;
    public string imageName;
}




public class RoomManager_H : MonoBehaviour
{
    public MeshRenderer bg;
    // Start is called before the first frame update
    void Start()
    {
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/mapdata.txt";
        string jsonData = File.ReadAllText(bgpath);
        MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        Texture2D bgTexture = new Texture2D(info.mapWidth,info.mapHeight);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex",bgTexture);

    }
    // Update is called once per frame
    void Update()

    {



    }
}