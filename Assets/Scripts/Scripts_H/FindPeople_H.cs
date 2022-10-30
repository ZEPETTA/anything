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
        markText.text = "���� �ƴ�\nģ���� �߿���\n�� ģ������ ���ڳ׿�\n�� �̾߱��غ���";
        userText.text = "�����մϴ�!";
        userText.interactable = false;
        searchOn = true;
    }
    public void XButton()
    {
        searchImage.SetActive(false);
        findDialog.SetActive(false);
        userText.interactable = true;
        markText.text = "� �п츦\n�Ұ����� �ٱ��?\n���� �غ���";
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
