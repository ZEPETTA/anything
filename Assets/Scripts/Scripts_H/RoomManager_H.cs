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
    public List<TileInfo> tileList;
    public enum TileType
    {
        Nomal,
        Potal,
        DefinedArea,

    }
    public TileType tileType;
    //public Vector3 scale;

    //public Vector3 angle;
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
    public GameObject[] tilePrefab;
    public MeshRenderer bg;
    // Start is called before the first frame update
    void Start()
    {
        #region ���ȭ�� ��������
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/mapdata.txt";
        string jsonData = File.ReadAllText(bgpath);
        MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        Texture2D bgTexture = new Texture2D(info.mapWidth, info.mapHeight);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        #endregion
        #region Ÿ�� ��������
        for(int i =0; i<info.tileList.Count; i++)
        {
            Vector3 tilePos = info.tileList[i].position;
            Texture tileSprite = Resources.Load<Texture>("Resources_L/" + info.tileList[i].imageName);
            GameObject tile = new GameObject();
            switch (info.tileType)
            {
                case MapInfo.TileType.Nomal:
                    tile = tilePrefab[0];
                    break;
                case MapInfo.TileType.Potal:
                    tile = tilePrefab[1];
                    break;
                case MapInfo.TileType.DefinedArea:
                    tile = tilePrefab[2];
                    break;
            }
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