using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class PortalInfo
{
    public enum MoveType
    {
        Key,
        Instant
    }

    public enum PlaceType
    {
        OtherMap,
        DefinedArea,
        OtherSpace,
        DetailMajor
    }
    //Key = F키 이동
    //Instant = 즉시 이동

    public MoveType moveType;
    public PlaceType placeType;

    public string definedAreaName;
    public string mapName;
    public Vector3 position;
    //이름으로 mapInfo에서 해당 이름의 definedArea의 배열(또는 리스트)을 찾을 수 있어야 함
    //찾은 area를 Portal_L에서 받아서, 그 중 하나로 이동할 수 있어야 함
}

public class Portal2D_L : MonoBehaviourPunCallbacks
{
    //포털과 충돌할 수 있도록, 포털 오브젝트의 z값이 플레이어와 닿도록 설정하기
    public PortalInfo portalInfo;
    public float playerZ = 0f;
    GameObject definedAreaParent;
    bool onPlayer = false;
    bool goOtherRoom = false;
    GameObject player;

    MajorRoomManager_L majorRoomManager;

    // Start is called before the first frame update
    void Start()
    {
        //portalInfo = new PortalInfo();

        //===테스트용===
/*        portalInfo.definedAreaName = "Test";
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;*/
        //==============

        definedAreaParent = GameObject.Find("DefinedAreaParent");

        //majorRoomManager = GameObject.Find("MajorRoomManager").GetComponent<MajorRoomManager_L>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                MovePlayer(player);
                onPlayer = false;
            }
        }
    }

    void MovePlayer(GameObject collidedObj)
    {
        GameObject player = collidedObj;
        switch (portalInfo.placeType)
        {
            case PortalInfo.PlaceType.OtherMap:
                MapInfo.nowMapName = portalInfo.mapName;
                if (portalInfo.moveType == PortalInfo.MoveType.Instant)
                {
                    SceneManager.LoadScene("RoomScene_H");
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        SceneManager.LoadScene("RoomScene_H");
                    }
                }
                break;
            case PortalInfo.PlaceType.DefinedArea:

                GameObject definedAreaParent = GameObject.FindGameObjectWithTag("DefinedAreaParent");
                GameObject definedAreaParent2 = definedAreaParent.transform.Find(portalInfo.definedAreaName).gameObject;
                Transform[] definedAreas = definedAreaParent2.transform.GetComponentsInChildren<Transform>();
                Transform randomDefinedArea = definedAreas[Random.Range(0, definedAreas.Length)];
                //모든 DefinedArea를 담는 DefinedAreaParent가 있고,
                //각 이름으로 분류된 DefinedArea를 담는 DefinedAreaParent2가 있어야 함
                //예를 들어, 이름이 "Test"인 DefinedArea(지정영역) < DefinedAreaParent2 < DefinedAreaParent
                //수정필요
                player.transform.position = new Vector3(randomDefinedArea.position.x, randomDefinedArea.position.y, playerZ);

                break;
            case PortalInfo.PlaceType.OtherSpace:
                break;
            case PortalInfo.PlaceType.DetailMajor:
                LeaveRoom();
                SpaceInfo.spaceName = portalInfo.mapName;
                //SceneManager.LoadScene("RoomScene_H");
                //PhotonNetwork.LoadLevel(MapInfo.mapName);
                break;
        }

    }

    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    void JoinMajorRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        
        PhotonNetwork.JoinOrCreateRoom(SpaceInfo.spaceName, roomOptions, null);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (goOtherRoom)
        {
            print("다른 방으로");
            print("On Left Room 로비 안에 있나요?? : " + PhotonNetwork.InLobby);
            //JoinLobby();
            //JoinMajorRoom();
        }
        else
        {
            print("로비로 나가기");
            //PhotonNetwork.LoadLevel("LobbyScene_L");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinLobby();
        print("방 나왔음");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

/*        if (majorRoomManager.player)
            PhotonNetwork.Destroy(majorRoomManager.player);*/

        print(PhotonNetwork.CurrentLobby.Name);

        if (PhotonNetwork.IsConnectedAndReady)
            JoinMajorRoom();

        print("OnJoinedLobby 호출");
        
        //print("로비 안에 있나요?? : " + PhotonNetwork.InLobby);

    }

    [PunRPC]
    void PrintCurrLobby()
    {
        print(PhotonNetwork.CurrentLobby.Name);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방으로 이동");
        //PhotonNetwork.LoadLevel(SpaceInfo.spaceName);
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene(SpaceInfo.spaceName);
    }



    /*   private void OnTriggerEnter(Collider other)
       {
           if (other.transform.tag == "Player")
           {
               print("Trigger Entered");
               MovePlayer(other.transform.gameObject);
           }
       }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            print("방 들어감");
            if (portalInfo.moveType == PortalInfo.MoveType.Instant)
            {
                MovePlayer(collision.gameObject);
                goOtherRoom = true;
            }
            else
            {
                player = collision.gameObject;
                onPlayer = true;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            player = null;
            onPlayer = false;
            goOtherRoom = false;
        }
    }
}
