using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class New_FirstDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            PhotonNetwork.LoadLevel("3New_InsideScene");
        }
    }
}
