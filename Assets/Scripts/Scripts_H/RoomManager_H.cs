using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class MapInfo
{
    public static string mapName;
    public byte[] backGroundImage;
    public int mapWidth;
    public int mapHeight;
    public List<TileInfo> tileList;
    public List<string> areaName;
    public List<WallInfo> wallList;
    public List<PortalInfo> portalList;
    public List<DefinedAreaInfo> definedAreaList;
    
}
[System.Serializable]
public class ObjectInfo
{
    public Vector3 Position;
    public enum objectType
    {
        Text,
        Image,
    }
}
[System.Serializable]
public class WallInfo
{
    public Vector3 positon;
}
[System.Serializable]
public class DefinedAreaInfo
{
    public string areaName;
    public Vector3 positon;
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
    public GameObject wallPrefab;
    public GameObject tilePrefab;
    public GameObject definedAreaPrefab;
    public GameObject portalPrefab;
    public Material invisible;
    public MeshRenderer bg;
    // Start is called before the first frame update
    void Start()
    {
        #region ���ȭ�� ��������
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/"+ MapInfo.mapName +".txt";
        string jsonData = File.ReadAllText(bgpath);
        MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        Texture2D bgTexture = new Texture2D(info.mapWidth, info.mapHeight);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        #endregion
        #region Ÿ�� ��������
        string tileParent = "TileParent";
        for(int i =0; i<info.tileList.Count; i++)
        {
            Vector3 tilePos = info.tileList[i].position;
            Texture tileSprite = Resources.Load<Texture>("Resources_L/" + info.tileList[i].imageName);
            GameObject realTile = Instantiate(tilePrefab);
            realTile.transform.position = tilePos;
            realTile.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tileSprite);
            realTile.transform.parent = GameObject.Find(tileParent).transform;
        }
        #endregion
        #region ��Ż ��������
        Transform portalParent = GameObject.Find("PortalParent").transform;
        if (portalParent)
        {
            for(int i = 0; i < info.portalList.Count; i++)
            {
                //Vector3 tilePos = info.portalList[i].position;
                GameObject myPortal = Instantiate(portalPrefab);
                Portal2D_L portal2D = myPortal.GetComponent<Portal2D_L>();
                portal2D.portalInfo = new PortalInfo();
                myPortal.transform.parent = portalParent;
                myPortal.transform.localPosition = info.portalList[i].position;

                portal2D.portalInfo.position = myPortal.transform.localPosition;
                portal2D.portalInfo.placeType = info.portalList[i].placeType;
                portal2D.portalInfo.moveType = info.portalList[i].moveType;
                portal2D.portalInfo.definedAreaName = info.portalList[i].definedAreaName;
                portal2D.portalInfo.mapName = info.portalList[i].mapName;
            }
        }
        #endregion
        #region �������� ��������
        GameObject areaParent = GameObject.Find("DefinedAreaParent");
        Dictionary<string, int> nameDic = new Dictionary<string, int>();
        for(int i=0; i < info.areaName.Count; i++)
        {
            GameObject child = new GameObject(info.areaName[i]);
            child.transform.parent = areaParent.transform;
            nameDic.Add(info.areaName[i],i);
        }
        for(int i=0;i<info.definedAreaList.Count; i++)
        {
            GameObject area = Instantiate(definedAreaPrefab);
            area.transform.position = info.definedAreaList[i].positon;
            area.GetComponentInChildren<Text>().text = info.definedAreaList[i].areaName;
            area.transform.parent = areaParent.transform.GetChild(nameDic[info.definedAreaList[i].areaName]);
        }

        #endregion
        #region �� ��������
        for(int i =0; i<info.wallList.Count; i++)
        {
            Vector3 wallPos = info.wallList[i].positon;
            GameObject wall = Instantiate(wallPrefab);
            wall.transform.position = wallPos;
            //wall.GetComponent<MeshRenderer>().material.mainTexture = invisibleTexture;
            wall.transform.parent = GameObject.Find("WallParent").transform;
        }
        #endregion

    }
    // Update is called once per frame
    void Update()

    {



    }
}