using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAJ_ChatItem : MonoBehaviour
{
    //Text
    private Text chatText;
    
    //RectTransform
    private RectTransform rt;
    
    // Start is called before the first frame update
    void Awake()
    {
        chatText = GetComponent<Text>();
        rt = GetComponent<RectTransform>();
    }

    public void SetText(string chat)
    {
        //텍스트 셋팅
        chatText.text = chat;
        
        //height 맞춰주자
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
