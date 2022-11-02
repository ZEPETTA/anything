using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class SpaceInfo
{
    public List<MapInfo> mapList;
    public string makerID;
    public static string spaceName;
    public string spaceIntroduction;
}

[System.Serializable]
public class MapInfo
{
    public static string nowMapName;
    public string mapName;
    public enum MapSize
    {
        Big,
        Mideum,
        Small
    }
    public MapSize mapSize;
    public byte[] backGroundImage;
    public List<TileInfo> tileList;
    public List<string> areaName;
    public List<WallInfo> wallList;
    public List<PortalInfo> portalList;
    public List<DefinedAreaInfo> definedAreaList;
    public List<ObjectInfo> objectList;
    public List<SpawnPointInfo> spawnPointInfoList;
}
[System.Serializable]
public class ObjectInfo
{
    public string objName;
    public int objWidth;
    public int objHeight;
    public Vector3 Position;
    public enum ObjectType
    {
        Text,
        Image,
    }
    public ObjectType objType;
    public bool upperObj;
    public byte[] image;
    public string text;
    public enum ObjectSkill
    {
        nomalObj,
        urlObj,
        changeObj,
        talkingObj,
    }
    public ObjectSkill objSkill;
    public string urlSkill;
    public byte[] changeSkill;
    public string talkingSkill;
    //애니메이션 어케해야할지 고민중
    public string textSkill;
    public byte[] imageSkill;

    public enum InteractionType
    {
        pressF,
        touch,
    }
    public InteractionType interactionType;
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
public class SpawnPointInfo
{
    public Vector3 position;
}

[System.Serializable]
public class UserInfo
{
    public string id;
    public string password;
    public string name;
    public string phoneNumber;
    public string departmentName;
}

public class RoomManager_H : MonoBehaviourPun
{
    public static RoomManager_H instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public SpaceInfo spaceInfo;
    //public MapInfo mapInfo;
    public GameObject wallPrefab;
    public GameObject tilePrefab;
    public GameObject definedAreaPrefab;
    public GameObject portalPrefab;
    public GameObject spawnPointPrefab;
    public GameObject objectPrefab;
    public List<Vector3> spawnPointPosList; //로드 시, 리스트에 위치 넣어두고 랜덤 위치에 캐릭터 옮김
    public Material invisible;
    public MeshRenderer bg;
    public GameObject quadBG;

    public GameObject player;
    public Transform playerSpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        #region 배경화면 가져오기
        string bgpath = Application.dataPath + "/Resources/Resources_H/MapData/"+ SpaceInfo.spaceName +".txt";
        string jsonData = File.ReadAllText(bgpath);
        spaceInfo = JsonUtility.FromJson<SpaceInfo>(jsonData);
        //MapInfo info = JsonUtility.FromJson<MapInfo>(jsonData);
        MapInfo info = new MapInfo();
        for(int i=0; i<spaceInfo.mapList.Count; i++)
        {
            //Debug.Log(spaceInfo.mapList[i].mapName);
             if(spaceInfo.mapList[i].mapName == MapInfo.nowMapName)
            {
                info = spaceInfo.mapList[i];
                Debug.Log("mynameis" + MapInfo.nowMapName);
                break;
            }
            if(i == spaceInfo.mapList.Count - 1)
            {
                info = spaceInfo.mapList[0];
                MapInfo.nowMapName = info.mapName;
            }
        }
        //mapInfo = info;
        if(info.mapSize == MapInfo.MapSize.Big)
        {
            quadBG.transform.localScale = new Vector3(180,90 , 1);
        }
        else if(info.mapSize == MapInfo.MapSize.Mideum)
        {
            quadBG.transform.localScale = new Vector3(120, 60, 1);
        }
        else
        {
            quadBG.transform.localScale = new Vector3(60, 30, 1);
        }
        Texture2D bgTexture = new Texture2D(0, 0);
        bgTexture.LoadImage(info.backGroundImage);
        bg.material.SetTexture("_MainTex", bgTexture);
        #endregion
        #region 타일 가져오기
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
        #region 포탈 가져오기
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
        #region 지정구역 가져오기
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
        #region 벽 가져오기
        for(int i =0; i<info.wallList.Count; i++)
        {
            Vector3 wallPos = info.wallList[i].positon;
            GameObject wall = Instantiate(wallPrefab);
            wall.transform.position = wallPos;
            //wall.GetComponent<MeshRenderer>().material.mainTexture = invisibleTexture;
            wall.transform.parent = GameObject.Find("WallParent").transform;
        }
        #endregion
        #region 스폰 지점 가져오기
        Transform spawnPointParent = GameObject.Find("SpawnPointParent").transform;
        if (spawnPointParent)
        {
            spawnPointPosList = new List<Vector3>();

            for(int i = 0; i < info.spawnPointInfoList.Count; i++)
            {
                GameObject spawnPoint = Instantiate(spawnPointPrefab);
                spawnPoint.transform.parent = spawnPointParent;
                spawnPoint.transform.localPosition = info.spawnPointInfoList[i].position;
                spawnPointPosList.Add(spawnPoint.transform.localPosition);
                
            }
        }
        #endregion
        #region 오브젝트 가져오기
        GameObject objectParent = GameObject.Find("ObjectParent");
        for(int i =0; i< info.objectList.Count; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.transform.position = info.objectList[i].Position;
            Debug.Log("야!!!");
            obj.transform.GetChild(0).gameObject.GetComponent<ObjectInfo_H>().objectInfo = info.objectList[i];
            obj.transform.parent = objectParent.transform;
        }
        #endregion

        //player = PhotonNetwork.Instantiate("$Main_Character_1_0_2D", playerSpawnPos.position, Quaternion.identity);
        //player.GetComponent<CharacterMove_H>().enabled = true;

        if (spawnPointPosList.Count > 0)
        {
            int randPos = Random.Range(0, spawnPointPosList.Count);
            player.transform.position = spawnPointPosList[randPos];
        }


        //스폰 지점에 캐릭터 위치
        //포톤 도입 시 캐릭터 찾는 부분 수정 필요
    }
    // Update is called once per frame
    void Update()

    {



    }
}