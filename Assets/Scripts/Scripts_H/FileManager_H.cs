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

    public GameObject signUpImage;

    public InputField idInputfield;
    public Text[] OKText; //0 - id, 1- password
    public Text[] NoText;
    public Button findSameIdButton;

    public InputField passwordInputfield;
    public InputField passwordCheckInputfield;

    public InputField nameInputfield;

    public InputField phoneNumInputfield;
    public Button phoneNumCheckButton;
    public Button phoneCheckButtonImage;

    public Button closeImage;
    [SerializeField]
    bool[] singUpCheck = new bool[6] { false, false, false, false, false, false};
    public Button signUpButton;
    public Toggle signUpToggle;

    public GameObject CardUP;
    public Image SelectImage;

    private void Start()
    {
        idInputfield.onValueChanged.AddListener(OnIDValueChanged);
        passwordInputfield.onValueChanged.AddListener(OnPasswordValueChanged);
        passwordCheckInputfield.onValueChanged.AddListener(OnPasswordCheckValueChanged);
        nameInputfield.onValueChanged.AddListener(OnNameValueChanged);
        phoneNumInputfield.onValueChanged.AddListener(OnPhoneNumValueChanged);
    }

    public void XButton()
    {
        signUpImage.gameObject.SetActive(false);
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
        #region °¡ÀÔ ÆÄÆ®
        if (signUpToggle.isOn)
        {
            singUpCheck[5] = true;
        }
        else
        {
            singUpCheck[5] = false;
        }
        for (int i=0;i<singUpCheck.Length;i++)
        {
            if(singUpCheck[i] == false)
            {
                signUpButton.interactable = false;
                break;
            }
            if (i == singUpCheck.Length - 1)
            {
                signUpButton.interactable = true;
            }
        }
        #endregion
    }
    #region ¾ÆÀÌµðÆÄÆ®
    void OnIDValueChanged(string id)
    {
        findSameIdButton.interactable = id.Length > 0;
        singUpCheck[0] = false;
    }
    public void OnIDCheckButton()
    {
        if(idInputfield.text.Length > 0)
        {
            OKText[0].gameObject.SetActive(true);
            info.id = idInputfield.text;
            singUpCheck[0] = true;
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
            singUpCheck[1] = true;
        }
        else
        {
            NoText[2].gameObject.SetActive(true);
            OKText[2].gameObject.SetActive(false);
            singUpCheck[1] = false;
        }
    }
    #endregion
    #region ÀÌ¸§ ÆÄÆ®
    void OnNameValueChanged(string name)
    {
        info.name = name;
        singUpCheck[2] = true;
    }
    #endregion
    #region ÈÞ´ëÆù ¹øÈ£ ÆÄÆ®
    void OnPhoneNumValueChanged(string num)
    {
        phoneNumCheckButton.interactable = (num.Length == 11);
        if(OKText[3].gameObject.activeInHierarchy == false)
        {
            OKText[3].gameObject.SetActive(false);
            NoText[3].gameObject.SetActive(true);
        }


    }
    public void OnClickButtonImage()
    {
        phoneCheckButtonImage.gameObject.SetActive(false);
    }
    public void OnPhoneNumBTNClick()
    {
        info.phoneNumber = phoneNumInputfield.text;
        OKText[3].gameObject.SetActive(true);
        NoText[3].gameObject.SetActive(false);
        singUpCheck[3] = true;
        phoneCheckButtonImage.gameObject.SetActive(true);
    }
    #endregion
    #region ÇÐ°úÀü°ø ÀÎÁõ ÆÄÆ®
    public void OnCardButton()
    {
        CardUP.SetActive(true);
    }
    public void OpeinFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        StartCoroutine(GetTexture());
    }
    public void XCardButton()
    {
        CardUP.SetActive(false);
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
            singUpCheck[4] = true;
            SelectImage.gameObject.SetActive(true);
        }
    }
    #endregion
    #region °¡ÀÔ ÆÄÆ®
    public void OnSignButton()
    {
        signUpImage.gameObject.SetActive(false);
    }
    #endregion
    

    public void OnJoin()
    {
        string jsonData = JsonUtility.ToJson(info, true);
    }
    public void OnBack()
    {
        SceneManager.LoadScene("LoginScene_L");
    }

}