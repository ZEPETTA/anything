using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;

public class LobbyManager_L : MonoBehaviour
{
    public GameObject mapMaker;
    GameObject clickedObject;
    GameObject outlineImage;
    string clickedRoomName;
    public GameObject roomPrefab;
    public InputField[] mapMakerInputField;
    public GameObject roomPanel;
    // Start is called before the first frame update
    void Start()
    {
        clickedRoomName = "RoomScene_H";
        LoadRoomList();
    }

    void LoadRoomList()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Resources_H/MapData");
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.txt");
        GameObject rooms = GameObject.Find("RoomsPanel");
        for(int i =0; i < fileInfos.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            GameObject room = Instantiate(roomPrefab);
            room.transform.SetParent(roomPanel.transform, false);
            room.GetComponent<Room_H>().roomName = fileInfos[i].Name;
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
                        MapInfo.mapName = clickedObject.GetComponent<Room_H>().roomName;
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
    public void OnClickCreateRoom()
    {
        mapMaker.SetActive(true);
    }
    public void OnQuit()
    {
        mapMaker.SetActive(false);
    }
    public void MakeMap()
    {
        SpaceInfo.spaceName = mapMakerInputField[0].text;
        SpaceInfo spaceInfo = new SpaceInfo();
        MapInfo.mapName = mapMakerInputField[0].text;
        MapInfo info = new MapInfo();
        List<MapInfo> mapInfos = new List<MapInfo>();
        mapInfos.Add(info);
        spaceInfo.mapList = mapInfos;
        string jsonMap = JsonUtility.ToJson(spaceInfo, true);
        File.WriteAllText(Application.dataPath + "/Resources/Resources_H/MapData" + "/" + SpaceInfo.spaceName + ".txt", jsonMap);
        SceneManager.LoadScene("RoomScene_H");
    }

}
