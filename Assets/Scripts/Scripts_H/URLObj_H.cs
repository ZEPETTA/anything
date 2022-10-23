using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLObj_H : MonoBehaviour
{
    public string urlLink = "https://docs.google.com/spreadsheets/d/1LatmQjWUM82l0rmQCsufMPkWkK_grPDbwxdeIKchP-M/edit#gid=342731973";
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GoToURL()
    {
        Application.OpenURL(urlLink);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
