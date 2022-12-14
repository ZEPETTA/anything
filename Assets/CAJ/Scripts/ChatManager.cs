using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviourPun
{
    public string userNickname_tmp;

    //ChatItem 공장
    public GameObject chatItemFactory;
    
    //InputChat
    public InputField inputChat;
    
    //Scrollview 의 Content transform
    public Transform trContent;

    private Color nickColor;

    void Start()
    {
        //InputChat에서 엔터를 눌렀을 때 호출 되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);

        Cursor.visible = false;

        nickColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }
    
    void OnSubmit(string s)
    {
        //string chatText = PhotonNetwork.NickName + " : " + s;
        //string chatText = userNickname_tmp + " : " + s;
        string chatText = "<color=#" + ColorUtility.ToHtmlStringRGB(nickColor) + ">" + PhotonNetwork.NickName + "</color>" + " " + "<color=green>"+s+"</color>";
        photonView.RPC("RpcAddChat", RpcTarget.All, chatText);

        //GameObject item = Instantiate(chatItemFactory, trContent);
        //item.GetComponent<Text>().text = chatText;

        //Debug.Log(s);
        //photonView.RPC("RpcAddChat", RpcTarget.All, s);

        inputChat.text = "";
        
        inputChat.ActivateInputField();
    }

    [PunRPC]
    void RpcAddChat(string chat)
    {
        GameObject item = Instantiate(chatItemFactory, trContent);

        Text t = item.GetComponent<Text>();
        //t.text = inputChat.text;
        t.text = chat;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
            Cursor.visible = false;
                
            }
        }
    }
}