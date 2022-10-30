using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindPeople_H : MonoBehaviour
{
    public GameObject findDialog;
    public Text markText;
    public InputField userText;
    public GameObject searchImage;
    bool CalenderOn = false;
    bool searchOn = false;
    int SearchCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        userText.onSubmit.AddListener(OnUserTextValueChanged);
    }

    void OnUserTextValueChanged(string userT)
    {
        markText.text = "내가 아는\n친구들 중에는\n이 친구들이 낫겠네요\n잘 이야기해봐요";
        userText.text = "감사합니다!";
        userText.interactable = false;
        searchOn = true;
    }
    public void XButton()
    {
        searchImage.SetActive(false);
        findDialog.SetActive(false);
        userText.interactable = true;
        markText.text = "어떤 학우를\n소개시켜 줄까요?\n말만 해봐요";
        userText.text = "";
        searchOn = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CalenderOn == true && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Hello??");
            findDialog.SetActive(true);
        }
        if(searchOn == true && Input.GetKeyDown(KeyCode.Return))
        {
            SearchCount++;
            if(SearchCount > 1)
            {
                searchImage.SetActive(true);
                searchOn = false;
                SearchCount = 0;
            }
            
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
