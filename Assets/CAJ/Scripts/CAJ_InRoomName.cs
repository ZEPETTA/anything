using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CAJ_InRoomName : MonoBehaviourPunCallbacks
{
    //방 목록
    public Transform roomListContent;
    //모든 룸 roomInfo -> 딕셔너리(키, 밸류값)
    //키값 : 방 이름(string) , 밸류값 : Roominfo
    private Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //방 버튼 프리팹
    public GameObject roomItemFactory;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
    
    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        //자신의 게임 오브젝트 이름 -> roomName
        //name = roomName;
        //name = PhotonNetwork.CurrentRoom.Name;
        
        //방 정보 세팅 -> 방이름 (1/10)
        //roomInfo.text = roomName + "(" + currPlayer + "/" + maxPlayer + ")";
        //roomInfo.text = PhotonNetwork.CurrentRoom.Name + "(" + currPlayer + "/" + maxPlayer + ")";
        //roomInfo.text = PhotonNetwork.CurrentRoom.Name + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")";
    }
}
