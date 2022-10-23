using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PortalInfo
{
    public enum MoveType
    {
        Key,
        Instant
    }

    public enum PlaceType
    {
        OtherMap,
        DefinedArea,
        OtherSpace
    }
    //Key = FŰ �̵�
    //Instant = ��� �̵�

    public MoveType moveType;
    public PlaceType placeType;

    public string definedAreaName;
    public string mapName;
    public Vector3 position;
    //�̸����� mapInfo���� �ش� �̸��� definedArea�� �迭(�Ǵ� ����Ʈ)�� ã�� �� �־�� ��
    //ã�� area�� Portal_L���� �޾Ƽ�, �� �� �ϳ��� �̵��� �� �־�� ��
}

public class Portal2D_L : MonoBehaviour
{
    //���а� �浹�� �� �ֵ���, ���� ������Ʈ�� z���� �÷��̾�� �굵�� �����ϱ�
    public PortalInfo portalInfo;
    public float playerZ = 0f;
    GameObject definedAreaParent;
    bool onPlayer = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //portalInfo = new PortalInfo();

        //===�׽�Ʈ��===
/*        portalInfo.definedAreaName = "Test";
        portalInfo.moveType = PortalInfo.MoveType.Instant;
        portalInfo.placeType = PortalInfo.PlaceType.DefinedArea;*/
        //==============

        definedAreaParent = GameObject.Find("DefinedAreaParent");
    }

    // Update is called once per frame
    void Update()
    {
        if (onPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                MovePlayer(player);
                onPlayer = false;
            }
        }
    }

    void MovePlayer(GameObject collidedObj)
    {
        GameObject player = collidedObj;
        switch (portalInfo.placeType)
        {
            case PortalInfo.PlaceType.OtherMap:
                MapInfo.mapName = portalInfo.mapName;
                if (portalInfo.moveType == PortalInfo.MoveType.Instant)
                {
                    SceneManager.LoadScene("RoomScene_H");
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        SceneManager.LoadScene("RoomScene_H");
                    }
                }
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
                player.transform.position = new Vector3(randomDefinedArea.position.x, randomDefinedArea.position.y, playerZ);

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
            if (portalInfo.moveType == PortalInfo.MoveType.Instant)
            {
                MovePlayer(collision.gameObject);
            }
            else
            {
                player = collision.gameObject;
                onPlayer = true;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            player = null;
            onPlayer = false;
        }
    }
}
