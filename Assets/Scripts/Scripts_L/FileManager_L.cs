using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;

public class FileManager_L : MonoBehaviour
{
    public RawImage myBG;
    public RawImage myFG;

    string path = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickSetBG()
    {
        path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        StartCoroutine(IESetBG());
    }

    public void OnClickSetFG()
    {
        path = EditorUtility.OpenFilePanel("Show all images(.png)", "", "png");
        StartCoroutine(IESetFG());
    }

    IEnumerator IESetBG()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            myBG.texture = myTexture;
        }
    }

    IEnumerator IESetFG()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            myFG.texture = myTexture;
        }
    }
   
}
