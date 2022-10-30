using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MajorWorkItem_L : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData data)
    {
        print(data.pointerEnter.name);
        if (data.pointerEnter.tag == "MajorArtWorkItem")
            data.pointerEnter.transform.GetChild(1).gameObject.SetActive(true);
    }


}
