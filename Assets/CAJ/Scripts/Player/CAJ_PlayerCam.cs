using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CAJ_PlayerCam : MonoBehaviourPunCallbacks
{
    //CamPos의 Transform
    public Transform camPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //만약에 내 것이라면
        if (photonView.IsMine == true)
        {
            //CamPos 를 활성화 시키자
            camPos.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false) return;
        if (Cursor.visible == true) return;
    }
}
