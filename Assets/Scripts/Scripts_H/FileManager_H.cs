using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;






public class FileManager_H : MonoBehaviour
{
    public UserInfo info = new UserInfo();
    string path;
    public RawImage rawImage;

    public Image signUpImage;

    public InputField idInputfield;
    public Text[] OKText; //0 - id, 1- password
    public Text[] NoText;
    public Button findSameIdButton;

    public InputField passwordInputfield;
    public InputField passwordCheckInputfield;

    public InputField nameInputfield;

    public InputField phoneNumInputfield;
    public Button phoneNumCheckButton;

    public Image findSameIdImage;
    public Button closeImage;

    private void Start()
    {
        idInputfield.onValueChanged.AddListener(OnIDValueChanged);
        passwordInputfield.onValueChanged.AddListener(OnPasswordValueChanged);
        passwordCheckInputfield.onValueChanged.AddListener(OnPasswordCheckValueChanged);
        nameInputfield.onValueChanged.AddListener(OnNameValueChanged);
        phoneNumInputfield.onValueChanged.AddListener(OnPhoneNumValueChanged);
    }

    
    private void Update()
    {
        #region ¾ÆÀÌµð ÆÄÆ®
        if (idInputfield.text.Length < 1)
        {
            NoText[0].gameObject.SetActive(true);
            OKText[0].gameObject.SetActive(false);
        }
        else
        {
            NoText[0].gameObject.SetActive(false);
        }
        #endregion
    }
    #region ¾ÆÀÌµðÆÄÆ®
    void OnIDValueChanged(string id)
    {
        findSameIdButton.interactable = id.Length > 0;
        info.id = id;
    }
    public void OnIDCheckButton()
    {
        if(idInputfield.text.Length > 0)
        {
            OKText[0].gameObject.SetActive(true);
        }
    }
    #endregion
    #region ºñ¹Ð¹øÈ£ ÆÄÆ®
    string passwordChecker;
    void OnPasswordValueChanged(string password)
    {
        info.password = password;
        string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
        Regex regex = new Regex(str);
        //passwordChecker = Regex.Replace(info.password,@"[ ^0-9a-zA-Z°¡-ÆR ]{1,10}", "", RegexOptions.Singleline);
        Debug.Log(regex.IsMatch(password));
        if(regex.IsMatch(password))
        {
            NoText[1].gameObject.SetActive(true);
        }
        else
        {
            NoText[1].gameObject.SetActive(false);
        }
    }
    void OnPasswordCheckValueChanged(string password)
    {
        if(password == info.password)
        {
            NoText[2].gameObject.SetActive(false);
            OKText[2].gameObject.SetActive(true);
        }
        else
        {
            NoText[2].gameObject.SetActive(true);
            OKText[2].gameObject.SetActive(false);
        }
    }
    #endregion
    #region ÀÌ¸§ ÆÄÆ®
    void OnNameValueChanged(string name)
    {
        info.name = name;
    }
    #endregion
    #region ÈÞ´ëÆù ¹øÈ£ ÆÄÆ®
    void OnPhoneNumValueChanged(string num)
    {
        phoneNumCheckButton.interactable = (num.Length == 11);

    }
    #endregion
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