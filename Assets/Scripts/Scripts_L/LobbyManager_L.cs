using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager_L : MonoBehaviourPunCallbacks
{
    public GameObject mapMaker;
    GameObject clickedObject;
    GameObject outlineImage;
    string clickedRoomName;
    public InputField searchInputField;
    public GameObject roomPrefab;
    public GameObject roomPanel;
    public Transform roomsPanel;
    List<Room_H> spaceName = new List<Room_H>();
    DetailMajorManager_L detailMajorManager;
    // Start is called before the first frame update
    void Start()
    {
        clickedRoomName = "RoomScene_H";
        LoadRoomList();
        searchInputField.onValueChanged.AddListener(OnSearchFilterValueChange);
        detailMajorManager = GameObject.Find("DetailMajorManager").GetComponent<DetailMajorManager_L>();
    }

    void OnSearchFilterValueChange(string value)
    {

    }

    void LoadRoomList()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Resources_H/MapData");
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.txt");
        for(int i =0; i < fileInfos.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            GameObject room = Instantiate(roomPrefab);
            room.transform.SetParent(roomsPanel, false);
            room.GetComponent<Room_H>().roomName = fileInfos[i].Name;
            StreamReader sr = fileInfos[i].OpenText();
            SpaceInfo spinfio = JsonUtility.FromJson<SpaceInfo>(sr.ReadToEnd());
            room.GetComponent<Room_H>().roomInfoText = "방 설명 : " + spinfio.spaceIntroduction;
            spaceName.Add(room.GetComponent<Room_H>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClickedObject();
        }

    }

    void CheckClickedObject()
    {
        clickedObject = EventSystem.current.currentSelectedGameObject;
        
        if (clickedObject)
        {
            switch (clickedObject.tag)
            {
                case "LobbyRoom":
                    if(outlineImage == clickedObject.transform.GetChild(0).gameObject)
                    {
                        //clickedRoomName = clickedObject.GetComponentInChildren<Text>().text;
                        SpaceInfo.spaceName = clickedObject.GetComponent<Room_H>().roomName;
                        //PhotonNetwork.Destroy(detailMajorManager.player);
                        LeaveRoom();
                        //SceneManager.LoadScene(clickedRoomName);
                        break;
                    }
                    if (outlineImage)
                    {
                        outlineImage.SetActive(false);
                    }
                    outlineImage = clickedObject.transform.GetChild(0).gameObject;
                    outlineImage.SetActive(true);
                    break;
            }
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

        if (PhotonNetwork.IsConnectedAndReady)
            JoinUserRoom();

        print("OnJoinedLobby 호출");

    }

    void JoinUserRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(SpaceInfo.spaceName, roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방으로 이동");
        //PhotonNetwork.LoadLevel(SpaceInfo.spaceName);
        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene(clickedRoomName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collision.gameObject.GetPhotonView().IsMine)
        {
            mapMaker.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetPhotonView().IsMine)
        {
            mapMaker.SetActive(false);
        }
    }



}
