using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;


public class CAJ_ChatManager : MonoBehaviourPun
{
    //ChatItem 공장
    public GameObject chatItemFactory;
    
    //InputChat
    public InputField inputChat;
    
    //ScrollView 의 Content transform
    //public Transform trContent;
    public RectTransform trContent;
    
    //나의 닉네임 색깔
    Color nickColor; 


    void Start()
    {
        //InputChat 에서 엔터를 눌렀을 때 호출 되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);
        
        //커서를 안 보이게!
        Cursor.visible = false;
        
        nickColor = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f)
        );
    }
    
    void OnSubmit(string s)
    {
        //<color=#FF0000>닉네임</color>
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(nickColor) + ">" +
                          PhotonNetwork.NickName + "</color>" + " : " + s;

        
        //1. 글을 쓰다가 엔터를 치면
        photonView.RPC("RPCAddChat", RpcTarget.All, chatText);
        
        
        //4. inputChat 의 내용을 초기화
        inputChat.text = "";
        
        //5. inputChat 이 선택되도록 한다.
        inputChat.ActivateInputField();
    }
    
    [PunRPC]
    void RpcAddChat(string chat)
    {
        //2. ChatItem을 하나 만든다.
        //(부모를 ScrollView - Content)
        GameObject item = Instantiate(chatItemFactory, trContent);
         
        //3. text 컴포넌트 가져와서 inputField의 내용을 셋팅
        // Text t = item.GetComponent<Text>();
        // t.text = chat;
        CAJ_ChatItem chatItem = item.GetComponent<CAJ_ChatItem>();
        chatItem.SetText(chat);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        //esc 키를 누르면 커서를 활성화!
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            Cursor.visible = true;
        }
        
        
        
        //어딘가를 클릭하면 커서를 활성화
        if(Input.GetMouseButtonDown(0))
        {
            //만약에 커서가 UI에 없다면
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                Cursor.visible = false;
            }

            // //모바일땐
            // if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            // {
            //     
            // }
        }
    }
    
    
}
