using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
        if (CAJ_PlayerManager.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
        //
        //
        // if (playerPrefab == null)
        // {
        //     Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        // }
        // else
        // {
        //     Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
        //     // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //     PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
