using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CAJ_ConnectionManager : MonoBehaviourPunCallbacks
{
    //닉네임
    //닉네임 InputField
    public InputField inputNickName;
    //접속 Button
    public Button btnConnect;
    
    
    public void OnClickConnect()
    {
        //1. 서버 접속 요청 (App Id, 지역, 서버에 요청)
        PhotonNetwork.ConnectUsingSettings();
    }
    
    void Start()
    {
        //닉네임이 변할 때마다 호출하게 하는 함수 등록 (나중에 필요없음!)
        inputNickName.onValueChanged.AddListener(OnValueChanged);
        //inputNickName에서 Enter키 누르면 호출되는 함수 등록
        inputNickName.onSubmit.AddListener(OnSubmit);
        //inputNickName에서 Focusing이 사라졌을 때 호출되는 함수 등록
        //inputNickName.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnSubmit(string s)
    {
        if (s.Length > 0)
            OnClickConnect();
        
        print("닉네임 엔터 : " + s);
    }

    private void OnValueChanged(string s)
    {
        //만약 s의 길이가 0보다 크면 버튼 동작, 그렇지 않으면 버튼 동작 x
        btnConnect.interactable = s.Length > 0;
        print("닉네임 입력 : " + s);
    }

    //2-1. 마스터 서버 접속성공시 호출(Lobby에 진입할 수 없는 상태)
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    //2-2. 마스터 서버 접속성공시 호출(Lobby에 진입할 수 있는 상태)
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        
        //내 닉네임 설정
        PhotonNetwork.NickName = inputNickName.text;

        //로비 진입 요청
        PhotonNetwork.JoinLobby();
    }

    //3. 로비 진입 성공시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        
        //내 닉네임 설정
        //PhotonNetwork.NickName = "MetaMong_" + Random.Range(1, 100);

        //LobbyScene으로 이동 ( SceneManager.LoadScene("CAJ_LobbyScene"); )
        //Scene이 로드 되는 순간 네트워크 유실 되지 않기 위해서 -> PhotonNetwork.LoadLevel 사용
        PhotonNetwork.LoadLevel("CAJ_OutScene");
    }


    void Update()
    {

    }
}