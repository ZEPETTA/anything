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
        //4. InputChat�� ������ �ʱ�ȭ
        inputComment.text = "";
        //5. InputChat�� Focusing �� ������.
        inputComment.ActivateInputField();
    }

    //���� Content�� H
    float prevContentH;
    //ScorllView�� RectTransform
    public RectTransform rtScrollView;

    void AddChat(string commentText)
    {
        //0. �ٲ�� ���� Content H���� ����

        prevContentH = rtContent.sizeDelta.y;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        GameObject item = Instantiate(commentItemPrefab, rtContent);
        //2. ���� ChatItem���� ChatItem ������Ʈ �����´�
        item.GetComponent<ChangeTextSize_L>().text = commentText;
        //CommentItem_L comment = item.GetComponent<CommentItem_L>();
        //3. ������ ������Ʈ�� s�� ����
        
        //comment.SetText(commentText);


        StartCoroutine(AutoScrollBottom());
    }

    IEnumerator AutoScrollBottom()
    {
        yield return null;

        //trScrollView H ���� Content H ���� Ŀ����(��ũ�� ���ɻ���)
        if (rtContent.sizeDelta.y > rtScrollView.sizeDelta.y)
        {
            //4. Content�� �ٴڿ� ����־��ٸ�
            if (rtContent.anchoredPosition.y >= prevContentH - rtScrollView.sizeDelta.y)
            {
                //5. Content�� y���� �ٽ� ����������
                rtContent.anchoredPosition = new Vector2(0, rtContent.sizeDelta.y - rtScrollView.sizeDelta.y);
            }
        }
    }

}
