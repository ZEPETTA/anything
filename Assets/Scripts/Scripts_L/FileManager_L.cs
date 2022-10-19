using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;

public class FileManager_L : MonoBehaviour
{
    public MeshRenderer bg;
    byte[] mapImage;
    string path = "";
    string savepth;
    // Start is called before the first frame update
    void Start()
    {
        savepth = Application.dataPath + "/Resources/Resources_H/mapData.png";
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
            Texture2D convertedTexture = (Texture2D)myTexture;
            byte[] textuerData = convertedTexture.EncodeToPNG();
            File.WriteAllBytes(savepth, textuerData);
            mapImage = textuerData;
            bg.material.SetTexture("_MainTex", convertedTexture);
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
        }
    }
    public void SaveMap()
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path);

    }
}
