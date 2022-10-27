using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class MainManager_L : MonoBehaviourPunCallbacks
{
    GameObject clickedObject;
    GameObject outlineImage;
    string clickedRoomName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoInfoScene()
    {
        SceneManager.LoadScene("MyInfoScene_H");
    }

    public void GoLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene_L");
    }

    public void OnClickMajor()
    {
        clickedObject = EventSystem.current.currentSelectedGameObject;

        if (clickedObject)
        {
            switch (clickedObject.tag)
            {
                case "MainRoom":
                    if (outlineImage == clickedObject.transform.GetChild(0).gameObject)
                    {
                        //clickedRoomName = clickedObject.GetComponentInChildren<Text>().text;
                        //MapInfo.mapName = clickedObject.GetComponent<Room_H>().roomName;
                        //SceneManager.LoadScene("RoomScene_H");
                        //SceneManager.LoadScene(clickedObject.GetComponent<MajorName_L>().majorSceneName);
                        JoinRoom();
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

    //방 참가 요청
    public void JoinRoom()
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        //hash["password"] = 
        //roomOptions.CustomRoomProperties = hash;
        //roomOptions.CustomRoomPropertiesForLobby = new string[]
        //{
        //    "password"
        //};
        PhotonNetwork.JoinOrCreateRoom(clickedObject.GetComponent<MajorName_L>().majorSceneName,roomOptions,null);
       // PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    

    //방 참가가 완료 되었을 때 호출 되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel(clickedObject.GetComponent<MajorName_L>().majorSceneName);
    }

    //방 참가가 실패 되었을 때 호출 되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }
}
