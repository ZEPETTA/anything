using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MajorWorkAddBtn_L : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    MyInfoManager_L myInfoManager;

    // Start is called before the first frame update
    void Start()
    {
        myInfoManager = GameObject.FindGameObjectWithTag("MyInfoManager_L").GetComponent<MyInfoManager_L>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerExit(PointerEventData data)
    {
            data.pointerEnter.SetActive(false);
    }

    public void OnPointerClick(PointerEventData data)
    {
        myInfoManager.OnClickMajorWork();
    }
}
