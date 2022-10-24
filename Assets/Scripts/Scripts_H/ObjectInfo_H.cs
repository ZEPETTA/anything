using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo_H : MonoBehaviour
{
    public ObjectInfo objectInfo;
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
        
    }
    public void OnPlayerCall()
    {
        if(objectInfo.objSkill == ObjectInfo.ObjectSkill.urlObj)
        {
            UrlObj();
        }
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
