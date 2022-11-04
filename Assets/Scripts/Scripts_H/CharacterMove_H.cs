using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterMove_H : MonoBehaviourPun
{
    public bool myCharacter = true;
    public float speed;
    public float runSpeed;
    float applyRunSpeed;
    bool applyRunFlag = false;
    private Vector3 vector;
    public int walkCount;
    int currentWalkCount;
    bool canMove = true;
    private Animator animator;
    public Sprite[] imoticon;
    public GameObject imoticonPrefab;
    public GameObject pressFKey;
    private KeyCode[] keyCodes = {
KeyCode.Alpha1,
KeyCode.Alpha2,
KeyCode.Alpha3,
KeyCode.Alpha4,
KeyCode.Alpha5,
KeyCode.Alpha6,
KeyCode.Alpha7,
KeyCode.Alpha8,
KeyCode.Alpha9,
};
    public bool inObj = false;
    public ObjectInfo_H objInfo;
    public Userinfo user;
    public int idx;

    void Start()
    {
        if (photonView.IsMine)
        {
            user = new Userinfo();

            user.name = PhotonNetwork.NickName;
        }
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StopAllCoroutines();
                StartCoroutine(MoveCoroutine());
            }
        }
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                photonView.RPC("RPCEmoji", RpcTarget.All, i, idx);

/*                GameObject imo = gameObject.transform.GetChild(0).gameObject;
                EmoDestory_H emo = imo.GetComponent<EmoDestory_H>();
                emo.emoOn = true;
                emo.checkTime = 0;
                SpriteRenderer spriteRenderer = imo.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = imoticon[i];
                imo.transform.parent = gameObject.transform;*/
            }

            //키를 눌렀을 때
            //나의 아이디를 찾은 후, <==여기까진 로컬
            //아이디를 통해 해당 키의 이모티콘을 켜주는 것 <==이건 RPC로 해야함

            //  photonView.RPC("RPCEmoji", RpcTarget.All, i);
        }
        if(inObj == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                objInfo.OnPlayerCall();
            }
        }
    }

    [PunRPC]
    void RPCEmoji(int num, int myIdx)
    {
//        if (GetComponent<CharacterMove_H>().enabled)
//        {
            GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < playerArray.Length; i++)
            {
                if (playerArray[i].GetComponent<CharacterMove_H>().idx == myIdx)
                {
                    GameObject imo = playerArray[i].transform.GetChild(0).gameObject;
                    EmoDestory_H emo = imo.GetComponent<EmoDestory_H>();
                    emo.emoOn = true;
                    emo.checkTime = 0;
                    imo.GetComponent<SpriteRenderer>().sprite = imoticon[num];
                    imo.transform.parent = playerArray[i].transform;
                }
            }
            
//        }
    }

    IEnumerator MoveCoroutine()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            applyRunSpeed = runSpeed;
            applyRunFlag = true;
        }
        else
        {
            applyRunSpeed = 0;
            applyRunFlag = false;
        }
        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        int count = 0;
        animator.SetFloat("DirX", vector.x);
        animator.SetFloat("DirY", vector.y);
        animator.SetBool("Walking", true);
        while (currentWalkCount < walkCount)
        {
            count++;
            transform.position += vector.normalized * (speed + applyRunSpeed) * 0.1f;
            if (applyRunFlag)
            {
                currentWalkCount++;
            }
            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }
        animator.SetBool("Walking", false);
        currentWalkCount = 0;
        canMove = true;
    }
}
