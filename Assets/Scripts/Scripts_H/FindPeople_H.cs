using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPeople_H : MonoBehaviour
{
    bool CalenderOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
