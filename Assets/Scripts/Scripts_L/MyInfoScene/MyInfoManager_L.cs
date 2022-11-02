using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class MyInfoManager_L : MonoBehaviour
{
    string path = "";

    public RawImage rawImage;

    public Transform majorWorkContent;
    public GameObject majorWorkPrefab;

    public GameObject panelMajorWorkEdit;

    public GameObject currClickedMajorWorkItem;
    public RawImage majorWorkDetail;


    [Header("Edit할 InputField, Text 오브젝트")]
    public GameObject inputNickname;
    public GameObject textPW;
    public GameObject inputDetailMajor;
    public GameObject inputKeyword;
    public GameObject inputSpecialty;
    public GameObject inputVision;
    public GameObject inputUrl;

    [Header("Edit 반영할 Text 오브젝트")]
    public GameObject textNickname;
    public GameObject textDetailMajor;
    public GameObject textKeyword;
    public GameObject textSpecialty;
    public GameObject textVision;
    public GameObject textUrl;

    [Header("Edit 관련 Button 오브젝트")]
    public GameObject btnCheckNickname;
    public GameObject btnChangePhoneNum;
    public GameObject btnWithdraw;
    public GameObject btnChangeDepart; 
    public GameObject btnSave;
    public GameObject btnEdit;

    public List<InputField> inputFieldList;
    public List<Text> textList;

    // Start is called before the first frame update
    void Start()
    {

        inputFieldList.Add(inputNickname.GetComponent<InputField>());
        inputFieldList.Add(inputDetailMajor.GetComponent<InputField>());
        inputFieldList.Add(inputKeyword.GetComponent<InputField>());
        inputFieldList.Add(inputSpecialty.GetComponent<InputField>());
        inputFieldList.Add(inputVision.GetComponent<InputField>());
        inputFieldList.Add(inputUrl.GetComponentInChildren<InputField>());

        textList.Add(textNickname.GetComponent<Text>());
        textList.Add(textDetailMajor.GetComponent<Text>());
        textList.Add(textKeyword.GetComponent<Text>());
        textList.Add(textSpecialty.GetComponent<Text>());
        textList.Add(textVision.GetComponent<Text>());
        textList.Add(textUrl.GetComponent<Text>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenFileExplorer()
    {

            path = WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
            //path = EditorUtility.OpenFilePanel("Show all images(.jpg)", "", "jpg");
            StartCoroutine(GetTexture());
        
    }
    public GameObject uploadImage;
    public RawImage raw;
    public void OpenUploadImage()
    {
        uploadImage.SetActive(true);
    }
    public void CloseUploadImage()
    {
        uploadImage.SetActive(false);
        AddWork((Texture2D)raw.texture);
        raw.texture = null;
    }

    IEnumerator GetTexture()
    {
        //xCardButton.interactable = false;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D convertedTexture = (Texture2D)myTexture;
            //rawImage.texture = myTexture;

            raw.texture = myTexture;

            //byte[] textuerData = convertedTexture.EncodeToJPG();
            //if (Directory.Exists(pythonDirectory) == false)
            //{
            //    Directory.CreateDirectory(pythonDirectory);
            //}
            //File.WriteAllBytes(pythonDirectory + "/cardImage.jpg", textuerData);
        }
    }
    void AddWork(Texture2D texture)
    {
        GameObject work = Instantiate(majorWorkPrefab, majorWorkContent);
        work.GetComponent<RawImage>().texture = texture;
    }

    public string WriteResult(string[] paths)
    {
        string result = "";
        if (paths.Length == 0)
        {
            return "";
        }
        foreach (string p in paths)
        {
            result += p + "\n";
        }
        return result;
    }

    public void AddMajorWork()
    {
        OpenFileExplorer();
    }
    public void OnClickMajorWork()
    {
        panelMajorWorkEdit.SetActive(true);
        majorWorkDetail.texture = EventSystem.current.currentSelectedGameObject.GetComponentInParent<RawImage>().texture;
        currClickedMajorWorkItem = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
    }
    public void OnClickMajorWorkDelete()
    {
        Destroy(currClickedMajorWorkItem);
        panelMajorWorkEdit.SetActive(false);
    }

    public void OnClickBtnEdit()
    {
        btnEdit.SetActive(false);
        btnSave.SetActive(true);

        for(int i = 0; i < inputFieldList.Count; i++)
        {
            inputFieldList[i].text = textList[i].text;
        }

        inputNickname.SetActive(true);
        btnCheckNickname.SetActive(true);
        btnChangePhoneNum.SetActive(true);
        textPW.SetActive(true);
        btnWithdraw.SetActive(true);
        inputDetailMajor.SetActive(true);
        inputKeyword.SetActive(true);
        inputSpecialty.SetActive(true);
        inputVision.SetActive(true);
        inputUrl.SetActive(true);
        btnChangeDepart.SetActive(true);

        textNickname.SetActive(false);
        textDetailMajor.SetActive(false);
        textKeyword.SetActive(false);
        textSpecialty.SetActive(false);
        textVision.SetActive(false);


        
    }

    public void OnClickBtnSave()
    {
        btnEdit.SetActive(true);
        btnSave.SetActive(false);

        for(int i = 0; i < inputFieldList.Count; i++)
        {
            textList[i].text = inputFieldList[i].text;
        }

        inputNickname.SetActive(false);
        btnCheckNickname.SetActive(false);
        btnChangePhoneNum.SetActive(false);
        textPW.SetActive(false);
        btnWithdraw.SetActive(false);
        inputDetailMajor.SetActive(false);
        inputKeyword.SetActive(false);
        inputSpecialty.SetActive(false);
        inputVision.SetActive(false);
        inputUrl.SetActive(false);
        btnChangeDepart.SetActive(false);

        textNickname.SetActive(true);
        textDetailMajor.SetActive(true);
        textKeyword.SetActive(true);
        textSpecialty.SetActive(true);
        textVision.SetActive(true);
    }
}
