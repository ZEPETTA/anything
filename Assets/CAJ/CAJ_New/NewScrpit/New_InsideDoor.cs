using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class New_InsideDoor : MonoBehaviour
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

    // �÷��̾� �±� ������Ʈ�� �浹�ϸ� �� ��ȯ�� �Ͼ��
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene("InsideScene");
            //PhotonNetwork.LoadLevel("CAJ_LobbyScene");
            //PhotonNetwork.LoadLevel("CAJ_InsideScene");
            //PhotonNetwork.LoadLevel("CAJ_LobbyScene");
            PhotonNetwork.LoadLevel("4New_ChannelScene");
        }
    }
}
