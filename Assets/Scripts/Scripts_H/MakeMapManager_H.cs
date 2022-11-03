using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MakeMapManager_H : MonoBehaviour
{
    public ToggleController controller;
    public InputField roomPasswordInputField;
    public InputField roomNameInputField;
    public InputField roomIntroductionInputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isOn)
        {
            roomPasswordInputField.gameObject.SetActive(true);
        }
        else
        {
            roomPasswordInputField.gameObject.SetActive(false);
        }
    }
    public void MakeMap()
    {
        SpaceInfo info = new SpaceInfo();
        SpaceInfo.spaceName = roomNameInputField.text;
        info.spaceIntroduction = roomIntroductionInputField.text;
        info.passWord = roomPasswordInputField.text;
        string jsonMap = JsonUtility.ToJson(info, true);
        File.WriteAllText(Application.dataPath + "/Resources/Resources_H/MapData" + "/" + SpaceInfo.spaceName + ".txt", jsonMap);
        SceneManager.LoadScene("RoomScene_H");
    }

    public void XButton()
    {
        gameObject.SetActive(false);
    }
    public  void TurnOnMakeMap()
    {
        gameObject.SetActive(true);
    }
}
