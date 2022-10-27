using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CAJ_CharacterManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void Start()
    {
        //플레이어 생성
        //PhotonNetwork.Instantiate("CAJ_Player", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate("CAJ_Player", new Vector3(0, 1, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
