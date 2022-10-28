using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar_H : MonoBehaviour
{
    public GameObject calendar;
    bool CalenderOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CalenderOn == true && Input.GetKeyDown(KeyCode.F))
        {
            calendar.GetComponent<CalendarController>().ShowCalendar();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(true);
            CalenderOn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CharacterMove_H character = collision.GetComponent<CharacterMove_H>();
            character.pressFKey.SetActive(false);
            CalenderOn = false;
        }

    }
}
