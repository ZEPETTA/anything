using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class FileManager_L : MonoBehaviour
{
    public MeshRenderer bg;
    byte[] mapImage;
    string path = "";
    string savepth;
    int mapWidth;
    int mapHeight;
    // Start is called before the first frame update
    void Start()
    {
        savepth = Application.dataPath + "/Resources/Resources_H/MapData";
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
        
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D convertedTexture = (Texture2D)myTexture;
            mapHeight = convertedTexture.height;
            mapWidth = convertedTexture.width;
            byte[] textuerData = convertedTexture.EncodeToPNG();
            if (Directory.Exists(savepth) == false)
            {
                Directory.CreateDirectory(savepth);
            }
            File.WriteAllBytes(savepth + "/mapData.png", textuerData);
            mapImage = textuerData;
            bg.material.SetTexture("_MainTex", convertedTexture);
        }
    }

    IEnumerator IESetFG()
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
        }
    }
    public void SaveMap()
    {
        MapInfo backGroundInfo = new MapInfo();
        backGroundInfo.backGroundImage = mapImage;
        backGroundInfo.mapWidth = mapWidth;
        backGroundInfo.mapHeight = mapHeight;
        string jsonMap = JsonUtility.ToJson(backGroundInfo,true);
        string path = Application.dataPath + "/Resources/Resources_H/MapData";
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/mapdata.txt", jsonMap);
        SceneManager.LoadScene("RoomScene_H");
    }
}
