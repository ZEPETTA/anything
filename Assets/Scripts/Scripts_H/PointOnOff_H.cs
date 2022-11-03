using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PointOnOff_H : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    public GameObject mouseImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        mouseImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseImage.SetActive(false);
        Debug.Log("Pointer Exit");
    }
}
