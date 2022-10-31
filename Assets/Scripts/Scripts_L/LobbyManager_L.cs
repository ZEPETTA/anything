using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager_L : MonoBehaviourPun
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
                        PhotonNetwork.Destroy(detailMajorManager.player);
                        SceneManager.LoadScene(clickedRoomName);
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
    public void MakeMap()
    {
        SpaceInfo info = new SpaceInfo();
        string jsonMap = JsonUtility.ToJson(info, true);
        File.WriteAllText(Application.dataPath + "/Resources/Resources_H/MapData" + "/" + SpaceInfo.spaceName + ".txt", jsonMap);
        SceneManager.LoadScene("RoomScene_H");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            mapMaker.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mapMaker.SetActive(false);
        }
    }

}
