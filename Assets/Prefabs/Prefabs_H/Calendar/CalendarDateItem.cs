using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour {

    public Text scheduleText;
    public GameObject scheduleInputfield;
    public InputField inputField;
    public CalendarController calendarController;
    private void Start()
    {
        calendarController = transform.parent.parent.parent.GetComponent<CalendarController>();
        //inputField.onValueChanged.AddListener(OnInputText);
        //inputField.onEndEdit.AddListener(EndInput);
        inputField.onSubmit.AddListener(EndInput);
    }
    public void OnDateItemClick()
    {
        for(int i =0; i < calendarController._dateItems.Count; i++)
        {
            calendarController._dateItems[i].GetComponent<CalendarDateItem>().scheduleInputfield.SetActive(false);
        }
        scheduleInputfield.SetActive(true);
        //CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }
    public void OnInputText(string b)
    {
        scheduleText.text = b;
    }
    public void EndInput(string b)
    {
        scheduleInputfield.SetActive(false);
        inputField.text = "";
        if(scheduleText.text.Length < 1)
        {
            scheduleText.text = b;
        }
        else
        {
            scheduleText.text += "\n" + b;
        }
    }
}
