using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DetailMajorManager_L : MonoBehaviourPun
{
    public GameObject player;

    public Transform playerSpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        player = PhotonNetwork.Instantiate("$Main_Character_1_0_2D", playerSpawnPos.position, Quaternion.identity);
        player.GetComponent<CharacterMove_H>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
