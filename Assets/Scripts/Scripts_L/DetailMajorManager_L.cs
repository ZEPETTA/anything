using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DetailMajorManager_L : MonoBehaviourPun
{
    public GameObject player;

    public Transform playerSpawnPos;

    public Text logText;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        player = PhotonNetwork.Instantiate("$Main_Character_1_0_2D", playerSpawnPos.position, Quaternion.identity);
        player.GetComponent<CharacterMove_H>().enabled = true;
        logText.text = PhotonNetwork.CurrentLobby.Name;
        logText.text += "\n" + PhotonNetwork.CurrentRoom.Name + " , " + PhotonNetwork.CurrentRoom.PlayerCount;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
