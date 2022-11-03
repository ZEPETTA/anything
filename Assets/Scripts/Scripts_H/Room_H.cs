using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room_H : MonoBehaviour
{
    public string roomName;
    public Text roomNameText;
    public GameObject mouseImage;
    public Text roomInfoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roomName.Contains(".txt"))
        {
            roomName = roomName.Replace(".txt", "");
        }
        roomNameText.text = roomName;
    }
    public void MouseOn()
    {
        mouseImage.SetActive(true);
    }
    public void MouseOff()
    {
        mouseImage.SetActive(false);
    }
}
