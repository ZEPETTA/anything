using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LobbyManager_L : MonoBehaviour
{
    GameObject clickedObject;
    GameObject outlineImage;
    string clickedRoomName;

    // Start is called before the first frame update
    void Start()
    {
        clickedRoomName = "RoomScene_H";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClickedObject();
        }

    }

    void CheckClickedObject()
    {
        clickedObject = EventSystem.current.currentSelectedGameObject;
        
        if (clickedObject)
        {
            switch (clickedObject.tag)
            {
                case "LobbyRoom":
                    if(outlineImage == clickedObject.transform.GetChild(0).gameObject)
                    {
                        //clickedRoomName = clickedObject.GetComponentInChildren<Text>().text;
                        SceneManager.LoadScene(clickedRoomName);
                        break;
                    }
                    if (outlineImage)
                    {
                        outlineImage.SetActive(false);
                    }
                    outlineImage = clickedObject.transform.GetChild(0).gameObject;
                    outlineImage.SetActive(true);
                    break;
            }
        }

    }
}
