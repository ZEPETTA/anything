using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using SFB;

public class FileManager_L : MonoBehaviour
{
    public string testMapName;
    public MeshRenderer bg;
    byte[] mapImage;
    string path = "";
    string savepth;
    int mapWidth;
    int mapHeight;
    public int mapSize = 0;
    public MapInfo[] mapinfos;
    // Start is called before the first frame update
    void Start()
    {
        if(SpaceInfo.spaceName == null)
        {
            SpaceInfo.spaceName = testMapName;
        }
        savepth = Application.dataPath + "/Resources/Resources_H/MapData";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string WriteResult(string[] paths)
    {
        string result = "";
        if (paths.Length == 0)
        {
            return "";
        }
        foreach (string p in paths)
        {
            result += p + "\n";
        }
        return result;
    }
    public void OnClickSetBG()
    {
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        Debug.Log(path);

        StartCoroutine(IESetBG());
    }

    public void OnClickSetFG()
    {
        path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        //path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");

        StartCoroutine(IESetFG());
    }

    IEnumerator IESetBG()
    {
        
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D convertedTexture = (Texture2D)myTexture;
            mapHeight = convertedTexture.height;
            mapWidth = convertedTexture.width;
            byte[] textuerData = convertedTexture.EncodeToPNG();
            if (Directory.Exists(savepth) == false)
            {
                Directory.CreateDirectory(savepth);
            }
            File.WriteAllBytes(savepth + "/mapData.png", textuerData);
            mapImage = textuerData;
            bg.material.SetTexture("_MainTex", convertedTexture);
        }
    }

    IEnumerator IESetFG()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
    public void SaveMap()
    {
        #region 맵 배경 저장
        SpaceInfo spaceInfo = new SpaceInfo();
        MapInfo backGroundInfo = new MapInfo();
        backGroundInfo.backGroundImage = mapImage;
        backGroundInfo.mapWidth = mapWidth;
        backGroundInfo.mapHeight = mapHeight;
        string path = Application.dataPath + "/Resources/Resources_H/MapData";
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
#endregion
#region 맵 노말 타일 저장
        GameObject tileParent = GameObject.Find("TileParent");
        int a = tileParent.transform.childCount;
        List<TileInfo> tileInfos = new List<TileInfo>();
        for (int i =0; i < a; i++)
        {
            GameObject tile = tileParent.transform.GetChild(i).gameObject;
            TileInfo_H tileInfo_h = tile.GetComponent<TileInfo_H>();
            TileInfo tileInfo = tileInfo_h.tileInfo;
            tileInfo.imageName = tile.GetComponent<MeshRenderer>().material.mainTexture.name;
            tileInfo.position = tile.transform.position;
            tileInfos.Add(tileInfo);
        }
        backGroundInfo.tileList = tileInfos;
#endregion
#region 맵 지정구역 저장
        GameObject definedArea = GameObject.Find("DefinedAreaParent");
        List<string> nameList = new List<string>();
        List<DefinedAreaInfo> definedAreaInfos = new List<DefinedAreaInfo>();
        for(int i =0; i<definedArea.transform.childCount; i++)
        {
            GameObject gm = definedArea.transform.GetChild(i).gameObject;
            nameList.Add(gm.name);
            for(int j =0; j<gm.transform.childCount; j++)
            {
                DefinedAreaInfo de = new DefinedAreaInfo();
                de.areaName = gm.name;
                de.positon = gm.transform.GetChild(j).localPosition;
                definedAreaInfos.Add(de);
            }
        }
        backGroundInfo.definedAreaList = definedAreaInfos;
        backGroundInfo.areaName = nameList;
#endregion
#region 맵 포탈 저장
        GameObject portal = GameObject.Find("PortalParent");
        if(portal != null)
        {
            List<PortalInfo> portalInfoList = new List<PortalInfo>();

            for (int i = 0; i < portal.transform.childCount; i++)
            {

                Portal2D_L portalL = portal.transform.GetChild(i).GetComponent<Portal2D_L>();
                portalInfoList.Add(portalL.portalInfo);
                //portalL.portalInfo.position = portal.transform.GetChild(i).localPosition;

            }
            backGroundInfo.portalList = portalInfoList;
        }
        
#endregion
#region 맵 벽(이동불가능 구역)저장
        GameObject wall = GameObject.Find("WallParent");
        List<WallInfo> wallInfos = new List<WallInfo>();
        for(int i =0; i<wall.transform.childCount; i++)
        {
            WallInfo wallInfo = new WallInfo();
            wallInfo.positon = wall.transform.GetChild(i).position;
            wallInfos.Add(wallInfo);
        }
        backGroundInfo.wallList = wallInfos;
#endregion
#region 스폰 지점 저장
        Transform spawnPointParent = GameObject.Find("SpawnPointParent").transform;
        if (spawnPointParent)
        {
            List<SpawnPointInfo> spawnPointInfoList = new List<SpawnPointInfo>();
            for(int i = 0; i < spawnPointParent.childCount; i++)
            {
                SpawnPointInfo info = new SpawnPointInfo();
                info.position = spawnPointParent.GetChild(i).localPosition;
                spawnPointInfoList.Add(info);

            }
            backGroundInfo.spawnPointInfoList = spawnPointInfoList;
        }
#endregion
#region 오브젝트 저장
        GameObject objectP = GameObject.Find("ObjectParent");
        List<ObjectInfo> objectInfos = new List<ObjectInfo>();
        for(int i =0; i < objectP.transform.childCount; i++)
        {
            objectInfos.Add(objectP.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ObjectInfo_H>().objectInfo);
        }
        backGroundInfo.objectList = objectInfos;
        #endregion
        #region 맵사이즈 저장
        switch (mapSize)
        {
            case 0:
                backGroundInfo.mapSize = MapInfo.MapSize.Small;
                break;
            case 1:
                backGroundInfo.mapSize = MapInfo.MapSize.Mideum;
                break;
            case 2:
                backGroundInfo.mapSize = MapInfo.MapSize.Big;
                break;
        }
        #endregion
        List<MapInfo> mapInfos = new List<MapInfo>();
        mapInfos.Add(backGroundInfo);
        spaceInfo.mapList = mapInfos;
        string jsonMap = JsonUtility.ToJson(spaceInfo);
        File.WriteAllText(path + "/" + SpaceInfo.spaceName +".txt", jsonMap);
        SceneManager.LoadScene("RoomScene_H");
    }
}
