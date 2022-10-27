using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CAJ_RoomItem : MonoBehaviour
{
    //방 정보 Text
    public Text roomInfo;
    
    //설명 Text
    public Text roomDescription;
    
    //맵 id
    int mapId;
    
    //클릭 되었을 때 호출돼야 하는 함수를 담을 변수
    public System.Action<string, int> onClickAction;

    
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
        name = roomName;
        //name = PhotonNetwork.CurrentRoom.Name;
        
        //방 정보 세팅 -> 방이름 (1/10)
        roomInfo.text = roomName + "(" + currPlayer + "/" + maxPlayer + ")";
        //roomInfo.text = PhotonNetwork.CurrentRoom.Name + "(" + currPlayer + "/" + maxPlayer + ")";
        //roomInfo.text = PhotonNetwork.CurrentRoom.Name + "(" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + ")";
    }
    
    

    public void SetInfo(RoomInfo info)
    {
        //SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);
        
        //방 설명 세팅
        roomDescription.text = (string)info.CustomProperties["description"];
        
        //맵 id셋팅
        mapId = (int)info.CustomProperties["mapId"];
    }

    public void OnClick()
    {
        //만약에 onClickAction 가 null이 아니라면
        if(onClickAction != null)
        {
            //onClickAction 실행
            onClickAction(name, mapId);
        }

        
        // //만약에 onClickAction 이 null이 아니라면
        // if (OnClickAction != null)
        // {
        //     //onClickAction에 들어있는 함수를 호출
        //     OnClickAction(name);
        // }
        
        // //inputRoomname에 roomName을 전달해서 세팅
        // //1. InputRoomName 게임 오브젝트 찾자
        // GameObject inputRoomName = GameObject.Find("InputRoomName");
        // //2. 찾은 게임 오브젝트에서 InputField 컴포넌트 가져오자
        // InputField input = inputRoomName.GetComponent<InputField>();
        // //3. 가져온 컴포넌트에서 text 를 roomName으로 하자
        // input.text = name;
    }
}
