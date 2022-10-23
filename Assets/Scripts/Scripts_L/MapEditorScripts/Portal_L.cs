using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]


public class Portal_L : MonoBehaviour
{
    //포털과 충돌할 수 있도록, 포털 오브젝트의 z값이 플레이어와 닿도록 설정하기
    public PortalInfo portalInfo;
    public float playerZ = 0f;
    GameObject definedAreaParent;
    bool onPlayer = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        portalInfo = new PortalInfo();

        //===테스트용===
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
        GameObject player = collidedObj.transform.parent.gameObject;
        switch (portalInfo.placeType)
        {
            case PortalInfo.PlaceType.OtherMap:
                break;
            case PortalInfo.PlaceType.DefinedArea:

                GameObject definedAreaParent = GameObject.FindGameObjectWithTag("DefinedAreaParent");
                GameObject definedAreaParent2 = definedAreaParent.transform.Find(portalInfo.definedAreaName).gameObject;
                Transform[] definedAreas = definedAreaParent2.transform.GetComponentsInChildren<Transform>();
                Transform randomDefinedArea = definedAreas[Random.Range(0, definedAreas.Length)];
                //모든 DefinedArea를 담는 DefinedAreaParent가 있고,
                //각 이름으로 분류된 DefinedArea를 담는 DefinedAreaParent2가 있어야 함
                //예를 들어, 이름이 "Test"인 DefinedArea(지정영역) < DefinedAreaParent2 < DefinedAreaParent
                //수정필요

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
        if(onPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                MovePlayer(player);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            onPlayer = true;
            player = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            onPlayer = false;
            player = null;
        }
    }
}
