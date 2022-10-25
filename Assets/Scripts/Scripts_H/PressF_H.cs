using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressF_H : MonoBehaviour
{
    ObjectInfo objinfo;
    // Start is called before the first frame update
    void Start()
    {
        objinfo = gameObject.transform.parent.GetComponent<ObjectInfo_H>().objectInfo;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(objinfo == null)
        {
            return;
        }
        if(objinfo.interactionType == ObjectInfo.InteractionType.touch)
        {
            return;
        }
        if(collision.tag == "Player" && objinfo.interactionType == ObjectInfo.InteractionType.pressF)
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(true);
            character.objInfo = transform.parent.GetComponent<ObjectInfo_H>();
            character.inObj = true;
        }
        if(objinfo.objName.Length > 0)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = objinfo.objName;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objinfo.interactionType == ObjectInfo.InteractionType.touch)
        {
            return;
        }
        if (collision.tag == "Player" && objinfo.interactionType == ObjectInfo.InteractionType.pressF)
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(false);
            character.inObj = false;
        }
        if (objinfo.objName.Length > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
