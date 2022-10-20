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
public class TileList
{
   public List<TileInfo> tileList;
}


[System.Serializable]
public class TileInfo
{
    public Vector3 position;
    public string imageName;
}

[System.Serializable]
public class UserInfo
{
    public string id;
    public string password;
    public string name;
    public string collegeName;
}

public class RoomManager_H : MonoBehaviour
{
    public GameObject tilePrefab;
    public MeshRenderer bg;
    // Start is called before the first frame update
    void Start()
    {
        #region 배경화면 가져오기
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/mapdata.txt";
        string jsonData = File.ReadAllText(bgpath);
        MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        Texture2D bgTexture = new Texture2D(info.mapWidth, info.mapHeight);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        #endregion
        #region 타일 가져오기
        string tilepath = Application.dataPath + "/Resources/Resources_H/MapData/tiledata.txt";
        string tilejsonData = File.ReadAllText(tilepath);
        TileList tileList = JsonUtility.FromJson<TileList>(tilejsonData);
        for(int i =0; i<tileList.tileList.Count; i++)
        {
            Vector3 tilePos = tileList.tileList[i].position;
            Texture tileSprite = Resources.Load<Texture>("Resources_L/" + tileList.tileList[i].imageName);
            GameObject tile = Instantiate(tilePrefab);
            tile.transform.position = tilePos;
            tile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tileSprite);
            tile.transform.parent = GameObject.Find("TileParent").transform;
        }
        #endregion

    }
    // Update is called once per frame
    void Update()

    {



    }
}