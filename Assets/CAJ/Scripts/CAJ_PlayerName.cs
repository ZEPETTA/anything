using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CAJ_PlayerName : MonoBehaviourPun
{
    public Text nickName;
    
    
    // Start is called before the first frame update
    void Start()
    {
        nickName.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
