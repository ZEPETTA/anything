using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager_L : MonoBehaviourPunCallbacks
{
    List<GameObject> players;

    int numPlayers;

    int currPlayerNum = 1;
    int newPlayerNum;

    // Start is called before the first frame update
    void Start()
    {
        newPlayerNum = currPlayerNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            newPlayerNum = PhotonNetwork.CurrentRoom.PlayerCount;
            if (newPlayerNum > currPlayerNum)
            {
                photonView.RPC("RpcSetPlayerIdx", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RpcSetPlayerIdx()
    {
        GameObject[] playersArr = GameObject.FindGameObjectsWithTag("Player");
        System.Array.Sort<GameObject>(playersArr, (x, y) => string.Compare(x.GetPhotonView().ViewID.ToString(), y.GetPhotonView().ViewID.ToString()));

        players = playersArr.ToList();


        numPlayers = players.Count;
       

        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < numPlayers; i++)
        {
            players[i].GetComponent<CharacterMove_H>().idx = i;
        }

    }


    

/*    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RpcSetPlayerIdx", RpcTarget.All);
        }
    }*/
}
