using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal2D_L : MonoBehaviour
{
    //���а� �浹�� �� �ֵ���, ���� ������Ʈ�� z���� �÷��̾�� �굵�� �����ϱ�
    public PortalInfo portalInfo;
    public float playerZ = 0f;
    GameObject definedAreaParent;
    // Start is called before the first frame update
    void Start()
    {
        portalInfo = new PortalInfo();

        //===�׽�Ʈ��===
        portalInfo.definedAreaName = "Test";
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;
        //==============

        definedAreaParent = GameObject.Find("DefinedAreaParent");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MovePlayer(GameObject collidedObj)
    {
        GameObject player = collidedObj;
        switch (portalInfo.placeType)
        {
            case PortalInfo.PlaceType.OtherMap:
                break;
            case PortalInfo.PlaceType.DefinedArea:

                GameObject definedAreaParent = GameObject.FindGameObjectWithTag("DefinedAreaParent");
                GameObject definedAreaParent2 = definedAreaParent.transform.Find(portalInfo.definedAreaName).gameObject;
                Transform[] definedAreas = definedAreaParent2.transform.GetComponentsInChildren<Transform>();
                Transform randomDefinedArea = definedAreas[Random.Range(0, definedAreas.Length)];
                //��� DefinedArea�� ��� DefinedAreaParent�� �ְ�,
                //�� �̸����� �з��� DefinedArea�� ��� DefinedAreaParent2�� �־�� ��
                //���� ���, �̸��� "Test"�� DefinedArea(��������) < DefinedAreaParent2 < DefinedAreaParent
                //�����ʿ�

                if (portalInfo.moveType == PortalInfo.MoveType.Instant)
                {
                    player.transform.position = new Vector3(randomDefinedArea.position.x, randomDefinedArea.position.y, playerZ);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        player.transform.position = new Vector3(randomDefinedArea.position.x, randomDefinedArea.position.y, playerZ);
                    }
                }
                break;
            case PortalInfo.PlaceType.OtherSpace:
                break;
        }
    }

    /*   private void OnTriggerEnter(Collider other)
       {
           if (other.transform.tag == "Player")
           {
               print("Trigger Entered");
               MovePlayer(other.transform.gameObject);
           }
       }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            print("Player Collided");
            MovePlayer(collision.transform.gameObject);
        }
    }
}
