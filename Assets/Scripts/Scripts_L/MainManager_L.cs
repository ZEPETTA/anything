using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MainManager_L : MonoBehaviour
{
    GameObject clickedObject;
    GameObject outlineImage;
    string clickedRoomName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickMajor()
    {
        clickedObject = EventSystem.current.currentSelectedGameObject;

        if (clickedObject)
        {
            switch (clickedObject.tag)
            {
                case "MainRoom":
                    if (outlineImage == clickedObject.transform.GetChild(0).gameObject)
                    {
                        //clickedRoomName = clickedObject.GetComponentInChildren<Text>().text;
                        //MapInfo.mapName = clickedObject.GetComponent<Room_H>().roomName;
                        //SceneManager.LoadScene("RoomScene_H");
                        SceneManager.LoadScene(clickedObject.GetComponent<MajorName_L>().majorSceneName);
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
