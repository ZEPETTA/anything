using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PointOnOff_H : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    public Room_H room_H;
    GameObject mouseImage;
    private void Start()
    {
        mouseImage = GameObject.Find("PanelCanvas").transform.GetChild(4).gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseImage.SetActive(true);
        mouseImage.GetComponent<FollowMouseUI_H>().introString = room_H.roomInfoText;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseImage.SetActive(false);
    }
}
