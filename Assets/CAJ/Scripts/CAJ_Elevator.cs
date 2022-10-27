using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CAJ_Elevator : MonoBehaviourPunCallbacks
{
    //방 이름, 총 인원 -> 방 참가 되도록
    //방이름 InputField
    public InputField inputRoomName;
    //총인원 InputField
    public InputField inputMaxPlayer;
    //방참가 Button
    public Button btnJoin;
    //방생성 Button
    public Button btnCreate;
    
    //방 목록
    public Transform roomListContent;
    //모든 룸 roomInfo -> 딕셔너리(키, 밸류값)
    //키값 : 방 이름(string) , 밸류값 : Roominfo
    private Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //방 버튼 프리팹
    public GameObject roomItemFactory;

    //맵 Thumbnaile(썸네일) 들
    public GameObject[] mapThums;

    void Start()
    {
        //CreateRoom();
        //방 이름이 변할 때마다 호출하게 하는 함수 등록
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        //총 인원이 변할 때마다 호출하게 하는 함수 등록
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
        
    }
    
    private void OnRoomNameValueChanged(string room)
    {
        //생성 버튼 활성화
        btnCreate.interactable = room.Length > 0;
        print("방 이름 입력 : " + room);
        //참여 버튼 활성화
        btnJoin.interactable = room.Length > 0 && inputMaxPlayer.text.Length > 0;
    }


    private void OnMaxPlayerValueChanged(string s)
    {
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0;
    }
    

    //방 생성
    public void CreateRoom()
    {
        //방 옵션을 설정
        RoomOptions roomOptions = new RoomOptions();
        
        //최대 인원 설정
        //roomOptions.MaxPlayers = 20;
        roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        
        //룸 리스트에 보이게 ? (기본 = true, 공개방)
        roomOptions.IsVisible = true;
        
        //Custom 룸 정보를 설정 (해쉬테이블 사용 -> string Key, string Value, int Value 해쉬테이블이라 가능한 것)
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["description"] = "삼성";
        
        //mapId = 방장, 맵 선택에 따라 맵이 바뀌는 것을 숫자화한 것
        //hash["mapId"] = 5;
        hash["mapId"] = Random.Range(0, mapThums.Length);
        hash["room_name"] = inputRoomName.text;
        
        //위의 정보를 포함해서 정보를 만든다. 
        //CustomRoomProperties -> RoomInfo에 hash값 저장됨
        roomOptions.CustomRoomProperties = hash;
        
        //Custom 정보 키값을 보이게 하겠다.
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "description", "mapId", "room_name" };
        
        //방 생성 요청 (해당 옵션을 이용해서)
        PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions);
    }

    //2-1-1. 방이 생성되면 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    //2-1-2. 방생성이 실패될 때 호출 되는 함수(returnCode = 포톤에서 정의 해놓은 숫자)
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + "," + message);
        
        //JoinRoom();
    }
    
    //2-2-1. 방 참가 요청
    public void JoinRoom()
    {
        //PhotonNetwork.JoinRoom("MetaMong");
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }
    
        
    //2-2-2. 방 참가가 완료 되었을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("CAJ_RoomScene");
    }
        
    //2-2-3. 방 참가가 실패 되었을 때 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }
    
    //방 목록이 변경되었을 때(생성, 삭제, 정보 갱신) 호출해주는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        
        //룸 리스트 UI 삭제
        RemoveRoomList();
        //룸 리스트 UI 정보 갱신
        UpdateRoomCache(roomList);
        //룸 리스트 UI 생성
        CreateRoomListUI();

    }

    private void RemoveRoomList()
    {
        foreach (Transform tr in roomListContent)
        {
            Destroy(tr.gameObject);
        }
    }

    private void UpdateRoomCache(List<RoomInfo> roomList)
    {
        //룸 리스트의 정보에서 가져옴
        foreach (RoomInfo info in roomList)
        {
            //만약에 roomCache에 info.Name에 해당되는 키값이 있다면
            if (roomCache.ContainsKey(info.Name))
            {
                //만약에 해당 룸이 삭제되는 것이라면
                if (info.RemovedFromList)
                {
                    //roomCache에서 삭제
                    roomCache.Remove(info.Name);
                }
                //그렇지 않다면
                else
                {
                    //정보 갱신
                    roomCache[info.Name] = info;
                }
            }
            //그렇지 않다면
            else
            {
                //roomCache에 추가
                roomCache[info.Name] = info;
            }
        }
    }

    private void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            //방 정보 아이템 만들고
            GameObject go = Instantiate(roomItemFactory, roomListContent);
            
            //아이템 정보 세팅
            CAJ_RoomItem roomItem = go.GetComponent<CAJ_RoomItem>();
            //RoomItem roomItem = go.GetComponent<RoomItem>();
            //roomItem.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            roomItem.SetInfo(info);
            
            //OnClickAction에 SelectRoom을 넣는다.
            roomItem.onClickAction = SelectRoom;
            
            //키 밸류가 object형이기 때문에, string을 붙인다
            string des = (string)info.CustomProperties["description"];
            int mapId = (int)info.CustomProperties["mapId"];
            print(des);
        }
    }

    private int prevMapId = -1;
    void SelectRoom(string room, int mapId)
    {
        //방 이름 세팅
        inputRoomName.text = room;
        
        //만약에 이전에 선택한 맵이 있다면
        if (prevMapId > -1)
        {
            //해당 맵을 비활성화하면 됨
            mapThums[prevMapId].SetActive(false);
        }
        
        //맵 thumbnail 세팅
        mapThums[mapId].SetActive(true);
        //prevMapId <- mapId 세팅
        prevMapId = mapId;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
