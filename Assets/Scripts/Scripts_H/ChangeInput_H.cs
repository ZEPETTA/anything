using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChangeInput_H : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    public Button submitButton;
    List<Selectable> selectables = new List<Selectable>();
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // 엔터키를 치면 로그인 (제출) 버튼을 클릭
            //submitButton.onClick.Invoke();
            Debug.Log("Button pressed!");
        }
    }
}
