using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;







public class FileManager_H : MonoBehaviour
{
    public UserInfo info = new UserInfo();
    string path;
    public RawImage rawImage;
    public InputField idInputfield;
    public InputField passwordInputfield;
    public Button findSameIdButton;
    public Image findSameIdImage;
    public Button closeImage;

    private void Start()
    {
        idInputfield.onValueChanged.AddListener(OnIDValueChanged);
        passwordInputfield.onValueChanged.AddListener(OnPasswordValueChanged);
    }

    void OnIDValueChanged(string id)
    {
        findSameIdButton.interactable = id.Length > 0;
        info.id = id;
    }

    void OnPasswordValueChanged(string password)
    {
        info.password = password;
    }

    public void OpeinFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        StartCoroutine(GetTexture());
    }



    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = myTexture;
        }
    }

    public void OnJoin()
    {
        string jsonData = JsonUtility.ToJson(info, true);
    }
    public void OnBack()
    {
        SceneManager.LoadScene("LoginScene_L");
    }
    public void Same()
    {
        findSameIdImage.gameObject.SetActive(true);
        closeImage.Select();
    }
    public void CloseImage()
    {
        findSameIdImage.gameObject.SetActive(false);
        findSameIdButton.Select();
    }
}