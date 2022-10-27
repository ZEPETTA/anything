using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class New_Door : MonoBehaviour
{
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene("InsideScene");
            //PhotonNetwork.LoadLevel("CAJ_LobbyScene");
            //PhotonNetwork.LoadLevel("CAJ_InsideScene");
            //PhotonNetwork.LoadLevel("CAJ_LobbyScene");
            PhotonNetwork.LoadLevel("New_ChannelScene");
        }
    }
}
