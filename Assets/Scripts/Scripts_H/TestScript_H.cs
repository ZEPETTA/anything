using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo
{
    public Vector3 Position;
}
public class TestScript_H : MonoBehaviour
{
    private WebViewObject webViewObject;

   


    public void Click()
    {
        Application.OpenURL("https://www.acmicpc.net");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartWebView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartWebView()
    {
        string strUrl = "https://www.google.com/";
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init((msg) => { Debug.Log(string.Format("CallFromJS[{0}]", msg)); });
        webViewObject.LoadURL(strUrl);
        webViewObject.SetVisibility(true); 
        webViewObject.SetMargins(50, 50, 50, 50);
    }
}
