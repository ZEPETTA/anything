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

    // Start is called before the first frame update
    void Start()
    {
        
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

            GameObject work = Instantiate(majorWorkPrefab, majorWorkContent);
            work.GetComponent<RawImage>().texture = myTexture;

            //byte[] textuerData = convertedTexture.EncodeToJPG();
            //if (Directory.Exists(pythonDirectory) == false)
            //{
            //    Directory.CreateDirectory(pythonDirectory);
            //}
            //File.WriteAllBytes(pythonDirectory + "/cardImage.jpg", textuerData);
        }
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
}
