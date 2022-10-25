using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfo_H : MonoBehaviour
{
    public ObjectInfo objectInfo;
    GameObject speechBubble;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objectInfo.upperObj == true)
        {
            GetComponent<MeshRenderer>().sortingLayerName = "UpperObject";
        }
        else if(objectInfo.upperObj == false)
        {
            GetComponent<MeshRenderer>().sortingLayerName = "Object";
        }
        if(objectInfo.objSkill == ObjectInfo.ObjectSkill.talkingObj)
        {
            speechBubble = transform.Find("SpeechCanvas").GetChild(0).gameObject;
        }
        if(objectInfo.image.Length > 0)
        {
            Texture2D texture = new Texture2D(objectInfo.objWidth,objectInfo.objHeight);
            texture.LoadImage(objectInfo.image);
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (Texture)texture);
        }
    }
    public void OnPlayerCall()
    {
        if(objectInfo.objSkill == ObjectInfo.ObjectSkill.urlObj)
        {
            UrlObj();
        }
        else if (objectInfo.objSkill == ObjectInfo.ObjectSkill.changeObj)
        {
            ChangeObj();
        }
        else if(objectInfo.objSkill == ObjectInfo.ObjectSkill.talkingObj)
        {
            SpeechObj();
        }
    }
    void SpeechObj()
    {
        speechBubble.SetActive(true);
        speechBubble.transform.GetChild(0).GetComponent<Text>().text = objectInfo.talkingSkill;
    }
    void ChangeObj()
    {
        Texture2D objTexture = new Texture2D(objectInfo.objWidth,objectInfo.objHeight);
        objTexture.LoadImage(objectInfo.imageSkill);
        gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", objTexture);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(objectInfo.interactionType == ObjectInfo.InteractionType.touch)
        {
            OnPlayerCall();
        }
    }
    void UrlObj()
    {
        Application.OpenURL(objectInfo.urlSkill);
    }
    
}
