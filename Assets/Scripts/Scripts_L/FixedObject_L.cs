using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedObject_L : MonoBehaviour
{
    public enum FixedObjectType
    {
        NewsStand,
        NoticeBoard
    }

    public GameObject panelNewsStand;
    public GameObject panelNoticeBoard;

    public FixedObjectType fixedObjectType;
    GameObject pressF;
    bool isPlayerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOn)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FixedObjectEvent();
                pressF.SetActive(false);
            }
        }
    }

    void FixedObjectEvent()
    {
        switch (fixedObjectType)
        {
            case FixedObjectType.NewsStand:
                panelNewsStand.SetActive(true);
                break;
            case FixedObjectType.NoticeBoard:
                panelNoticeBoard.SetActive(true);
                panelNoticeBoard.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0)).x, Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0)).y+1f,-5f);
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(true);
            isPlayerOn = true;
            pressF = character.pressFKey;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(false);
            isPlayerOn = false;
            pressF = null;
        }

    }
}
