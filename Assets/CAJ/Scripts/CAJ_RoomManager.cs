using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CAJ_RoomManager : MonoBehaviourPunCallbacks
{
    //방 정보 Text
    public Text roomInfo;
    
    //설명 Text
    public Text roomDescription;
    
    //맵 id
    private int mapId;

    // Start is called before the first frame update
    void Start()
    {
        //플레이어 생성
        //PhotonNetwork.Instantiate("CAJ_Player", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate("CAJ_Player", new Vector3(0, 2, 0), Quaternion.identity);
    }
    
    //방에 플레이어가 참여했을 때 호출해주는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "님이 방에 들어왔습니다.");
    }

    // public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    // {
    //     //자신의 게임 오브젝트 이름 -> roomName
    //     name = roomName;
    //     
    //     //방 정보 세팅 -> 방이름 (1/10)
    //     roomInfo.text = roomName + "(" + currPlayer + "/" + maxPlayer + ")";
    // }

    // public void SetInfo(RoomInfo info)
    // {
    //     //SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
    //     SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);
    //     
    //     //방 설명 세팅
    //     roomDescription.text = (string)info.CustomProperties["description"];
    //     
    //     //맵 id셋팅
    //     mapId = (int)info.CustomProperties["mapId"];
    // }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
