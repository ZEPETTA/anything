using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedObject_L : MonoBehaviour
{
    public enum FixedObjectType
    {
        NewsStand,
        GuestBook
    }

    public GameObject panelNewsStand;
    public GameObject panelGuestBook;

    public FixedObjectType fixedObjectType;

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
            case FixedObjectType.GuestBook:
                panelGuestBook.SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            isPlayerOn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            isPlayerOn = false;
    }
}
