using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommentManager_L : MonoBehaviour
{
    public InputField inputComment;

    public GameObject commentItemPrefab;

    public RectTransform rtContent;

    public List<InputField> inputCommentList;
    public List<RectTransform> rtContentList;
    public List<GameObject> articleList;
    public List<RectTransform> rtScrollViewList;
    Color idColor;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < inputCommentList.Count; i++)
        {
            inputCommentList[i].onSubmit.AddListener(OnSubmit);
        }

        //inputComment.onSubmit.AddListener(OnSubmit);

        idColor = new Color32((byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            (byte)Random.Range(0, 256),
            255);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < articleList.Count; i++)
            {
                if (articleList[i].activeSelf)
                {
                    rtContent = rtContentList[i];
                    inputComment = inputCommentList[i];
                    rtScrollView = rtScrollViewList[i];
                }
            }
        }
    }

    void OnSubmit(string s)
    {
        //string commentText = "<color=#" + ColorUtility.ToHtmlStringRGB(idColor) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;
        string commentText = "<color=#" + ColorUtility.ToHtmlStringRGB(idColor) + ">" + "Jinho" + "</color>" + " : " + s;

        //photonView.RPC("RpcAddChat", RpcTarget.All, chatText);
        AddChat(commentText);
        //4. InputChat의 내용을 초기화
        inputComment.text = "";
        //5. InputChat에 Focusing 을 해주자.
        inputComment.ActivateInputField();
    }

    //이전 Content의 H
    float prevContentH;
    //ScorllView의 RectTransform
    public RectTransform rtScrollView;

    void AddChat(string commentText)
    {
        //0. 바뀌기 전의 Content H값을 넣자

        prevContentH = rtContent.sizeDelta.y;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        GameObject item = Instantiate(commentItemPrefab, rtContent);
        //2. 만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        item.GetComponent<ChangeTextSize_L>().text = commentText;
        //CommentItem_L comment = item.GetComponent<CommentItem_L>();
        //3. 가져온 컴포넌트에 s를 셋팅
        
        //comment.SetText(commentText);


        StartCoroutine(AutoScrollBottom());
    }

    IEnumerator AutoScrollBottom()
    {
        yield return null;

        //trScrollView H 보다 Content H 값이 커지면(스크롤 가능상태)
        if (rtContent.sizeDelta.y > rtScrollView.sizeDelta.y)
        {
            //4. Content가 바닥에 닿아있었다면
            if (rtContent.anchoredPosition.y >= prevContentH - rtScrollView.sizeDelta.y)
            {
                //5. Content의 y값을 다시 설정해주자
                rtContent.anchoredPosition = new Vector2(0, rtContent.sizeDelta.y - rtScrollView.sizeDelta.y);
            }
        }
    }

}
